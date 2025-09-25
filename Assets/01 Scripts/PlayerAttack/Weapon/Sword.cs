using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : BaseWeapon
{
    private MeleeWeaponSO MeleeData => weaponData as MeleeWeaponSO;
    private SwordHitbox hitbox;

    [SerializeField] private Collider attackHitbox; // 검 히트 박스
    [SerializeField] private TrailRenderer trailRenderer;

    protected override void Awake()
    {
        base.Awake();

        if (hitbox == null)
        {
            Debug.LogError("Sword: Hitbox가 연결되지 않았습니다! Hitbox 자식 오브젝트를 확인하세요.", this);
        }
        else if (hitbox.GetComponent<Collider>() != null)
        {
            hitbox.GetComponent<Collider>().enabled = false; // 시작 시 반드시 꺼두기
        }
        // Todo: 플레이어 애니메이터를 가져오는 로직
    }

    protected override void Start()
    {
        base.Start();
        scanner.detectCollider.radius = scanner.scanRange;
    }

    protected override void PerformAttack(Transform target)
    {
        // 공격 방향으로 플레이어(또는 무기)의 방향을 돌립니다.
        transform.root.LookAt(target); // transform.root는 플레이어 전체를 의미
        transform.root.eulerAngles = new Vector3(0, transform.root.eulerAngles.y, 0);

        // Todo: 애니메이션 시작, 검기 VFX 추가, 사운드 추가

        // 히트박스를 잠시 켰다 끄는 코루틴을 시작합니다.
        if (hitbox != null)
        {
            StartCoroutine(AttackHitboxRoutine());
        }
    }

    // 히트박스 공격용 루틴
    private IEnumerator AttackHitboxRoutine()
    {
        Debug.Log("검 공격 시작!");

        hitbox.StartAttack(MeleeData.Damage);
        // 히트박스를 켭니다.
        hitbox.enabled = true;

        // 휘두르기 모션? 등을 추가

        // 0.3초 동안 공격 판정을 유지합니다.
        // melee SO에 추가해도 좋을 것 같습니다.
        yield return new WaitForSeconds(MeleeData.hitboxActiveDuration);

        // 히트박스를 다시 끕니다.
        hitbox.enabled = false;

        Debug.Log("검 공격 종료!");
    }
}
