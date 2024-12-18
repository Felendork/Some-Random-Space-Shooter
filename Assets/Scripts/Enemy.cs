using System.Collections;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Player _player;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    public GameObject _enemyPrefab;

    [SerializeField]
    private Vector3 _laserOffset = new Vector3(0, -1.41f, 0);

    private Animator _enemyDeath;

    private int _speed = -3;

    private AudioSource[] _enemyAudio;
    private AudioSource _laserAudio;
    private AudioSource _enemyDeathAudio;

    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();

        _enemyDeath = gameObject.GetComponent<Animator>();

        _enemyAudio = GetComponentsInChildren<AudioSource>();
        _enemyDeathAudio = _enemyAudio[0];
        _laserAudio = _enemyAudio[1];
        

        if (_enemyDeath == null)
        {
            Debug.LogError("Animation is null.");
        }

        StartCoroutine(EnemyLaser());
    }

    void Update()
    {
        Movement();

        
    }

    void Movement()
    {

        transform.position += new Vector3(0, _speed, 0) * Time.deltaTime;

        if (transform.position.y <= -6.86f)
        {
            transform.position = new Vector3(Random.Range(9.48f, -9.48f), 8.28f, 0);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _speed = 0;

            _enemyDeath.SetTrigger("OnEnemyDeath");

            other.transform.GetComponent<Player>().Damage();

            _enemyDeathAudio.Play();

            Destroy(this.gameObject, 2.5f);
        }


        if (other.tag == "Laser")
        {
            _speed = 0;

            _enemyDeath.SetTrigger("OnEnemyDeath");

            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddPoints(10);
            }

            _enemyDeathAudio.Play();

            Destroy(GetComponent<Collider2D>());

            Destroy(this.gameObject, 2.5f);
        }
    }

    IEnumerator EnemyLaser()
    {
        while (_player != null)
        {
            yield return new WaitForSeconds(Random.Range(3, 5));

            Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);

            _laserAudio.Play();
        }
    }





}
