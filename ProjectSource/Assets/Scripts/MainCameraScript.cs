using System;
using UnityEngine;

public class MainCameraScript : MonoBehaviour
{
    private GameObject _player;
    private Camera _camera;
    private bool _isPlayerNull;
    private ConversationOverlayScript _convoOverlayScript;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _camera = GetComponent<Camera>();
        _convoOverlayScript = GameObject.FindWithTag("ConversationOverlay").GetComponent<ConversationOverlayScript>();

        if (_player == null)
        {
            _isPlayerNull = true;
            throw new ArgumentNullException($"The player object is not in the scene.");
        }
    }

    private void Update()
    {
        if (_isPlayerNull)
        {
            return;
        }

        transform.position = _player.transform.position;
    }

    public void FadeIn()
    {
        _camera.enabled = true;
    }

    public void FadeOut()
    {
        _camera.enabled = false;
        _convoOverlayScript.FadeIn();
    }
}