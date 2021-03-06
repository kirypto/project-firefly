﻿using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Colour = UnityEngine.Color;

public class ConversationOverlayScript : MonoBehaviour
{
    private TextMeshProUGUI _convoTextArea;
    private Camera _convoCamera;
    private MainCameraScript _mainCameraScript;
    private AudioListener _convoListener;
    private AudioSourceController _convoSource;


    [SerializeField] private int numDialogLines;
    [SerializeField] private List<string> dialogRaw;


    private List<List<string>> _dialogList;
    private List<string> _dialogBox;
    private bool _isFading;
    private Image _fadeOutImage;

    private void Awake()
    {
        _convoTextArea = GameObject.FindWithTag("ConversationTextArea").GetComponent<TextMeshProUGUI>();
        _convoCamera = GameObject.FindWithTag("ConversationCamera").GetComponent<Camera>();
        _fadeOutImage = GameObject.FindWithTag("ConversationFadeOut").GetComponent<Image>();
        _mainCameraScript = GameObject.FindWithTag("MainCamera").GetComponent<MainCameraScript>();
        _convoListener = GameObject.FindWithTag("ConversationCamera").GetComponent<AudioListener>();
        _convoSource = GameObject.FindWithTag("ConversationCamera").GetComponent<AudioSourceController>();

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
        
        ClearDialogBox();
    }

    private void Start()
    {
        Invoke(nameof(RunNextDialogSequence), 2f);
    }

    public void RunNextDialogSequence()
    {
        if (_dialogList.Count == 0)
        {
            _convoTextArea.text = "\n\n" +
                                  "Game Over\n" +
                                  "Thanks for playing!\n\n" +
                                  "Authors:\n" +
                                  " - Jessica Berry\n" +
                                  " - Garrett Hansen\n" +
                                  " - Bennett Lewis\n" +
                                  " - Robin Lowe\n" +
                                  " - Killian Stacey\n" +
                                  "";
            return;
        }

        ClearDialogBox();

        InvokeRepeating(nameof(DialogLoop), 3f, 1.5f);
    }

    private void ClearDialogBox()
    {
        _dialogBox.Clear();
        for (int i = 0; i < numDialogLines; i++)
        {
            _dialogBox.Add("");
        }
        UpdateDialogBox();
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

        ClearDialogBox();
        _convoCamera.enabled = true;
        _convoListener.enabled = true;
        _isFading = true;
        InvokeRepeating(nameof(FadeInLoop), 0f, 0.25f);
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
            _convoListener.enabled = false;
            _isFading = false;
            _mainCameraScript.FadeIn();
        }

        colour = new Colour(colour.r, colour.g, colour.b, newAlpha);
        _fadeOutImage.color = colour;
    }

    private void FadeInLoop()
    {
        Colour colour = _fadeOutImage.color;
        float newAlpha = colour.a - 0.1f;
        if (newAlpha <= 0.0f)
        {
            newAlpha = 0.0f;
            CancelInvoke(nameof(FadeInLoop));
            _isFading = false;
            RunNextDialogSequence();
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
            Invoke(nameof(FadeOut), 1f);
            return;
        }

        string message = currentDialog[0];
        currentDialog.RemoveAt(0);
        _dialogBox.RemoveAt(0);
        _dialogBox.Add(message);
        UpdateDialogBox();
        if(message.Contains("Granny")) {
            _convoSource.Play("Granny");
        } else if (message.Contains("Theo")) {
            _convoSource.Play("Theo");
        }
    }

    private void UpdateDialogBox()
    {
        _convoTextArea.text = string.Join("\n", _dialogBox);
    }
}