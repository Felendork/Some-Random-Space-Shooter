using Unity.VisualScripting;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    [SerializeField]
    private int _collectableID;

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
            Player player = other.GetComponent<Player>();



            if (player != null)
            {
                switch (_collectableID)
                {
                    case 0:
                        player.AmmoRecharge();
                        break;
                    case 1:
                        player.HealthRegen();
                        break;
                    case 2:
                        player.LaserBeam();
                        break;

                }
            }

            Destroy(this.gameObject);
        }
    }
}
