using System.Collections;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private int powerupID;

    private Player _player;

    [SerializeField]
    private AudioClip _powerUpSound;

    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (Input.GetKey(KeyCode.C) && Vector3.Distance(transform.position, _player.transform.position) < 6)
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, 5 * Time.deltaTime);
        }

        else
        {
            transform.position += Vector3.up * -3 * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_powerUpSound, transform.position);

            if (player != null)
            {
                switch (powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    case 3:
                        player.HalfSpeed();
                        break;
                    case 4:
                        player.ScatterShot();
                        break;
                    case 5:
                        player.HomingMissilePowerup();
                        break;
                }


            }

            Destroy(this.gameObject);
        }

        else
        {
            other.gameObject.TryGetComponent<Laser>(out Laser _laser);

            if (_laser != null)
            {
                Destroy(this.gameObject);
                Destroy(other.gameObject);
            }
        }
    }
}
