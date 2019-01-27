using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

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

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _playerMovementScript = _player.GetComponent<Movement>();
        _camera = GetComponent<Camera>();
        _listener = GetComponent<AudioListener>();
        _convoOverlayScript = GameObject.FindWithTag("ConversationOverlay")?.GetComponent<ConversationOverlayScript>();
        _scoreText = transform.Find("Canvas").Find("Score").Find("ScoreValue").GetComponent<TextMeshProUGUI>();

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
        _playerMovementScript.IsMovementAllowed = true;
        _fireflyCount = currentSpawnLocation.NumFireflies;
        _camera.enabled = true;
        _listener.enabled = true;
        _scoreText.text = _fireflyCount.ToString();
        
        // ---------------------------- DEBUG ---------------------------
        if (debugMode)
        {
            for (int i = 0; i < currentSpawnLocation.NumFireflies; i++)
            {
                Invoke(nameof(MarkFireflyCaught), i + 1f);
            }
        }
    }

    public void FadeOut()
    {
        _playerMovementScript.IsMovementAllowed = false;
        _camera.enabled = false;
        _listener.enabled = false;
        _convoOverlayScript.FadeIn();
    }
}