using System.Collections;
using System.Collections.Generic;
using System.Security;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BossFight : MonoBehaviour
{
    [SerializeField]
    private float _sideSpeed = 1.5f;
    [SerializeField]
    private Vector3 _direction = Vector3.right;

    [SerializeField]
    private float _canFire;
    [SerializeField]
    private float _fireRate = 0.5f;

    [SerializeField]
    private int _health;

    [SerializeField]
    private GameObject _normalLaser;
    [SerializeField]
    private GameObject _scatterShot;
    [SerializeField]
    private GameObject _laserBeam;

    [SerializeField]
    private Vector3 _laserOff = new Vector3(-1.99f, 3.3f, 0);
    [SerializeField]
    private Vector3 _scatterOff = new Vector3(-1.99f, -3.3f, 0);
    [SerializeField]
    private Vector3 _laserBeamOff = new Vector3(0, -9.21f, 0);

    private Player _player;
    private SpawnManager _spawnManager;
    [SerializeField]
    private UIManager _manager;

    private bool _isLaserBeamActive;

    private bool _bossHurtRight;
    private bool _bossHurtLeft;
    private bool _bossHurtCenter;

    private bool _normalLaserActive;
    private bool _scatterShotActive;
    private bool _missileActive;
    private bool _startAttacks;

    private bool _isEnemyDead = false;

    [SerializeField]
    private bool _isEnemyHurtRight;
    [SerializeField]
    private GameObject _rightDamageVisual;

    [SerializeField]
    private bool _isEnemyHurtLeft;
    [SerializeField]
    private GameObject _leftDamageVisual;

    [SerializeField]
    private bool _isEnemyHurtCenter;
    [SerializeField]
    private GameObject _centerDamageVisual;

    private int _attackID;
    [SerializeField]
    private GameObject[] _bossAttacks;

    [SerializeField]
    private ParticleSystem _enemyDeathVisual;

    private AudioSource[] _enemyAudio;
    private AudioSource _enemyDamageAudio;
    private AudioSource _enemyLaserAudio;
    private AudioSource _enemyLaserBeamAudio;
    private AudioSource _enemyDeathAudio;

    void Start()
    {
        transform.position = new Vector3(0, 9.5f, 0);

        _enemyAudio = GetComponentsInChildren<AudioSource>();
        _enemyDamageAudio = _enemyAudio[0];
        _enemyLaserAudio = _enemyAudio[1];
        _enemyLaserBeamAudio = _enemyAudio[2];
        _enemyDeathAudio = _enemyAudio[3];

        _health = 40;

        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        _spawnManager = GameObject.FindWithTag("Spawn_Manager").GetComponent<SpawnManager>();

        StartCoroutine(RandomizedAttackSequence());

    }


    void Update()
    {
        Movement();

        if (_isEnemyDead == true)
        {
            StopAllCoroutines();
        }
    }

    void Movement()
    {
        transform.Translate(Vector3.up * 1 * Time.deltaTime);

        if (transform.position.y <= 5.7f)
        {
            transform.position = new Vector3(transform.position.x, 5.7f, 0);

            transform.Translate(_direction * _sideSpeed * Time.deltaTime);

            if (transform.position.x <= -8)
            {
                _direction = Vector3.left;
            }

            else if (transform.position.x >= 8)
            {
                _direction = Vector3.right;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyDead == false)
        {
            _health--;

            _enemyDamageAudio.Play();

            other.transform.GetComponent<Player>().Damage();
        }


        if (other.tag == "Laser" && _isEnemyDead == false)
        {
            _health--;

            _enemyDamageAudio.Play();

            Destroy(other.gameObject);

        }


        if (other.tag == "Laser Beam" && _isEnemyDead == false)
        {
            _health--;

            _enemyDamageAudio.Play();
        }

        if (other.tag == "Homing_Missile" && _isEnemyDead == false)
        {
            _health--;

            _enemyDamageAudio.Play();

            Destroy(other.gameObject);
        }

        if (_health == 30 && _isEnemyHurtRight == false)
        {
            _isEnemyHurtRight = true;
            _rightDamageVisual.SetActive(true);
        }

        else if (_health == 20 && _isEnemyHurtLeft == false)
        {
            _isEnemyHurtLeft = true;
            _leftDamageVisual.SetActive(true);
        }

        else if (_health == 10 && _isEnemyHurtCenter == false)
        {
            _isEnemyHurtCenter = true;
            _centerDamageVisual.SetActive(true);
        }

        else if (_health <= 0)
        {
            _isEnemyDead = true;

            Instantiate(_enemyDeathVisual, transform.position, Quaternion.identity);

            gameObject.GetComponent<Renderer>().enabled = false;

            _enemyDeathAudio.Play();

            Destroy(GetComponent<Collider2D>());

            Destroy(this.gameObject, 2.5f);
        }

    }

    IEnumerator RandomizedAttackSequence()
    {
        EnemyLaser();
        yield return new WaitForSeconds(6);
        EnemyScatterShot();
        yield return new WaitForSeconds(3);
        EnemyLaserBeam();


    }

    private void EnemyLaser()
    {
        StartCoroutine(EnemyLaserRoutine());
    }

    IEnumerator EnemyLaserRoutine()
    {
        while (_player != null && _isEnemyDead == false)
        {

            yield return new WaitForSeconds(2);

            _enemyLaserAudio.Play();

            Instantiate(_normalLaser, transform.position + _laserOff, Quaternion.identity);


            if (transform.position.y <= -6.38f)
            {
                Destroy(this.gameObject);
            }
        }

    }

    private void EnemyScatterShot()
    {
        StartCoroutine(EnemyScatterShotRoutine());
    }

    IEnumerator EnemyScatterShotRoutine()
    {
        while (_player != null && _isEnemyDead == false)
        {
            yield return new WaitForSeconds(3);

            _enemyLaserAudio.Play();

            Instantiate(_scatterShot, transform.position + _scatterOff, Quaternion.identity);

            if (transform.position.y <= -6.38f)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void EnemyLaserBeam()
    {
        if (Time.time > _canFire)
        {
            _fireRate = 5f;

            _canFire = Time.time + _fireRate;

            EnemyLaserBeamInstantiate();
        }
    }

    public void EnemyLaserBeamInstantiate()
    {
        _enemyLaserBeamAudio.Play();

        GameObject _enemyLaserBeam = Instantiate(_laserBeam, transform.position + _laserBeamOff, Quaternion.identity);

        _enemyLaserBeam.transform.parent = transform;

        Destroy(_enemyLaserBeam, 2.5f);


    }
}
