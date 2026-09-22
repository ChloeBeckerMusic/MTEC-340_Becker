using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    [SerializeField] private float _launchForce = 7.0f;
    [SerializeField] private float _speedIncrement = 1.0f;
    private Rigidbody2D _rb;
    [SerializeField, Range(0.0f, 1.0f)] private float _paddleInfluence = 0.4f;

    private AudioSource _source;
    [SerializeField] private AudioClip _wallHit;
    [SerializeField] private AudioClip _paddleHit;
    [SerializeField] private AudioClip _brickHit;

    // ---------------------------------------------------------------------------------------------
    // ---------------------------------------------------------------------------------------------

    void Start()
    {
       _rb = GetComponent<Rigidbody2D>();
       _source = GetComponent<AudioSource>();

       Vector2 direction = Random.insideUnitCircle.normalized;
       
       if (Mathf.Abs(direction.y) < 0.4f)       
       {
           direction.y += 0.4f * Mathf.Sign(direction.y);    
       }

       _rb.AddForce(direction * _launchForce, ForceMode2D.Impulse);
    }

    // ---------------------------------------------------------------------------------------------

    void Update()
    {
        _rb.simulated = GameBehavior.Instance.State == Utilities.GameState.Play;
        // boolean and means if we're in play, keep simulated physics on,
        // and if not, stop the physics 
    }

    // ---------------------------------------------------------------------------------------------

    private void OnCollisionEnter2D(Collision2D other)
    {
        Vector2 velocity = _rb.linearVelocity;

        if (Mathf.Abs(velocity.y) < 1.0f)
        {
            velocity.y = velocity.y >= 0 ? 1.0f : -1.0f;

            _rb.linearVelocity =
                velocity.normalized * _rb.linearVelocity.magnitude;
        }

        if (other.gameObject.CompareTag("Paddle"))
        {
            // CHLOE COMPREHENSION NOTE: if the paddle's horizontal velocity is NOT zero,
            // then add this weight paddle influence

            if (!Mathf.Approximately(other.rigidbody.linearVelocity.x, 0.0f))
            {
                //Weighted sum using one-minus to calculate weights
                Vector2 direction = _rb.linearVelocity * (1.0f - _paddleInfluence)
                                    + other.rigidbody.linearVelocity * _paddleInfluence; 
                _rb.linearVelocity = _rb.linearVelocity.magnitude * direction.normalized * _speedIncrement; 

            }
        
            _source.clip = _paddleHit;

        }
         
        // -----

        else if (other.gameObject.CompareTag("BrickBluePrefab") || other.gameObject.CompareTag("BrickOrangePrefab") || other.gameObject.CompareTag("BrickYellowPrefab"))
        {
            _source.clip = _brickHit;
        }  

        // -----

        else
        {   
            _source.clip = _wallHit;
            _source.Play();

        }
        _source.pitch = Random.Range(0.9f, 1.1f);
        _source.Play();
    }

    // ---------------------------------------------------------------------------------------------

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("OutOfBounds"))
        {
            Debug.Log("Ball fell out!");

            StartCoroutine(GameBehavior.Instance.ResetAfterOutOfBounds());

        }
    }

    // ---------------------------------------------------------------------------------------------

}