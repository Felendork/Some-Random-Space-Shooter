using System;
using System.Collections;
using System.Runtime.CompilerServices;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _horizontalInput;
    [SerializeField]
    private float _verticalInput;

    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private float _speed = 5f;

    [SerializeField]
    private GameObject _laserPrefab;
    private Vector3 _laserOff = new Vector3(0, 1.05f, 0);
    private float _fireRate = 0.3f;
    private float _canFire = -1f;
    [SerializeField]
    private int _laserAmmoCount = 15;
    [SerializeField]
    private bool _canFireAmmo = true;

    [SerializeField]
    private GameObject _tripleShotPrefab;
    private Vector3 _tripleOff = new Vector3(-1.456f, 0.85f, 0);
    [SerializeField]
    private bool _isTripleShotActive;

    private bool _isShieldActive;

    [SerializeField]
    private GameObject _shieldVisual;
    [SerializeField]
    private int _shieldLives = 3;


    [SerializeField]
    private int _score;

    [SerializeField]
    private TMP_Text _thrusterFuelText;
    [SerializeField]
    private int _thrusterFuel;
    [SerializeField]
    private int _thrusterSpeed = 10;
    private bool _isSpeedboostActive;

    [SerializeField]
    private UIManager _manager;

    [SerializeField]
    private bool _playerHurtRight;
    [SerializeField]
    private GameObject _playerHurtRightVisual;
    [SerializeField]
    private GameObject _playerHurtLeftVisual;
    [SerializeField]
    private bool _playerHurtLeft;

    private AudioSource[] _playerAudio;
    private AudioSource _laserAudio;
    private AudioSource _playerDeathAudio;
    private AudioSource _outOfAmmoAudio;
    private AudioSource _laserBeamAudio;
    private AudioSource _homingMissileAudio;

    Renderer _shieldRenderer;

    [SerializeField]
    private GameObject _laserBeamVisual;
    [SerializeField]
    private bool _isLaserBeamActive = false;

    [SerializeField]
    private bool _isHalfSpeedActive = false;
    private float _halfSpeed = 2.5f;

    [SerializeField]
    private bool _isScatterShotActive;
    [SerializeField]
    private GameObject _scatterShot;
    private Vector3 _scatterOff = new Vector3(-1.45f, 0.8f, 0);

    [SerializeField]
    private GameObject _homingMissile;
    [SerializeField]
    private bool _isHomingMissileActive;
    private AudioSource _missileFire;


    void Start()
    {
        transform.position = new Vector3(0, -2f, 0);

        _playerAudio = GetComponentsInChildren<AudioSource>();
        _laserAudio = _playerAudio[0];
        _playerDeathAudio = _playerAudio[1];
        _outOfAmmoAudio = _playerAudio[2];
        _laserBeamAudio = _playerAudio[3];
        _missileFire = _playerAudio[4];


        _thrusterFuel = 500;

        _shieldRenderer = _shieldVisual.GetComponent<Renderer>();

        _laserBeamVisual.SetActive(false);
    }

    void Update()
    {
        Movement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            Laser();
        }
    }

    public void Movement()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(_horizontalInput, _verticalInput, 0);
        transform.Translate(movement * _speed * Time.deltaTime);

        Thruster();

        if (_isHalfSpeedActive == true)
        {
            _speed = _halfSpeed;
        }

        if (transform.position.y >= 1.4f)
        {
            transform.position = new Vector3(transform.position.x, 1.4f, 0);
        }
        else if (transform.position.y <= -3.81f)
        {
            transform.position = new Vector3(transform.position.x, -3.81f, 0);
        }

        if (transform.position.x >= 9.48f)
        {
            transform.position = new Vector3(9.48f, transform.position.y, 0);
        }
        else if (transform.position.x <= -9.48f)
        {
            transform.position = new Vector3(-9.48f, transform.position.y, 0);
        }
    }

    public void Thruster()
    {
        _thrusterFuelText.text = _thrusterFuel.ToString();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _speed = _thrusterSpeed;
            _thrusterFuel -= 1;
            _thrusterFuel = Mathf.Clamp(_thrusterFuel, 0, 500);
        }

        else if (_isSpeedboostActive == false)
        {
            _speed = 5;
            _thrusterFuel += 1;
            _thrusterFuel = Mathf.Clamp(_thrusterFuel, 0, 500);
            
        }
    }

    public void Laser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive == true && _canFireAmmo == true)
        {
            Instantiate(_tripleShotPrefab, transform.position + _tripleOff, Quaternion.identity);
            _laserAudio.Play();
        }

        else if (_isScatterShotActive == true && _canFireAmmo == true)
        {
            Instantiate(_scatterShot, transform.position + _scatterOff, Quaternion.identity);
            _laserAudio.Play();
        }

        else if (_isHomingMissileActive == true)
        {
            Instantiate(_homingMissile, transform.position + _laserOff, Quaternion.identity);
            _missileFire.Play();
        }

        else if (_canFireAmmo == true)
        {
            Instantiate(_laserPrefab, transform.position + _laserOff, Quaternion.identity);
            _laserAmmoCount--;
            _laserAudio.Play();
        }
        

        if (_laserAmmoCount == 0)
        {
            _canFireAmmo = false;

            _outOfAmmoAudio.Play();
        }
    }

    public void AmmoRecharge()
    {
        _laserAmmoCount += 15;
        _laserAmmoCount = Mathf.Clamp(_laserAmmoCount, 0, 15);
        _canFireAmmo = true;
    }

    public void LaserBeam()
    {
        _laserBeamVisual.SetActive(true);
        _laserBeamAudio.Play();
        StartCoroutine(LaserBeamVisual());
    }

    IEnumerator LaserBeamVisual()
    {
        yield return new WaitForSeconds(3.0f);
        _laserBeamVisual.SetActive(false);
    }
    public void Damage()
    {

        if (_isShieldActive == true && _shieldLives == 3)
        {
            _shieldLives = 2;
            _lives++;
            _shieldRenderer.material.SetColor("_Color", Color.green);
        }

        else if (_isShieldActive == true && _shieldLives == 2)
        {
            _shieldLives = 1;
            _lives++;
            _shieldRenderer.material.SetColor("_Color", Color.red);
        }

        else if (_isShieldActive == true && _shieldLives == 1)
        {
            _isShieldActive = false;
            _shieldVisual.SetActive(false);
            return;
        }

        _lives -= 1;

        CameraShake.Shake(0.5f, 0.5f);

        if (_lives == 2 && _playerHurtLeft == false)
        {
            _playerHurtLeft = true;
            _playerHurtLeftVisual.SetActive(true);
        }

        else if (_lives == 1 && _playerHurtRight == false)
        {
            _playerHurtRight = true;
            _playerHurtRightVisual.SetActive(true);
        }

        _manager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _playerDeathAudio.Play();

            PlayerDeathAudio();

            Destroy(this.gameObject, 2f);

            _manager.GameOver();
        }
    }

    public void HealthRegen()
    {
        _lives += 1;
        _lives = Mathf.Clamp(_lives, 1, 3);

        if (_lives == 2)
        {
            _playerHurtRight = false;
            _playerHurtRightVisual.SetActive(false);
            _manager.GetComponent<UIManager>().UpdateLives(_lives);
        }

        else if (_lives == 3)
        {
            _playerHurtLeft = false;
            _playerHurtLeftVisual.SetActive(false);
            _manager.GetComponent<UIManager>().UpdateLives(_lives);
        }
    }

    IEnumerator PlayerDeathAudio()
    {
        while (_playerDeathAudio != null)
        {
            yield return null;
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;

        StartCoroutine(TripleShot());
    }

    IEnumerator TripleShot()
    {
        while (_isTripleShotActive == true)
        {
            yield return new WaitForSeconds(5f);
            _isTripleShotActive = false;
        }
    }

    public void SpeedBoostActive()
    {
        StartCoroutine(SpeedBoostCooldown());
    }

    IEnumerator SpeedBoostCooldown()
    {
        _speed = _speed * 2;
        _isSpeedboostActive = true;
        yield return new WaitForSeconds(3f);
        _isSpeedboostActive = false;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisual.SetActive(true);
        _shieldLives += 3;
        _shieldLives = Mathf.Clamp(_shieldLives, 1, 3);

    }

    public void HalfSpeed()
    {
        StartCoroutine(HalfSpeedRoutine());
    }

    IEnumerator HalfSpeedRoutine()
    {
        _speed = _halfSpeed;
        _isHalfSpeedActive = true;
        yield return new WaitForSeconds(3f);
        _isHalfSpeedActive = false;
    }

    public void ScatterShot()
    {
        _isScatterShotActive = true;
        StartCoroutine(ScatterShotCooldown());
    }

    IEnumerator ScatterShotCooldown()
    {
        while (_isScatterShotActive == true)
        {
            yield return new WaitForSeconds(3f);
            _isScatterShotActive = false;
        }
    }

    public void HomingMissilePowerup()
    {
        _isHomingMissileActive = true;
        StartCoroutine(HomingMissilePowerupRoutine());
    }

    IEnumerator HomingMissilePowerupRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isHomingMissileActive = false;
    }

    public void AddPoints(int points)
    {
        _score += points;
        _manager.UpdateScore(_score);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy Laser")
        {
            Damage();

            Destroy(other.gameObject);
        }

        if (other.tag == "Backwards Laser")
        {
            Damage();

            Destroy(other.gameObject);
        }

        if (other.tag == "Enemy Laser Beam")
        {
            Damage();
        }
    }
}

