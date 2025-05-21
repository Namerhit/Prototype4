using System.Collections;
using TMPro;
using UnityEngine;

namespace Player
{
    public enum PowerupType
    {
        None,
        Strength,
        Rocket,
        Smash
    }

    public class PlayerController : MonoBehaviour
    {
        private Rigidbody _playerRb;
        private Coroutine _powerupCountdownCoroutine;
        private Coroutine _launchProjectile;
        private PowerupType _currentPowerup = PowerupType.None;

        private bool _isSmashing = false;
        
        [SerializeField] private GameObject _powerupIndicator;
        [SerializeField] private GameObject _projectile;
        
        [SerializeField] private Transform _focalPoint;
        
        [SerializeField] private float _speed;
        [SerializeField] private float _smashSpeed;

        [SerializeField] private AnimationCurve _smashCurve;
        [SerializeField] private AnimationCurve _smashStrengthCurve;
        
        [SerializeField] private TextMeshProUGUI _remainTimeText; 

        void Start()
        {
            _playerRb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            _powerupIndicator.transform.position = transform.position;
            
            if (_isSmashing) return;
        
            var forwardInput = Input.GetAxis("Vertical");
            _playerRb.AddForce(_focalPoint.forward * (_speed * forwardInput));
            
            if(_currentPowerup != PowerupType.Smash) return;
            
            if (!(Input.GetKeyDown(KeyCode.Space) && !_isSmashing)) return;
            
            StartCoroutine(SmashAttackRoutine());
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Powerup")) return;

            PowerupController powerupItem = other.GetComponent<PowerupController>();

            PowerupType collectedPowerup = powerupItem.TypeOfPowerup;
            
            if (_powerupCountdownCoroutine != null)
            {
                StopCoroutine(_powerupCountdownCoroutine);
                DeactivateCurrentPowerup();
            }

            ActivatePowerupEffect(collectedPowerup);

            _currentPowerup = collectedPowerup;
            _powerupIndicator.SetActive(true);
            
            Destroy(other.gameObject);
            
            _powerupCountdownCoroutine = StartCoroutine(PowerupCountdownRoutine());
        }

        private void DeactivateCurrentPowerup()
        {
            switch (_currentPowerup)
            {
                case PowerupType.Strength:
                    _playerRb.mass = 1;
                    _speed = 3;
                    break;
                
                case PowerupType.Rocket:
                    if (_launchProjectile != null)
                    {
                        StopCoroutine(_launchProjectile);    
                    }
                    break;
            }
            
            _currentPowerup = PowerupType.None;
            _powerupIndicator.SetActive(false);
            _remainTimeText.text = "No powerups";
        }

        private void ActivatePowerupEffect(PowerupType collectedPowerup)
        {
            switch (collectedPowerup)
            {
                case PowerupType.Strength:
                    _playerRb.mass = 45;
                    _speed *= 100;
                    break;
                
                case PowerupType.Rocket:
                    if (_launchProjectile != null)
                    {
                        StopCoroutine(_launchProjectile);
                    }
                    _launchProjectile = StartCoroutine(LaunchProjectileRoutine());
                    break;
            }
        }

        private IEnumerator LaunchProjectileRoutine()
        {
            var remainTime = 5;

            while (remainTime>0)
            {
                var waveEnemies = FindObjectsOfType<EnemyController>();

                foreach (var enemy in waveEnemies)
                {
                    if (enemy ==null) continue;
                    
                    var shootDirection = enemy.transform.position - transform.position;
                    var spawnRotation = Quaternion.LookRotation(shootDirection);
                    
                    var newProjectile= Instantiate(_projectile, transform.position, spawnRotation);
                    var projectileRb = newProjectile.GetComponent<Rigidbody>();
                    
                    projectileRb.AddForce(shootDirection.normalized * 30, ForceMode.Impulse);
                }
                
                yield return new WaitForSeconds(2);
                remainTime--;
            }
        }

        private IEnumerator SmashAttackRoutine()
        {
            _isSmashing = true;
            
            var startPosition = transform.position;
            var endPosition = transform.position;
            endPosition.y = 2.5f;

            var progress = 0f;

            while (progress < 1f)
            {
                progress += Time.deltaTime * _smashSpeed;
                transform.position = Vector3.LerpUnclamped(startPosition, endPosition, _smashCurve.Evaluate(progress));
                yield return null;
            }

            transform.position = endPosition;

            yield return new WaitForSeconds(0.2f);
            
            progress = 0f;

            while (progress < 1f)
            {
                progress += Time.deltaTime * _smashSpeed;
                transform.position = Vector3.LerpUnclamped(transform.position, startPosition, _smashCurve.Evaluate(progress));
                yield return null;
            }

            transform.position = startPosition;
            
            _playerRb.velocity = Vector3.zero;
            _playerRb.angularVelocity = Vector3.zero;

            var waveEnemies = FindObjectsOfType<EnemyController>();

            foreach (var enemy in waveEnemies)
            {
                
                var smashDirection = enemy.transform.position - transform.position;
                
                smashDirection.y = 0;

                var smashDistance = smashDirection.magnitude;

                var smashStrength = _smashStrengthCurve.Evaluate(smashDistance);

                var enemyRb = enemy.GetComponent<Rigidbody>();
                
                Debug.Log($"Distance: {smashDistance}, Strength: {smashStrength}");
                
                enemyRb.AddForce(smashDirection.normalized * (smashStrength * 1.5f), ForceMode.Impulse);

            }

            _isSmashing = false;
        }

        private IEnumerator PowerupCountdownRoutine()
        {
            var remainSeconds = 10;

            while (remainSeconds >= 1)
            {
                _remainTimeText.text = $"Remain time: {remainSeconds:D2}";
                
                yield return new WaitForSeconds(1);
                
                remainSeconds--;
            }

            DeactivateCurrentPowerup();
        }
    }
}