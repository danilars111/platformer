using UnityEngine;
using UnityEngine.Pool;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private float _attackCooldown = 0.25f;
    [SerializeField]
    private Transform _firepoint;
    [SerializeField]
    private GameObject[] _fireballs;

    private float _cooldownTimer = Mathf.Infinity;

    private Animator _anim;
    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();

    }

    private void Update()
    {

        if (Input.GetMouseButton(0) && _cooldownTimer > _attackCooldown && _playerMovement.canAttack()) {
            Attack();
        }
        _cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        _anim.SetTrigger("attack");
        _cooldownTimer = 0;

        // Pool Fireballs
        _fireballs[FindFireball()].transform.position = _firepoint.position;
        _fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(_playerMovement.GetDirection());
    }

    private int FindFireball()
    {
        for (int i = 0; i < _fireballs.Length; i++)
        {
            if(!_fireballs[i].activeInHierarchy)
            {
                return i;
            }
        }

        return 0;
    }
}
