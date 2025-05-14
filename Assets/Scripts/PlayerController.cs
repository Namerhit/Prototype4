using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _playerRb;
    
    private bool _hasPowerup;
    private float _powerupStrenght = 15f;

    private Coroutine _powerupCountdownCoroutine;
    
    [SerializeField] private Transform _focalPoint;
    [SerializeField] private GameObject _powerupIndicator;
    [SerializeField] private float _speed;
    [SerializeField] private TextMeshProUGUI _remainTimeText;
    
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

            if (_powerupCountdownCoroutine != null)
            {
                StopCoroutine(_powerupCountdownCoroutine);
            }
            
            _hasPowerup = true;
            _powerupIndicator.SetActive(true);
            
            Destroy(other.gameObject);
            
            _powerupCountdownCoroutine = StartCoroutine(PowerUpCountdownRoutine());
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
        var remainSeconds = 10;

        while (remainSeconds >= 1)
        {
            _remainTimeText.text = $"Remain time: {remainSeconds}";
            yield return new WaitForSeconds(1);
            remainSeconds--;
        }
        _hasPowerup = false;
        _powerupIndicator.SetActive(false);
        _remainTimeText.text = "No powerups";
    }
}
