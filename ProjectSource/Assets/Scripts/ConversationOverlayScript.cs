using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Colour = UnityEngine.Color;

public class ConversationOverlayScript : MonoBehaviour
{
    private TextMeshProUGUI _convoTextArea;
    private Camera _convoCamera;


    [SerializeField] private int numDialogLines;
    [SerializeField] private List<string> dialogRaw;
    [SerializeField] private bool debugMode;
    
    
    private List<List<string>> _dialogList;
    private List<string> _dialogBox;
    private bool _isFading;
    private Image _fadeOutImage;

    private void Awake()
    {
        _convoTextArea = GameObject.FindWithTag("ConversationTextArea").GetComponent<TextMeshProUGUI>();
        _convoCamera = GameObject.FindWithTag("ConversationCamera").GetComponent<Camera>();
        _fadeOutImage = GameObject.FindWithTag("ConversationFadeOut").GetComponent<Image>();

        _dialogList = new List<List<string>>();
        foreach (string text in dialogRaw)
        {
            _dialogList.Add(text.Split('|').ToList());
        }

        _dialogBox = new List<string>();
        for (int i = 0; i < numDialogLines; i++)
        {
            _dialogBox.Add("");
        }
        
        // --------------------------------------------------
        if (debugMode)
        {
            FadeOut();
        }
    }

    public void RunNextDialogSequence()
    {
        if (_dialogList.Count == 0)
        {
            _convoTextArea.text = "Game Over\nThanks for playing!\n\n\n\n";
            return;
        }

        _dialogBox.Clear();
        for (int i = 0; i < numDialogLines; i++)
        {
            _dialogBox.Add("");
        }

        InvokeRepeating(nameof(DialogLoop), 1f, 3f);
    }

    public void FadeOut()
    {
        if (_isFading)
        {
            return;
        }

        _isFading = true;
        InvokeRepeating(nameof(FadeOutLoop), 0f, 0.25f);
    }

    public void FadeIn()
    {
        if (_isFading)
        {
            return;
        }

        _isFading = true;
    }

    private void FadeOutLoop()
    {
        Colour colour = _fadeOutImage.color;
        float newAlpha = colour.a + 0.1f;
        if (newAlpha >= 1.0f)
        {
            newAlpha = 1.0f;
            CancelInvoke(nameof(FadeOutLoop));
            _convoCamera.enabled = false;
        }
        colour = new Colour(colour.r, colour.g, colour.b, newAlpha);
        _fadeOutImage.color = colour;
    }

    private void DialogLoop()
    {
        List<string> currentDialog = _dialogList[0];
        if (currentDialog.Count == 0)
        {
            CancelInvoke(nameof(DialogLoop));
            _dialogList.RemoveAt(0);
            return;
        }

        string message = currentDialog[0];
        currentDialog.RemoveAt(0);
        _dialogBox.RemoveAt(0);
        _dialogBox.Add(message);
        UpdateDialogBox();
    }

    private void UpdateDialogBox()
    {
        _convoTextArea.text = string.Join("\n", _dialogBox);
    }
}