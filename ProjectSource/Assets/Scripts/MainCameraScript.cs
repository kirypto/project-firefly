using System.Collections.Generic;
using System.Linq;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;
using Colour = UnityEngine.Color;

public class MainCameraScript : MonoBehaviour
{
    [SerializeField] private bool debugMode;
    
    private GameObject _player;
    private Camera _camera;
    private AudioListener _listener;
    private bool _isPlayerNull;
    private ConversationOverlayScript _convoOverlayScript;
    private TextMeshProUGUI _scoreText;
    private int _fireflyCount;

    private List<SpawnLocationScript> _spawnLocations;
    private Movement _playerMovementScript;
    private Image _fadeOutImage;
    private bool _isFading;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _playerMovementScript = _player.GetComponent<Movement>();
        _camera = GetComponent<Camera>();
        _listener = GetComponent<AudioListener>();
        _convoOverlayScript = GameObject.FindWithTag("ConversationOverlay")?.GetComponent<ConversationOverlayScript>();
        _scoreText = transform.Find("Canvas").Find("Score").Find("ScoreValue").GetComponent<TextMeshProUGUI>();
        _fadeOutImage = transform.Find("Canvas").Find("FadeOutPanel").GetComponent<Image>();

        if (_player == null)
        {
            _isPlayerNull = true;
            Debug.LogError("No Player object found in the scene!");
        }

        _spawnLocations = GameObject.FindGameObjectsWithTag("SpawnLocation")
                .Select(spawnLocationObject => spawnLocationObject.GetComponent<SpawnLocationScript>())
                .ToList();
        _spawnLocations.Sort((spawnLocation1, spawnLocation2) => spawnLocation1.SpawnIndex.CompareTo(spawnLocation2.SpawnIndex));

        if (_spawnLocations.Count == 0)
        {
            Debug.LogError("No SpawnLocation objects were found in the scene!");
        }
    }

    private void Update()
    {
        if (_isPlayerNull)
        {
            return;
        }

        Vector2 playerPos = _player.transform.position;
        transform.position = new Vector3(playerPos.x, playerPos.y, -10f);
    }

    public void MarkFireflyCaught()
    {
        _fireflyCount = Mathf.Max(0, _fireflyCount - 1);
        _scoreText.text = _fireflyCount.ToString();

        if (_fireflyCount == 0)
        {
            FadeOut();
        }
    }

    public void FadeIn()
    {
        if (_spawnLocations.Count == 0)
        {
            FadeOut();
            return;
        }
        SpawnLocationScript currentSpawnLocation = _spawnLocations[0];
        _spawnLocations.RemoveAt(0);
        _playerMovementScript.SpawnAtLocation(currentSpawnLocation.Location);
        _fireflyCount = currentSpawnLocation.NumFireflies;
        _scoreText.text = _fireflyCount.ToString();
        _camera.enabled = true;
        _listener.enabled = true;
        
        InvokeRepeating(nameof(FadeInLoop), 0f, 0.25f);
        
        // ---------------------------- DEBUG ---------------------------
        if (debugMode)
        {
            for (int i = 0; i < currentSpawnLocation.NumFireflies; i++)
            {
                Invoke(nameof(MarkFireflyCaught), i + 3f);
            }
        }
    }

    public void FadeOut()
    {
        _playerMovementScript.IsMovementAllowed = false;
        InvokeRepeating(nameof(FadeOutLoop), 0f, 0.25f);
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
            _playerMovementScript.IsMovementAllowed = true;
        }

        colour = new Colour(colour.r, colour.g, colour.b, newAlpha);
        _fadeOutImage.color = colour;
    }

    private void FadeOutLoop()
    {
        Colour colour = _fadeOutImage.color;
        float newAlpha = colour.a + 0.1f;
        if (newAlpha >= 1.0f)
        {
            newAlpha = 1.0f;
            CancelInvoke(nameof(FadeOutLoop));
            _camera.enabled = false;
            _listener.enabled = false;
            _convoOverlayScript.FadeIn();
            _isFading = false;
        }

        colour = new Colour(colour.r, colour.g, colour.b, newAlpha);
        _fadeOutImage.color = colour;
    }
}