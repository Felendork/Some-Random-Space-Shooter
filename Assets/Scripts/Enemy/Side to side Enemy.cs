using System.Collections;
using UnityEngine;

public class SidetosideEnemy : MonoBehaviour
{
    private Player _player;
    private SpawnManager _spawnManager;

    [SerializeField]
    private GameObject _laserBeam;

    [SerializeField]
    private GameObject _stationEnemy;
    private int _sideSpeed = 3;

    [SerializeField]
    private Vector3 _laserOffset = new Vector3(0, -8.38f, 0);

    [SerializeField]
    private ParticleSystem _enemyDeathVisual;

    private Vector3 _direction = Vector3.right;

    private bool _isEnemyDead = false;

    private AudioSource[] _enemyAudio;
    private AudioSource _laserBeamAudio;
    private AudioSource _enemyDeathAudio;

    private bool _isShieldActive;

    [SerializeField]
    private GameObject _shieldVisual;
    [SerializeField]
    private int _shield;

    private float _fireRate;
    private float _canFire = 2f;

    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        _spawnManager = GameObject.FindWithTag("Spawn_Manager").GetComponent<SpawnManager>();

        _enemyAudio = GetComponentsInChildren<AudioSource>();
        _enemyDeathAudio = _enemyAudio[0];
        _laserBeamAudio = _enemyAudio[1];

        _shield = Random.Range(1, 6);

        ShieldActive();
    }


    void Update()
    {
        Movement();

        EnemyLaser();
    }

    public void Movement()
    {
        transform.Translate(Vector3.up * 5 * Time.deltaTime);
        transform.Translate(_direction * _sideSpeed * Time.deltaTime);

        if (transform.position.y <= 6.4f)
        {
            transform.position = new Vector3(transform.position.x, 6.4f, 0);
        }

        if (transform.position.x <= -8)
        {
            _direction = Vector3.left;
        }

        else if (transform.position.x >= 8)
        {
            _direction = Vector3.right;
        }
    }

    public void ShieldActive()
    {
        if (_shield == 5)
        {
            _isShieldActive = true;
            _shieldVisual.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyDead == false)
        {
            if (_isShieldActive)
            {
                _isShieldActive = false;
                _shieldVisual.SetActive(false);
                other.transform.GetComponent<Player>().Damage();
                return;
            }
            _spawnManager.EnemyDestroyed(1);

            gameObject.GetComponent<Renderer>().enabled = false;

            Instantiate(_enemyDeathVisual, transform.position, Quaternion.identity);

            other.transform.GetComponent<Player>().Damage();

            _enemyDeathAudio.Play();

            _isEnemyDead = true;

            Destroy(this.gameObject);
        }


        if (other.tag == "Laser" && _isEnemyDead == false)
        {
            if (_isShieldActive)
            {
                _isShieldActive = false;
                _shieldVisual.SetActive(false);
                Destroy(other.gameObject);
                return;
            }

            _spawnManager.EnemyDestroyed(1);


            gameObject.GetComponent<Renderer>().enabled = false;

            Instantiate(_enemyDeathVisual, transform.position, Quaternion.identity);

            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddPoints(10);
            }

            _enemyDeathAudio.Play();

            Destroy(GetComponent<Collider2D>());

            _isEnemyDead = true;


            Destroy(this.gameObject);
        }


        if (other.tag == "Laser Beam" && _isEnemyDead == false)
        {
            if (_isShieldActive)
            {
                _isShieldActive = false;
                _shieldVisual.SetActive(false);
                return;
            }
            _spawnManager.EnemyDestroyed(1);


            gameObject.GetComponent<Renderer>().enabled = false;

            Instantiate(_enemyDeathVisual, transform.position, Quaternion.identity);

            if (_player != null)
            {
                _player.AddPoints(10);
            }

            _enemyDeathAudio.Play();

            Destroy(GetComponent<Collider2D>());

            _isEnemyDead = true;


            Destroy(this.gameObject);
        }

        if (other.tag == "Homing_Missile" && _isEnemyDead == false)
        {
            if (_isShieldActive)
            {
                _shieldVisual.SetActive(false);
                Destroy(other.gameObject);
                Destroy(this.gameObject);
            }

            _spawnManager.EnemyDestroyed(1);


            gameObject.GetComponent<Renderer>().enabled = false;

            Instantiate(_enemyDeathVisual, transform.position, Quaternion.identity);

            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddPoints(10);
            }

            _enemyDeathAudio.Play();

            Destroy(GetComponent<Collider2D>());

            _isEnemyDead = true;

            Destroy(this.gameObject);
        }

    }

    public void EnemyLaser()
    {
        if (Time.time > _canFire)
        {
            _fireRate = 5f;

            _canFire = Time.time + _fireRate;

            EnemyLaserInstantiate();
        }
    }

    public void EnemyLaserInstantiate()
    {
        _laserBeamAudio.Play();

        GameObject _enemyLaserBeam = Instantiate(_laserBeam, transform.position + _laserOffset, Quaternion.identity);

        _enemyLaserBeam.transform.parent = transform;

        Destroy(_enemyLaserBeam, 2.5f);


    }
}
