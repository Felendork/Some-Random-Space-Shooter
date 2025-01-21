using UnityEngine;

public class Radar : MonoBehaviour
{
    private DodgingEnemy _enemy;
    void Start()
    {
        _enemy = transform.parent.GetComponent<DodgingEnemy>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            _enemy.Dodge();
        }
    }
}
