using UnityEditor;
using UnityEngine;

public class BrickBehavior : MonoBehaviour

{
    private SpriteRenderer _spriteRenderer;
    private int _health;

    public int Health
    {
        get
        {
            return _health;
        }

        set
        {
            _health = value;

            if (_health < _colors.Length)
            {

                _spriteRenderer.color = _colors[_health];
            }
        }
    }

    [SerializeField] private Color[] _colors;


    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Health++;

            if (Health > 3)
            {
                GameBehavior.Instance.Score += 100;
                Debug.Log(GameBehavior.Instance.Score);
                Destroy(gameObject);
            }
        }
    }
} 

