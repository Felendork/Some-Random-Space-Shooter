using UnityEngine;

public class BackgroundEnemies : MonoBehaviour
{
    [SerializeField]
    private GameObject _backgroundEnemy;

    [SerializeField]
    private int _speed = -1;


    void Update()
    {
        Movement();
    }

    public void Movement()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y <= -13f)
        {
            Destroy(this.gameObject);
        }
    }
}
