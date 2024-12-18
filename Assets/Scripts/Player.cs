using System;
using System.Collections;
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
    private int _speed = 5;

    [SerializeField]
    private GameObject _laserPrefab;
    private Vector3 _laserOff = new Vector3(0, 1.05f, 0);
    private float _fireRate = 0.3f;
    private float _canFire = -1f;

    [SerializeField]
    private GameObject _tripleShotPrefab;
    private Vector3 _tripleOff = new Vector3(-1.744f, 1.05f, 0);
    private bool _isTripleShotActive;

    private bool _isShieldActive;

    [SerializeField]
    private GameObject _shieldVisual;

    [SerializeField]
    private int _score;

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



    void Start()
    {
        transform.position = new Vector3(0, -2f, 0);

        _playerAudio = GetComponentsInChildren<AudioSource>();
        _laserAudio = _playerAudio[0];
        _playerDeathAudio = _playerAudio[1];
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

    public void Laser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position + _tripleOff, Quaternion.identity);
        }

        else
        {
            Instantiate(_laserPrefab, transform.position + _laserOff, Quaternion.identity);
        }

        _laserAudio.Play();
    }

    public void Damage()
    {
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisual.SetActive(false);
            return;
        }

        _lives -= 1;

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
        yield return new WaitForSeconds(3f);
        _speed = 5;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisual.SetActive(true);

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

    }
}

