using UnityEngine;
using System;
using System.Collections;

public class ConversationController : MonoBehaviour
{
    #region Events
    public static event EventHandler completeEvent;
    #endregion

    #region Const
    const string ShowTop = "Show Top";
    const string ShowBottom = "Show Bottom";
    const string HideTop = "Hide Top";
    const string HideBottom = "Hide Bottom";
    #endregion

    #region Fields
    [SerializeField] ConversationPanel leftPanel;
    [SerializeField] ConversationPanel rightPanel;

    Canvas canvas;
    IEnumerator conversation;
    Tweener transition;
    #endregion

    #region MonoBehaviour
    /// <summary>
    /// Connect references, set the off screen position for the panels, and disable the canvas.
    /// </summary>
    void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        if (leftPanel.panel.CurrentPosition == null)
            leftPanel.panel.SetPosition(HideBottom, false);
        if (rightPanel.panel.CurrentPosition == null)
            rightPanel.panel.SetPosition(HideBottom, false);
        canvas.gameObject.SetActive(false);
    }
    #endregion

    #region Public
    /// <summary>
    /// Bring out the panel with the first speaker's message.
    /// </summary>
    /// <param name="data"></param>
    public void Show(ConversationData data)
    {
        canvas.gameObject.SetActive(true);
        conversation = Sequence(data);
        conversation.MoveNext();
    }

    /// <summary>
    /// Trigger new messages and speakers.
    /// </summary>
    public void Next()
    {
        if (conversation == null || transition != null)
            return;

        conversation.MoveNext();
    }
    #endregion

    #region Private
    IEnumerator Sequence(ConversationData data)
    {
        for (int i = 0; i < data.list.Count; ++i)
        {
            SpeakerData sd = data.list[i];

            ConversationPanel currentPanel = (sd.anchor == TextAnchor.UpperLeft || sd.anchor == TextAnchor.MiddleLeft || sd.anchor == TextAnchor.LowerLeft) ? leftPanel : rightPanel;
            IEnumerator presenter = currentPanel.Display(sd);
            presenter.MoveNext();

            // Determine entry and exit points based on the anchor settings.
            string show, hide;
            if (sd.anchor == TextAnchor.UpperLeft || sd.anchor == TextAnchor.UpperCenter || sd.anchor == TextAnchor.UpperRight)
            {
                show = ShowTop;
                hide = HideTop;
            }
            else
            {
                show = ShowBottom;
                hide = HideBottom;
            }

            // Snap the panel off screen before animating it on screen to avoid weird animation paths.
            currentPanel.panel.SetPosition(hide, false);
            MovePanel(currentPanel, show);
            yield return null;

            // Loop through a speaker's messages.
            while (presenter.MoveNext())
                yield return null;

            // The speaker has no more messages so move the current panel off screen and continue the conversation.
            MovePanel(currentPanel, hide);
            transition.easingControl.completedEvent += delegate (object sender, EventArgs e) {
                conversation.MoveNext();
            };

            yield return null;
        }

        // All speakers have completed all their messages.
        canvas.gameObject.SetActive(false);
        if (completeEvent != null)
            completeEvent(this, EventArgs.Empty);
    }

    void MovePanel(ConversationPanel obj, string pos)
    {
        transition = obj.panel.SetPosition(pos, true);
        transition.easingControl.duration = 0.5f;
        transition.easingControl.equation = EasingEquations.EaseOutQuad;
    }
    #endregion
}