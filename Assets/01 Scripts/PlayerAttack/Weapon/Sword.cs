using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : BaseWeapon
{
    private MeleeWeaponSO MeleeData => weaponData as MeleeWeaponSO;

    [SerializeField] private Collider attackHitbox;
    [SerializeField] private TrailRenderer trailRenderer;

    // 타겟이 공격이 한 번만 맞도록 하는 리스트
    private List<Collider> targetsHitThisSwing;

    protected void Awake()
    {
        if (attackHitbox == null)
        {
            attackHitbox = GetComponent<Collider>();
        }

        //attackHitbox.enabled = false; // 시작 시 반드시 꺼두기
        targetsHitThisSwing = new List<Collider>();
        // Todo: 플레이어 애니메이터를 가져오는 로직
    }

    protected override void PerformAttack(Transform target)
    {
        // 공격 방향으로 플레이어(또는 무기)의 방향을 돌립니다.
        transform.root.LookAt(target); // transform.root는 플레이어 전체를 의미

        Vector3 currentRotation = transform.root.eulerAngles;
        currentRotation.x = 0;
        currentRotation.z = 0;
        transform.root.eulerAngles = currentRotation;

        // Todo: 애니메이션 시작, 검기 VFX 추가, 사운드 추가

        // 히트박스를 잠시 켰다 끄는 코루틴을 시작합니다.
        if (attackHitbox != null)
        {
            StartCoroutine(AttackHitboxRoutine());
        }
    }

    // 히트박스 공격용 루틴
    private IEnumerator AttackHitboxRoutine()
    {
        // 공격 시작 시, 맞은 타겟 리스트를 초기화
        targetsHitThisSwing.Clear();
        attackHitbox.enabled = true;
        //if (trailRenderer != null) trailRenderer.emitting = true;

        yield return new WaitForSeconds(MeleeData.hitboxActiveDuration);

        Debug.Log($"검 휘두르기 종료, 총 {targetsHitThisSwing.Count}명의 적을 타격했습니다.");

        attackHitbox.enabled = false;
        //if (trailRenderer != null) trailRenderer.emitting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 이미 이번 공격에서 맞았던 타겟이라면 무시
        if (targetsHitThisSwing.Contains(other))
        {
            return;
        }

        // LayerMask를 이용한 2차 필터
        if ((MeleeData.targetLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            if (other.TryGetComponent<CharacterStats>(out var status))
            {
                status.TakeDamage(MeleeData.Damage);
                targetsHitThisSwing.Add(other); // 맞은 적 목록에 추가
            }
        }
    }
}
