using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance = null;

    // 어디에 생성할지
    public Transform bulletTrans;
    public Transform enemyTrans;
    public Transform bigExplosion;
    public Transform smallExplosion;

    // 프리팹
    [SerializeField]
    private GameObject poolingBulletPrefab;
    
    [SerializeField]
    private GameObject poolingEnemyPrefab;

    [SerializeField] 
    private GameObject poolingBigExplosionPrefab;

    [SerializeField] 
    private GameObject poolingSmallExplosionPrefab;
    
    // Pooling 큐
    private Queue<Bullet> _poolingBulletQueue = new Queue<Bullet>();
    private Queue<Enemy> _poolingEnemyQueue = new Queue<Enemy>();
    private Queue<Particle> _poolingBigExplosionQueue = new Queue<Particle>();
    private Queue<Particle> _poolingSmallExplosionQueue = new Queue<Particle>();

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
        } else 
            Destroy(this);

        Initialize(10);
    }

    private void Initialize(int initCount)
    {
        for(int i = 0; i < initCount; i++) {
            _poolingBulletQueue.Enqueue(CreatNew<Bullet>(poolingBulletPrefab, bulletTrans));
            _poolingEnemyQueue.Enqueue(CreatNew<Enemy>(poolingEnemyPrefab, enemyTrans));
            _poolingBigExplosionQueue.Enqueue(CreatNew<Particle>(poolingBigExplosionPrefab, bigExplosion));
            _poolingSmallExplosionQueue.Enqueue(CreatNew<Particle>(poolingSmallExplosionPrefab, smallExplosion));
        }
    }
    
    private T CreatNew<T>(GameObject prefab, Transform parent) where T : MonoBehaviour
    {
        var newObj = Instantiate(prefab).GetComponent<T>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(parent);
        return newObj;
    }


    public Bullet GetBullet()
    {
        if(Instance._poolingBulletQueue.Count > 0)
        {
            var obj = Instance._poolingBulletQueue.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = Instance.CreatNew<Bullet>(poolingBulletPrefab, bulletTrans);
            newObj.gameObject.SetActive(true);
            return newObj;
        }
    }
    
    public void ReturnBullet(Bullet obj)
    {
        obj.gameObject.SetActive(false);
        Instance._poolingBulletQueue.Enqueue(obj);
    }

    public Enemy GetEnemy()
    {
        if (Instance._poolingEnemyQueue.Count > 0)
        {
            var obj = Instance._poolingEnemyQueue.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newobj = Instance.CreatNew<Enemy>(poolingEnemyPrefab, enemyTrans);
            newobj.gameObject.SetActive(true);
            return newobj;
        }
    }

    public void ReturnEnemy(Enemy obj)
    {
        obj.gameObject.SetActive(false);
        Instance._poolingEnemyQueue.Enqueue(obj);
    }

    public Particle GetBigExplosionParticle()
    {
        if (Instance._poolingBigExplosionQueue.Count > 0)
        {
            var obj = Instance._poolingBigExplosionQueue.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newobj = Instance.CreatNew<Particle>(poolingBigExplosionPrefab, bigExplosion);
            newobj.gameObject.SetActive(true);
            return newobj;
        }
    }

    public void ReturnBigExplosionParticle(Particle obj)
    {
        obj.gameObject.SetActive(false);
        Instance._poolingBigExplosionQueue.Enqueue(obj);
    }
    
    public Particle GetSmallExplosionParticle()
    {
        if (Instance._poolingSmallExplosionQueue.Count > 0)
        {
            var obj = Instance._poolingSmallExplosionQueue.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newobj = Instance.CreatNew<Particle>(poolingSmallExplosionPrefab, smallExplosion);
            newobj.gameObject.SetActive(true);
            return newobj;
        }
    }
    
    public void ReturnSmallExplosionParticle(Particle obj)
    {
        obj.gameObject.SetActive(false);
        Instance._poolingSmallExplosionQueue.Enqueue(obj);
    }
}