using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    private int _missileSpeed = 10;
    [SerializeField]
    private float _missileRotation = 100f;

    private float _distanceToClosestEnemy = Mathf.Infinity;
    private float _distanceToEnemy;

    private GameObject[] _enemies;
    private GameObject _closestEnemy;
    private Vector3 _closestEnemyPos;
    private Quaternion _rotateTarget;


    private void Start()
    {
        HomingMissile();
    }
    void Update()
    {
        HomingMissileMovement();
    }

    private void HomingMissile()
    {
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (_enemies != null)
        {
            foreach (GameObject _currentEnemy in _enemies)
            {
                _distanceToClosestEnemy = (_currentEnemy.transform.position - this.gameObject.transform.position).sqrMagnitude;

                if (_distanceToEnemy < _distanceToClosestEnemy)
                {
                    _distanceToClosestEnemy = _distanceToEnemy;
                    _closestEnemy = _currentEnemy;
                }
            }
        }
    }

    private void HomingMissileMovement()
    {
        if (_enemies == null || _closestEnemy == null)
        {
            transform.Translate(Vector3.up * _missileSpeed * Time.deltaTime);
        }

        else if (_enemies != null)
        {
            _closestEnemyPos = _closestEnemy.transform.position;
            transform.Translate((_closestEnemyPos - transform.position).normalized * _missileSpeed * Time.deltaTime);

            _rotateTarget = Quaternion.LookRotation(Vector3.forward, (_closestEnemyPos - transform.position).normalized);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _rotateTarget, _missileRotation * Time.deltaTime);
        }

        if (transform.position.y >= 9.5f || transform.position.y <= -6.5f)
        {
            Destroy(this.gameObject);
        }
    }
}
