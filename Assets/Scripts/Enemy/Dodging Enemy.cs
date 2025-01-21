using System.Collections;
using UnityEngine;

public class DodgingEnemy : MonoBehaviour
{
    private Player _player;
    private SpawnManager _spawnManager;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _backwardsLaser;
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private Vector3 _laserOffset = new Vector3(0, -1.41f, 0);
    [SerializeField]
    private Vector3 _backwardsLaserOffset = new Vector3(0, 1.48f, 0);

    [SerializeField]
    private ParticleSystem _enemyDeathVisual;

    private int _speed = -3;

    private AudioSource[] _enemyAudio;
    private AudioSource _laserAudio;
    private AudioSource _enemyDeathAudio;

    [SerializeField]
    private bool _isEnemyDead = false;

    private bool _isShieldActive;

    [SerializeField]
    private GameObject _shieldVisual;
    [SerializeField]
    private int _shield;

    private bool _dodge;
    private int _direction;


    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        _spawnManager = GameObject.FindWithTag("Spawn_Manager").GetComponent<SpawnManager>();

        _enemyAudio = GetComponentsInChildren<AudioSource>();
        _enemyDeathAudio = _enemyAudio[0];
        _laserAudio = _enemyAudio[1];

        _shield = Random.Range(1, 6);

        ShieldActive();

        StartCoroutine(EnemyLaser());
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (_dodge == true)
        {
            Vector3 path = (_direction == 0) ? Vector3.left : Vector3.right;
            transform.Translate(path * _speed * 4 * Time.deltaTime);

            if (transform.position.y <= -6.86f)
            {
                transform.position = new Vector3(Random.Range(9.48f, -9.48f), 8.28f, 0);
            }
        }
        
        float _distance = Vector3.Distance(transform.position, _player.transform.position);
        
        if (_distance < 3)
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, 3 * Time.deltaTime);
        }

        if (transform.position.y <= -6.86f)
        {
            transform.position = new Vector3(Random.Range(9.48f, -9.48f), 8.28f, 0);
        }
    }

    public void Dodge()
    {
        StartCoroutine(DodgeRoutine());
    }

    IEnumerator DodgeRoutine()
    {
        _direction = Random.Range(0, 2);

        _dodge = true;

        yield return new WaitForSeconds(1f);

        _dodge = false;
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

            _speed = 0;

            gameObject.GetComponent<Renderer>().enabled = false;

            Instantiate(_enemyDeathVisual, transform.position, Quaternion.identity);

            other.transform.GetComponent<Player>().Damage();

            _enemyDeathAudio.Play();

            _isEnemyDead = true;

            Destroy(this.gameObject, 2.5f);
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

            _speed = 0;

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


            Destroy(this.gameObject, 2.5f);
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

            _speed = 0;

            gameObject.GetComponent<Renderer>().enabled = false;

            Instantiate(_enemyDeathVisual, transform.position, Quaternion.identity);

            if (_player != null)
            {
                _player.AddPoints(10);
            }

            _enemyDeathAudio.Play();

            Destroy(GetComponent<Collider2D>());

            _isEnemyDead = true;


            Destroy(this.gameObject, 2.5f);
        }

        if (other.tag == "Homing_Missile" && _isEnemyDead == false)
        {
            if (_isShieldActive)
            {
                _shieldVisual.SetActive(false);
                Destroy(other.gameObject);
                Destroy(this.gameObject, 1.5f);
            }

            _spawnManager.EnemyDestroyed(1);

            _speed = 0;

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

            Destroy(this.gameObject, 1.5f);
        }

    }

    IEnumerator EnemyLaser()
    {
        while (_player != null && _isEnemyDead == false)
        {
            Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);

            _laserAudio.Play();

            yield return new WaitForSeconds(Random.Range(3, 5));

            if (transform.position.y < _player.transform.position.y)
            {
                Instantiate(_backwardsLaser, transform.position + _backwardsLaserOffset, Quaternion.identity);

                _laserAudio.Play();

                yield return new WaitForSeconds(Random.Range(1, 3));
            }
        }
    }
}
