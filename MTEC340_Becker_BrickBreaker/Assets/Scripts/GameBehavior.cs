using System.Collections;
using UnityEngine;
using TMPro;

public class GameBehavior : MonoBehaviour
{
    public static GameBehavior Instance;
    // GameBehavior here is like an access point (because of the word static)
    // static means that hey this line of code belongs to the CLASS,
    // not to the instance-- there is only ONE instance

    private Utilities.GameState _state;

    public Utilities.GameState State
    {
        get => _state;

        set
        {
           _state = value;

           _message.enabled = State == Utilities.GameState.Pause;
        }

    }

    [SerializeField] private TMP_Text _message; 
    private float _durationBetweenPoints = 0.3f;
    
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _outOfBounds;
    
    private GameObject _currentBall;
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private TextMeshProUGUI _scoreTextUI;


    private int _score;

    public int Score
    {
        get { return _score; }
        set 
        { 
            _score = value;      
            _scoreTextUI.text = "Score: " + _score.ToString();
        }
    }

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            Debug.Log("New instance initialized...");
		
            DontDestroyOnLoad(gameObject);
        }
        
        else 
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetGame();

        // Set initial state 
        _audioSource = GetComponent<AudioSource>();
        State = Utilities.GameState.Play;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            State = State == Utilities.GameState.Play ? 
                Utilities.GameState.Pause : 
                Utilities.GameState.Play;
        }
    }
    private void SpawnBall()
    {
        _currentBall= Instantiate(_ballPrefab);

    }

    public void ResetGame()
    {
        Debug.Log("Game Reset!");

        if (_currentBall != null)
        {
            Destroy(_currentBall);
        }

        Score = 0;
        //apply a delay when a player scores to give it a respite 
        Invoke(nameof(SpawnBall), _durationBetweenPoints);
        }

    public IEnumerator ResetAfterOutOfBounds()
    {
        _audioSource.PlayOneShot(_outOfBounds, 0.35f);
        yield return new WaitForSeconds(_outOfBounds.length);

        ResetGame();
    }
}




