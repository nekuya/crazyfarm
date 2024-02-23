///-----------------------------------------------------------------
///   Author : Gabriel Bernabeu                    
///   Date   : 17/05/2022 10:27
///-----------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// WARNING: JsonUtility will only use serialized data, that is: Serializable structs, public members...
/// If you do not need TInput / TOutput, use string type.
/// </summary>
/// <typeparam name="TInput">Lists of structs & structs are supported.</typeparam>
/// <typeparam name="TOutput">Lists of structs & structs are supported.</typeparam>
public class WebQuery<TInput, TOutput>
{
    public delegate void WebQueryEventHandler(S_Response response);

    protected const int HTTP_UNAUTHORIZED_CODE = 401;
    protected const string SERVER_URL = "Replace with URL";

    public readonly string localPath;
    public readonly E_QueryType type;
    public readonly TInput sentData;

    public S_Response Response => _response;
    private S_Response _response;

    public event WebQueryEventHandler OnComplete;

    /// <param name="sentData">Not used if left to default.</param>
    public WebQuery(string localPath = "/", E_QueryType type = E_QueryType.GET, TInput sentData = default)
    {
        this.localPath = localPath;
        this.type = type;
        this.sentData = sentData;
    }

    private bool IsTypeList(Type type) => type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>));

    #region Task-based (Does NOT work with WebGL!)

    public async void Send_TaskBased(WebQueryEventHandler callback)
    {
        OnComplete = callback;
        await Communicate_TaskBased();
    }

    public async Task<S_Response> Send_TaskBased()
    {
        return await Communicate_TaskBased();
    }

    private async Task<S_Response> Communicate_TaskBased()
    {
        UnityWebRequest lRequest = InitRequest();
        UnityWebRequestAsyncOperation lOperation = lRequest.SendWebRequest();

        while (!lOperation.isDone)
            await Task.Yield();

        ParseResponse(lRequest);
        OnComplete?.Invoke(Response);
        return Response;
    }

    #endregion

    #region Coroutine-based

    /// <summary>
    /// WebQuery's "coroutineStarter" needs to be set!
    /// </summary>
    public Coroutine Send_CoroutineBased(WebQueryEventHandler callback)
    {
        OnComplete = callback;
        return WebQuery.coroutineStarter.StartCoroutine(Communicate_CoroutineBased());
    }

    /// <summary>
    /// WebQuery's "coroutineStarter" needs to be set!
    /// </summary>
    public Coroutine Send_CoroutineBased()
    {
        return WebQuery.coroutineStarter.StartCoroutine(Communicate_CoroutineBased());
    }

    private IEnumerator Communicate_CoroutineBased()
    {
        UnityWebRequest lRequest = InitRequest();
        UnityWebRequestAsyncOperation lOperation = lRequest.SendWebRequest();

        yield return new WaitWhile(() => !lOperation.isDone);

        ParseResponse(lRequest);
        OnComplete?.Invoke(Response);
    }

    #endregion

    private UnityWebRequest InitRequest()
    {
        byte[] lSentData = new byte[] { };

        //If data member was not initialized (= default), do not fill lSentData.
        if (!EqualityComparer<TInput>.Default.Equals(sentData, default))
        {
            string lStringifiedData;

            if (IsTypeList(typeof(TInput)))
                lStringifiedData = JsonUtility.ToJson(new DataContainer<TInput>(sentData));
            else
                lStringifiedData = JsonUtility.ToJson(sentData);

            lSentData = System.Text.Encoding.UTF8.GetBytes(lStringifiedData);
        }

        //Set the verb according to the chosen type enum.
        string lHttpVerb = UnityWebRequest.kHttpVerbGET;

        switch (type)
        {
            case E_QueryType.GET:
                lHttpVerb = UnityWebRequest.kHttpVerbGET;
                break;
            case E_QueryType.POST:
                lHttpVerb = UnityWebRequest.kHttpVerbPOST;
                break;
            case E_QueryType.PATCH:
                lHttpVerb = "PATCH";
                break;
            case E_QueryType.DELETE:
                lHttpVerb = UnityWebRequest.kHttpVerbDELETE;
                break;
        }

        //Communicate with the server.
        UnityWebRequest lRequest = new UnityWebRequest(SERVER_URL + localPath, lHttpVerb);
        lRequest.SetRequestHeader("Authorization", "Bearer " + WebQuery.Token);
        lRequest.uploadHandler = new UploadHandlerRaw(lSentData);
        lRequest.downloadHandler = new DownloadHandlerBuffer();
        lRequest.uploadHandler.contentType = "application/json";

        return lRequest;
    }

    protected void ParseResponse(UnityWebRequest request)
    {
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            if (request.responseCode == HTTP_UNAUTHORIZED_CODE)
                _response.type = E_ResponseType.UNAUTHORIZED;
            else
                _response.type = E_ResponseType.ERROR;
        }
        else
        {
            try
            {
                if (IsTypeList(typeof(TOutput)))
                {
                    _response.data = JsonUtility.FromJson<DataContainer<TOutput>>(
                        AddContainerToJson(request.downloadHandler.text)).data;
                }
                else
                    _response.data = JsonUtility.FromJson<TOutput>(request.downloadHandler.text);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to deserialize received json data: {e.Message} \n" +
                                    $"If the received data isn't a json, this is not a problem.");
            }

            _response.type = E_ResponseType.SUCCESS;
        }

        _response.info = request.responseCode + " ~ " + request.downloadHandler.text;
        request.Dispose();
    }

    public struct S_Response
    {
        public E_ResponseType type;
        public TOutput data;

        /// <summary>
        /// Information about the query, with responseCode & downloadHandler.
        /// </summary>
        public string info;
    }

    /// <summary>
    /// Used for supporting Lists in JsonUtility.
    /// </summary>
    /// <typeparam name="TDataType"></typeparam>
    [Serializable]
    private struct DataContainer<TDataType>
    {
        public TDataType data;
        public DataContainer(TDataType data)
        {
            this.data = data;
        }
    }

    private static string AddContainerToJson(string json) => "{\"data\":" + json + "}";

    private static string RemoveContainerFromJson(string json)
    {
        Match lMatch = Regex.Match(json, @"^{\""data\"":(.+)}$");
        return lMatch.Success ? lMatch.Groups[1].Value : json;
    }
}

public static class WebQuery
{
    /// <summary>
    /// Mandatory for Coroutine-based methods!
    /// </summary>
    public static MonoBehaviour coroutineStarter;

    //Debug, to improve
    public static string Token => "Replace with token variable";
}

public enum E_QueryType
{
    GET,
    POST,
    PATCH,
    DELETE
}

public enum E_ResponseType
{
    NONE, SUCCESS, ERROR, UNAUTHORIZED
}