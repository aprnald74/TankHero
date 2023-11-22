using UnityEngine;
using UnityEditor;

public class Particle : MonoBehaviour
{
    public ParticleSystem particle;
    private float _timer;

    private void OnEnable()
    {
        particle.Play();
    }

    private void Update()
    {
        if (_timer > 3f)
            Push();
        else
            _timer += Time.deltaTime;
    }
    
    private void Push()
    {
        _timer = 0f;
        if (this.gameObject.name == "BigExplosionEffect(Clone)")
            ObjectPoolManager.Instance.ReturnBigExplosionParticle(this);
        else if (this.gameObject.name == "SmallExplosionEffect(Clone)")
            ObjectPoolManager.Instance.ReturnSmallExplosionParticle(this);
    }
}
