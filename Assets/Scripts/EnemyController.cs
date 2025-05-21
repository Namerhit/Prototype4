using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameObject _player;
    private Rigidbody _enemyRb;
    [SerializeField] private float _speed;
    
    void Start()
    {
        _player = GameObject.Find("Player");
        _enemyRb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        var moveDirection = (_player.transform.position - transform.position).normalized;
        _enemyRb.AddForce(moveDirection * _speed);
        
        if(!(transform.position.y < -10)) return;
        
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!(other.gameObject.CompareTag("Projectile"))) return;

        var punchDirection = transform.position - _player.transform.position;
        punchDirection.y = 0;
        
        Debug.DrawRay(transform.position, punchDirection.normalized * 5, Color.green, 2);
        
        _enemyRb.AddForce(punchDirection.normalized * 25, ForceMode.Impulse);
        
        Destroy(other.gameObject);
    }
}
