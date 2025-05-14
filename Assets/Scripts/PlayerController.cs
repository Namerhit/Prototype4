using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _playerRb;
    
    private bool _hasPowerup;
    private float _powerupStrenght = 15f;
    
    [SerializeField] private Transform _focalPoint;
    [SerializeField] private GameObject _powerupIndicator;
    [SerializeField] private float _speed;
    
    void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        var forwardInput = Input.GetAxis("Vertical");
        _playerRb.AddForce(_focalPoint.forward * (_speed * forwardInput));

        _powerupIndicator.transform.position = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag("Powerup")) return;

        _hasPowerup = true;
        _powerupIndicator.SetActive(true);
        
        Destroy(other.gameObject);
        
        StartCoroutine(PowerUpCountdownRoutine());
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!(other.gameObject.CompareTag("Enemy") && _hasPowerup)) return;
        
        var enemyRb = other.gameObject.GetComponent<Rigidbody>();
        var punchDirection = (other.gameObject.transform.position - transform.position);
        
        enemyRb.AddForce(punchDirection * _powerupStrenght, ForceMode.Impulse);
    }

    private IEnumerator PowerUpCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        _hasPowerup = false;
        _powerupIndicator.SetActive(false);
    }
}
