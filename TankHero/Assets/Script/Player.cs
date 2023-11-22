using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;
using Image = UnityEngine.UI.Image;

public class Player : MonoBehaviour
{
    // 탱크
    private Rigidbody _tankRigid;
    public Transform tank;
    public Slider tankHpSlider;
    public float rotationSpeed;
    public float moveSpeed;
    private float _tankHp;
    
    public float TankHp { get { return _tankHp; } set { _tankHp = value;  }}
    public float tankMaxHp;
    private bool _isTankLive;
    
    // 바퀴
    public List<Transform> wheels;
    private int _isFront;

    // 전등
    public GameObject rLight;
    public GameObject lLight;
    public bool isLight;
    
    // 터랫
    public Transform turret;
    public float rotationTurretSpeed;
    private bool _isTurretLive;
    
    // 총신
    public Transform gunBarrel;
    public Transform firePoint;
    public float rotationGunBarrelSpeed;
    public float minRotationAngle;
    public float maxRotationAngle;
    
    // 마우스
    private float _mouseSpeed;
    private float _mouseY;
    private float _mouseX;

    // 총알
    public Transform attackPoint;
    public Image fireSlider;
    public Slider powerGauge;
    public float fireTime;
    private float _fireTickTime;
    public float maxPower;
    private float _power;
    private float _powerGaugeValue;
    
    
    // 수리
    public Image repairSlider;
    public float repairTime;
    private float _repairTickTime;
    private int _count;
    public int heel;

    private void Awake()
    {
        _tankRigid = gameObject.GetComponent<Rigidbody>();
        tank = gameObject.GetComponent<Transform>();
        isLight = false;
        _isTankLive = true;
        _isTurretLive = true;

        _tankHp = tankMaxHp;
        _power = maxPower;
        _powerGaugeValue = 0f;
        
        SoundManager.Instance.MoveStart();
    }
    
    void FixedUpdate()
    {
        if (_isTankLive)
            Move();
        if (_isTurretLive) 
            MoveTurret();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            isLight = !isLight;
            rLight.SetActive(isLight);
            lLight.SetActive(isLight);
        }

        if (Input.GetKeyDown(KeyCode.F) && repairTime < _repairTickTime) {
            StartCoroutine(Repair());
            _repairTickTime = 0f;
        }
        
        if (_isTurretLive) 
            Shout();

        if (!_isTankLive && !_isTurretLive)
        {
            SoundManager.Instance.MoveStop();
            GameManager.Instance.StartCoroutine(GameManager.Instance.GameEnd());
        }

        if (_tankHp <= 0) {
            _isTankLive = false;
        } else {
            _isTankLive = true;
        }   

        tankHpSlider.value = _tankHp / tankMaxHp;
        
        _fireTickTime += Time.deltaTime;

        _repairTickTime += Time.deltaTime;

        powerGauge.value = _powerGaugeValue;
        
        repairSlider.fillAmount = 1.0f - (Mathf.Lerp(0, 100, _repairTickTime / repairTime) / 100);
        
        fireSlider.fillAmount = 1.0f - (Mathf.Lerp(0, 100, _fireTickTime / fireTime) / 100);
        
        PredictTrajectory(firePoint.position, firePoint.forward *_power);
    }
    
    private void Shout()
    {
        if (_fireTickTime > fireTime)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (_powerGaugeValue <= 1f)
                {
                    _powerGaugeValue += Time.deltaTime;
                    _power = _power + (_powerGaugeValue / 5);
                }
            }
            
            if (Input.GetKeyUp(KeyCode.Space) && _fireTickTime > fireTime)
            {
                SoundManager.Instance.PlaySFX(SoundManager.sfxClips.Shout);
                var bullet = ObjectPoolManager.Instance.GetBullet();
                bullet.transform.position = firePoint.position;
                bullet.transform.rotation = Quaternion.Euler(gunBarrel.eulerAngles.x, (turret.eulerAngles.y), 0);
                bullet.GetComponent<Rigidbody>().AddForce(firePoint.forward * _power, ForceMode.Impulse);
                _fireTickTime = 0f;
                _powerGaugeValue = 0f;
                _power = maxPower;
            }
        }
    }
    
    public void TurretDie()
    {
        _isTurretLive = false;
    }

    public void TurretLive()
    {
        _isTurretLive = true;
    }
    
    

    private void MoveTurret()
    {
        // 마우스의 X 좌표를 가져와서 회전 각도로 변환
        _mouseX = Input.GetAxis("Mouse X") * rotationTurretSpeed;
        _mouseY = Input.GetAxis("Mouse ScrollWheel") * rotationGunBarrelSpeed;
        
        // 현재 오브젝트의 Y 회전을 마우스 이동에 따라 조절
        turret.Rotate(Vector3.up * _mouseX);
        
        gunBarrel.Rotate(Vector3.right * _mouseY);

        Vector3 angle = gunBarrel.eulerAngles;
        if (angle.x > 180)
            angle.x -= 360;
        angle.x = Mathf.Clamp(angle.x, minRotationAngle, maxRotationAngle);
        gunBarrel.rotation = Quaternion.Euler(angle.x, angle.y, 0);

    }
    
    private void Move()
    {
        // 앞 뒤 이동
        float moveInput = 0f;
        if (Input.GetKey(KeyCode.W))
        {
            moveInput = 1f;
            _isFront = 1;
        } else if (Input.GetKey(KeyCode.S))
        {
            moveInput = -1f;
            _isFront = -1;
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            _isFront = 1;
        }

        // 좌 우 이동
        float rotationInput = 0f;
        if (Input.GetKey(KeyCode.A)) {
            rotationInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D)) {
            rotationInput = 1f;
        }

        // 이동
        Vector3 moveDirection = transform.forward * moveInput * moveSpeed;
        _tankRigid.MovePosition(transform.position + moveDirection * Time.fixedDeltaTime);

        // 회전
        Vector3 rotation = Vector3.up * rotationInput * rotationSpeed * Time.fixedDeltaTime;
        _tankRigid.MoveRotation(_tankRigid.rotation * Quaternion.Euler(rotation * _isFront));

        // 바퀴 회전
        float wheelRotationSpeed = 360.0f; // 바퀴 회전 속도 (도/초)
        float wheelRotationAngle = wheelRotationSpeed * moveInput * Time.fixedDeltaTime;

        foreach (Transform wheel in wheels) {
            wheel.Rotate(Vector3.right, wheelRotationAngle);
        }
    }

    IEnumerator Repair()
    {
        for (_count = 0; _count < 5; _count++) {
            _tankHp += heel;
            if (_tankHp > tankMaxHp) 
                _tankHp = tankMaxHp;
            
            
            turret.GetComponent<Tower>().TowerHp += heel;
            if (turret.GetComponent<Tower>().TowerHp > turret.GetComponent<Tower>().towerMaxHp) 
                turret.GetComponent<Tower>().TowerHp = turret.GetComponent<Tower>().towerMaxHp;
            
            yield return new WaitForSeconds(1f);
        }

    }
    
    private void PredictTrajectory(Vector3 startPos, Vector3 vel)
    {  
        int step = 250;
        float deltaTime = Time.fixedDeltaTime;
        Vector3 gravity = Physics.gravity;
  
        Vector3 position = startPos;
        Vector3 velocity = vel;

        for (int i = 0; i < step; i++)
        {
            if (position.y <= 1) {
                attackPoint.position = position - new Vector3(0, 1f, 0);
            } else {
                position += velocity * deltaTime + 0.5f * gravity * deltaTime * deltaTime;
                velocity += gravity * deltaTime;
            }
        }
    }
}