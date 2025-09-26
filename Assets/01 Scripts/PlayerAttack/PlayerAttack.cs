using UnityEngine;

/// <summary>
/// 임시 공격 호출 스크립트
/// </summary>
public class PlayerAttack : MonoBehaviour
{
    // 인스펙터 창에서 현재 장착 중인 무기를 연결해줄 변수
    [SerializeField] private BaseWeapon currentWeapon;
    [SerializeField] private EnemyScanner scanner;
    
    private void Start()
    {
        // 게임 시작 시, 현재 무기의 데이터에 맞춰 스캐너 범위 설정
        ApplyWeaponScanRange();
    }

    private void Update()
    {
        // 무기가 연결되어 있는지 확인하고, 연결되어 있다면 공격 명령
        if (scanner != null && scanner.nearestTarget != null)
        {
            if (currentWeapon != null)
            {
                currentWeapon.Attack(scanner.nearestTarget);
            }
        }
    }

    // 무기별 탐지 범위 가져오기
    private void ApplyWeaponScanRange()
    {
        if (currentWeapon != null && scanner != null)
        {
            // 1. 무기로부터 WeaponSO 데이터를 가져옵니다.
            BaseWeaponSO data = currentWeapon.GetWeaponData();
            // 2. 스캐너의 탐지 범위를 SO에 있는 값으로 설정합니다.
            scanner.scanRange = data.ScanRange;
            scanner.detectCollider.radius = scanner.scanRange;
        }
    }

    // 무기 교체시 현재 무기의 스캔 범위 변경 메서드
    public void EquipNewWeapon(BaseWeapon newWeapon)
    {
        currentWeapon = newWeapon;
        currentWeapon.InstigatorTrans = transform;
        ApplyWeaponScanRange();
    }


}
