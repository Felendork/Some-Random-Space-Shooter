using UnityEngine;

public class PlayerLaser : MonoBehaviour
{

    void Update()
    {
        Movement();

        EnemyLaserMovement();
    }

    public void Movement()
    {
        if (gameObject.tag == "Laser")
        {
            transform.position += new Vector3(0, 10, 0) * Time.deltaTime;
            if (transform.position.y >= 6.97f)
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
            transform.position += new Vector3(0, -10, 0) * Time.deltaTime;
            if (transform.position.y <= -6.5f)
            {
                Destroy(this.gameObject);

            }
        }
    }
}
