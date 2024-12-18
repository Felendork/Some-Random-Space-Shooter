using System.Collections;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private int powerupID;

    [SerializeField]
    private AudioClip _powerUpSound;
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        transform.position += Vector3.up * -3 * Time.deltaTime;

        if (transform.position.y <= -6.01f)
        {
            Destroy(this.gameObject);
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
                }


            }

            Destroy(this.gameObject);
        }
    }
}
