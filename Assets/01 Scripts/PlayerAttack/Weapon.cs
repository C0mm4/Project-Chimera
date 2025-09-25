using UnityEngine;

/// <summary>
/// 무기 공격 로직
/// </summary>
public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponData;
    private float lastAttackTime;
    
    private EnemyScanner scanner;


    private void Awake()
    {
        scanner = GetComponent<EnemyScanner>();
        
        ObjectPoolManager.Instance.CreatePool(weaponData.ProjectilePrefab.name,transform, 10);
    }

    private void Start()
    {
        SetData();
    }

    public void SetWeapon(WeaponSO newWeapon)
    {
        weaponData = Instantiate(newWeapon);
        SetData();
    }

    private void SetData()
    {
        scanner.scanRange = weaponData.ScanRange;
        scanner.detectCollider.radius = scanner.scanRange;
    }

    public void Attack()
    {
        // 1단계 공격 쿨타임 확인
        if (Time.time < lastAttackTime + weaponData.AttackCooldown)
        {
            return;
        }

        // 2단계 공격 가능한 타겟이 있는지 확인
        if (scanner.nearestTarget == null)
        {
            return; // 타겟이 없으면 아무것도 안 함
        }

        // 3단계 공격 시작
        GameObject projectileObj = ObjectPoolManager.Instance.GetPool(weaponData.ProjectilePrefab.name);
        Projectile projectile = projectileObj.GetComponent<Projectile>();

        projectile.Initialize(transform, scanner.nearestTarget, weaponData);

        // 마지막 공격 시간 갱신
        lastAttackTime = Time.time;
    }

    // 추가 필요 (플레이어 위치, 방향에 따라 활의 위치 변경?)
}
