using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected Bullet bulletPrefab;
    [SerializeField, Min(1)] protected int shootsBeforeReload;
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected float damage = 1;
    [Tooltip("0 - ��� ��������"), Min(0)] public float spread;
    [SerializeField] protected LayerMask layersToShoot;

    public float reloadTime;
    public float betweenShootDelay;

    protected int bulletsInMagazine;

    private bool _canShoot = true;

    protected Transform[] shootPointsPositions;

    [HideInInspector] public UnityEvent FiredEvent = new();
    [HideInInspector] public UnityEvent FireDelayEndedEvent = new();
    [HideInInspector] public UnityEvent ReloadStartedEvent = new();
    [HideInInspector] public UnityEvent ReloadEndedEvent = new();

    public abstract void Shoot();

    protected virtual void Awake()
    {
        bulletsInMagazine = shootsBeforeReload;

        foreach (var shootPoint in GetComponentsInChildren<ShootPoint>())
        {
            shootPoint.EnemyDetected.AddListener(CheckInputAndReloadTimeAndShoot);
        }

        shootPointsPositions = GetComponentsInChildren<ShootPoint>().Select(shootPoint => shootPoint.transform).ToArray();
    }

    protected virtual void Update()
    {

    }

    private void CheckInputAndReloadTimeAndShoot()
    {
        if (_canShoot && Input.GetMouseButton(0))
        {
            bulletsInMagazine--;
            Shoot();
            FiredEvent.Invoke();
            Invoke(nameof(OnFiredDelayEnded), betweenShootDelay);
            _canShoot = false;

            if (bulletsInMagazine <= 0)
            {
                ReloadStartedEvent.Invoke();
                Invoke(nameof(Reload), reloadTime);
            }

            else
            {
                Invoke(nameof(RefreshAfterShoot), betweenShootDelay);
            }
        }
    }

    private void Reload()
    {
        bulletsInMagazine = shootsBeforeReload;
        _canShoot = true;
        ReloadEndedEvent.Invoke();
    }

    private void OnFiredDelayEnded()
    {
        FireDelayEndedEvent.Invoke();
    }

    private void RefreshAfterShoot()
    {
        _canShoot = true;
    }

    protected Vector3 GetWeaponLookDirection()
    {
        return transform.forward;
    }

    protected void ShootBulletWithSpread(Vector3 startPoint)
    {
        var randomNumberX = Random.Range(-spread, spread);
        var randomNumberY = Random.Range(-spread, spread);
        var randomNumberZ = Random.Range(-spread, spread);

        var bullet = Instantiate(bulletPrefab, startPoint, transform.rotation);
        bullet.transform.Rotate(randomNumberX, randomNumberY, randomNumberZ);
        bullet.Rb.velocity = bullet.transform.forward * bulletSpeed;
        bullet.damage = damage;
    }
}
 