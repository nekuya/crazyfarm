using UnityEngine;

public abstract class InteractableWSign : Interactable
{
    [SerializeField] private Highlighter highlighter;
    [SerializeField] private CanvasGroup inputCanvas;

    public override Interactor ActiveInteractor
    {
        get { return base.ActiveInteractor; }

        protected set
        {
            base.ActiveInteractor = value;

            inputCanvas.alpha = ActiveInteractor ? 0.5f : 1f;
        }
    }

    public override void InteractorEnter()
    {
        highlighter.IsOn = true;
        inputCanvas.alpha = 1f;
    }

    public override void InteractorExit()
    {
        highlighter.IsOn = false;
        inputCanvas.alpha = 0f;
    }
}