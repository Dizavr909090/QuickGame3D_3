using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private Transform _muzzleTransform;
    [SerializeField]
    private Projectile _projectile;
    [SerializeField]
    private float _msBetweenShots = 100;
    [SerializeField, Range(1,100)]
    private float _muzzleVelocity = 35;

    private float nextShotTime;

    public void Shoot()
    {
        nextShotTime = Time.time + _msBetweenShots / 1000; //ms into s
        Projectile newProjectile = Instantiate(_projectile, _muzzleTransform.position, _muzzleTransform.rotation);
        newProjectile.SetSpeed(_muzzleVelocity);
    }
    public bool CanShoot()
    {
        return Time.time > nextShotTime;
    }
}
