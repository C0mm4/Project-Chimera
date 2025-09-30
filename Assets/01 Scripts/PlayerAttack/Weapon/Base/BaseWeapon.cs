using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 무기 공격 로직
/// </summary>
public abstract class BaseWeapon : MonoBehaviour
{

    [SerializeField] protected BaseWeaponSO weaponData;
    protected float lastAttackTime;


    protected EnemyScanner scanner;
    public Transform InstigatorTrans;

    public BaseWeaponSO GetWeaponData()

    {
        return weaponData;
    }



    public void SetWeapon(BaseWeaponSO weapon, Transform originTrans)
    {
        this.weaponData = weapon;
        InstigatorTrans = originTrans;
    }

    public void Attack(Transform target)

    {
        // 1단계 공격 쿨타임 확인
        if (Time.time < lastAttackTime + weaponData.AttackCooldown)
        {
            return;
        }

        // 2단계 공격 가능한 타겟이 있는지 확인
        if (target == null)
        {
            return; // 타겟이 없으면 아무것도 안 함
        }

        TargetView(target);
        PerformAttack(target);
        // 마지막 공격 시간 갱신
        lastAttackTime = Time.time;
    }

    // 추가 필요 (플레이어 위치, 방향에 따라 활의 위치 변경?)
    protected abstract void PerformAttack(Transform target);

    //방향 전환
    private void TargetView(Transform target)
    {
        Vector3 dir = target.position - transform.position;

        float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);

        transform.DORotateQuaternion(targetRotation, 0.1f).SetEase(Ease.OutSine);
    }

    public virtual void OnPoolingDisable()
    {

    }
}

