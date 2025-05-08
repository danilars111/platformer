using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] 
    private float _speed = 5;
    [SerializeField] 
    private float _jumpPower = 10;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private LayerMask wallLayer;
    [SerializeField]
    private float _gravityScale = 1f;

    private Rigidbody2D _body;
    private Animator _anim;
    private int _direction = 1;
    private BoxCollider2D _boxCollider;
    private float _wallJumpCooldown;
    private float _horizontalInput;


    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _body.gravityScale = _gravityScale; 
    }

    private void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        // -1 on x-axis scale flips character

        if ((_horizontalInput > 0 && transform.localScale.x < 0) || 
            (_horizontalInput < 0 && transform.localScale.x > 0))
        {
            flip();
        }

        
        _anim.SetBool("run", _horizontalInput != 0);
        _anim.SetBool("grounded", isGrounded());

        if(_wallJumpCooldown > 0.2f)
        {

            _body.linearVelocity = new Vector2(_horizontalInput * _speed, _body.linearVelocityY);

            if (onWall() && !isGrounded())
            {
                _body.gravityScale = 0;
                _body.linearVelocity = Vector2.zero;
            } else
            {
                _body.gravityScale = _gravityScale;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }
        } else
        {
            _wallJumpCooldown += Time.deltaTime;
        }
    }

    private void Jump()
    {
        if (isGrounded())
        {
            _anim.SetTrigger("jump");
            _body.linearVelocity = new Vector2(_body.linearVelocityX, _jumpPower);
        }
        else if (onWall() && !isGrounded())
        {

            if (_horizontalInput == 0)
            {
                _body.linearVelocity = new Vector2(-Mathf.Sign(_direction) * _speed, 0);
                flip();
            } else
            {
                _body.linearVelocity = new Vector2(-Mathf.Sign(_direction) * _speed / 2, _jumpPower);
            }
            _wallJumpCooldown = 0;
        }
     }

    private void flip()
    {
        _direction *= -1;
        transform.localScale = new Vector3(_direction, 1, 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0, new Vector2(_direction, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
}
