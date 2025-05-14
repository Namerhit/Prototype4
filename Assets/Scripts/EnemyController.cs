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
}
