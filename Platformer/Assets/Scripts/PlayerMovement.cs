using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] 
    private float _speed = 5;
    
    private Rigidbody2D _body;
    private Animator _anim;
    private int _direction = 1;

    private bool _grounded;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
       
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        // -1 on x-axis scale flips character

        if ((horizontalInput > 0 && transform.localScale.x < 0) || 
            (horizontalInput < 0 && transform.localScale.x > 0))
        {
            _direction *= -1;
            transform.localScale = new Vector3(_direction, 1, 1);
        }

        _body.linearVelocity = new Vector2(horizontalInput * _speed, _body.linearVelocityY);


        if (Input.GetKey(KeyCode.Space) && _grounded)
        {
            Jump();
        }

        _anim.SetBool("run", horizontalInput != 0);
        _anim.SetBool("grounded", _grounded);
    }

    private void Jump()
    {
        _grounded = false;
        _anim.SetTrigger("jump");
        _body.linearVelocity = new Vector2(_body.linearVelocityX, _speed);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            _grounded = true;
        }
    }
}
