using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FakeTrail : MonoBehaviour
{
    [SerializeField] private float pointLifetime = 5f; //lifetime of a point on the trail
    [SerializeField] private float minPointDistance = 0.1f; //minimum distance moved before a new point is solidified.
    [SerializeField] private Vector3 fakeVelocity; //direction the points are moving

    private LineRenderer _line;
    //position data
    private List<Vector3> points;
    private Queue<float> spawnTimes = new Queue<float>(); 
    //list of spawn times, to simulate lifetime. Back of the queue is vertex 1, front of the queue is the end of the trail.
    //Length of this queue is one less than the number of positions in the linerenderer, since the linerenderer always has a non-solidified vertex at the object's position.

    // Use this for initialization
    void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _line.useWorldSpace = true;
        points = new List<Vector3>() { transform.position }; //indices 1 - end are solidified points, index 0 is always transform.position
        _line.SetPositions(points.ToArray());
    }

    void AddPoint(Vector3 position)
    {
        points.Insert(1, position);
        spawnTimes.Enqueue(Time.time);
    }

    void RemovePoint()
    {
        spawnTimes.Dequeue();
        points.RemoveAt(points.Count - 1); //remove corresponding oldest point at the end
    }

    void Update()
    {
        //cull based on lifetime
        while (spawnTimes.Count > 0 && spawnTimes.Peek() + pointLifetime < Time.time)
        {
            RemovePoint();
        }

        //move positions
        Vector3 diff = -fakeVelocity * Time.deltaTime;
        for (int i = 1; i < points.Count; i++)
        {
            points[i] += diff;
        }

        //add new point
        if (points.Count < 2 || Vector3.Distance(transform.position, points[1]) > minPointDistance)
        {
            //if we have no solidified points, or we've moved enough for a new point
            AddPoint(transform.position);
        }

        //update index 0;
        points[0] = transform.position;

        //save result
        _line.positionCount = points.Count;
        _line.SetPositions(points.ToArray());
    }
}