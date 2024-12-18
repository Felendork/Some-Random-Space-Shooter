using UnityEngine;
using System;

public class Astroid : MonoBehaviour
{
    [SerializeField]
    private GameObject _astroid;
    private SpawnManager _spawnManager;

    private Animator _explosion;

    private AudioSource _explosionSource;
    void Start()
    {
        transform.position = new Vector3(0, 5, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        _explosionSource = gameObject.GetComponent<AudioSource>();
    }


    void Update()
    {
        transform.Rotate(0, 0, 0.3f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            _explosion = gameObject.GetComponent<Animator>();
            _explosion.SetTrigger("Astroid Explosion");

            _spawnManager.StartSpawning();

            _explosionSource.Play();

            Destroy(GetComponent<Collider2D>());

            Destroy(this.gameObject, 2.40f);
        }
    }
}
