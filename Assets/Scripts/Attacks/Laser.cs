using UnityEngine;

public class Laser : MonoBehaviour
{
    private int _playerLaserSpeed = 10;

    private int _enemyLaserSpeed = -10;

    private int _backwardsLaserSpeed = 10;


    void Update()
    {
        Movement();

        EnemyLaserMovement();

        BackwardsEnemyLaserMovement();
    }

    public void Movement()
    {
        if (gameObject.tag == "Laser")
        {
            transform.Translate(Vector3.up * _playerLaserSpeed * Time.deltaTime);
            if (transform.position.y >= 15f)
            {
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }

                Destroy(this.gameObject);
            }
        }
    }

    public void EnemyLaserMovement()
    {
        if (gameObject.tag == "Enemy Laser")
        {
            transform.Translate(Vector3.up * _enemyLaserSpeed * Time.deltaTime);
            if (transform.position.y <= -6.5f)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void BackwardsEnemyLaserMovement()
    {
        if (gameObject.tag == "Backwards Laser")
        {
            transform.Translate(Vector3.up * _backwardsLaserSpeed * Time.deltaTime);
            if (transform.position.y >= 15f)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
