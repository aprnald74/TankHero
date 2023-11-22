using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Slider = UnityEngine.UI.Slider;

public class Enemy : MonoBehaviour
{
    // 적 탱크
    public Transform enemy;
    private Rigidbody _rigidbody;
    public List<Transform> wheels = new List<Transform>();
    public float speed;
    
    // HP
    public GameObject hpUi;
    private Slider _slider;
    private float _enemyHp;
    public float enemyMaxHp;
    public float EnemyHp { get { return _enemyHp; } set { _enemyHp = value;  }}
    
    // 발사
    private GameObject _target;
    public Transform firePoint;
    public float fireTime;
    public float power;
    
    private void OnEnable()
    {
        _rigidbody = this.gameObject.GetComponent<Rigidbody>();
        StartCoroutine(Shout());
        _enemyHp = enemyMaxHp;

        _slider = hpUi.GetComponent<Slider>();

        _target = GameObject.Find("Player");
    }

    private void Update()
    {
        _slider.value = _enemyHp / enemyMaxHp;

        if (_enemyHp <= 0) {
            GameManager.Instance.GetScore();
            ObjectPoolManager.Instance.ReturnEnemy(this);
        }
    }

    private void FixedUpdate()
    {
        Move();

        hpUi.transform.position = enemy.position + new Vector3(0, 4f, 0);
        
        transform.LookAt(_target.transform.position);
    }

    IEnumerator Shout()
    {
        yield return new WaitForSeconds(fireTime);
        
        SoundManager.Instance.PlaySFX(SoundManager.sfxClips.Shout);
        var bullet = ObjectPoolManager.Instance.GetBullet();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = Quaternion.Euler(0, enemy.eulerAngles.y, 0);
        bullet.GetComponent<Rigidbody>().AddForce(firePoint.forward * power, ForceMode.Impulse);

        StartCoroutine(Shout());
    }

    private void Move()
    {
        // 이동
        Vector3 moveDirection = transform.forward * speed;
        _rigidbody.MovePosition(transform.position + moveDirection * Time.fixedDeltaTime);

        // 바퀴 회전
        float wheelRotationSpeed = 360.0f; // 바퀴 회전 속도 (도/초)
        float wheelRotationAngle = wheelRotationSpeed * Time.fixedDeltaTime;

        foreach (Transform wheel in wheels) {
            wheel.Rotate(Vector3.right, wheelRotationAngle);
        }
    }
}
