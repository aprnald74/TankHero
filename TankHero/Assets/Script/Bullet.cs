using UnityEngine;
using UnityEditor;

public class Bullet : MonoBehaviour
{

    private float _timer;
    public float damage;
    

    private void Update()
    {
        if (_timer > 10f)
            Push();
        else
            _timer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        SoundManager.Instance.PlaySFX(SoundManager.sfxClips.Boom);
        if (!other.CompareTag("AttackPoint"))
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<Enemy>().EnemyHp -= damage;
                SmallParticleSpawn();
            }
            else if (other.CompareTag("Player"))
            {
                other.GetComponent<Player>().TankHp -= damage;
                SmallParticleSpawn();
            }
            else if (other.CompareTag("Tower"))
            {
                other.GetComponent<Tower>().TowerHp -= damage;
            }
            else
            {
                BigParticleSpawn();
            }

            Push();
        }
    }

    private void BigParticleSpawn()
    {
        var bullet = ObjectPoolManager.Instance.GetBigExplosionParticle();
        bullet.transform.position = this.gameObject.transform.position;
    }
    
    private void SmallParticleSpawn()
    {
        var bullet = ObjectPoolManager.Instance.GetSmallExplosionParticle();
        bullet.transform.position = this.gameObject.transform.position;
    }

    private void Push()
    {
        _timer = 0f;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ObjectPoolManager.Instance.ReturnBullet(this);
    }
}
