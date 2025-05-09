using System.IO;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f;

    private bool _hit = false;
    private int _direction;
    private float _lifeTime;

    private BoxCollider2D _boxCollider;
    private Animator _animator;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();   
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_hit)
        {
            return;
        }
        float movementSpeed = _speed * Time.deltaTime * _direction;
        transform.Translate(movementSpeed, 0, 0);


        _lifeTime += Time.deltaTime;
        if (_lifeTime > Random.Range(1f, 3f))
        {
            _direction = 0;
            _animator.SetTrigger("explode");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _hit = true;
        _boxCollider.enabled = false;
        _animator.SetTrigger("explode");
    }

    public void SetDirection(int direction)
    {
        _lifeTime = 0;
        _direction = direction;
        gameObject.SetActive(true);
        _hit = false;
        _boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        transform.localScale = new Vector3(_direction, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
