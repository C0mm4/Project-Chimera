using UnityEngine;

/// <summary>
/// 발사체 이동 로직
/// </summary>
/// 
public class Projectile : MonoBehaviour
{
    public Transform Instigator;

    private Transform targetTransform;      // 현재 타겟의 위치
    private Vector3 startPosition;          // 발사체 시작 위치(플레이어 or 무기)

    private Vector3 lastKnownPosition;      // 타겟이 죽었을 때 예외 처리용 위치

    private float speed;                    // 발사체 속도
    private float arcHeight;
    private float damage;

    // 회전 계산 용 변수
    private Vector3 lastPosition;

    // 시간 기반 궤적 계산
    private float flightDuration;            // 전체 이동해야 할 거리
    private float timeElapsed;



    public void Initialize(Transform start, Transform target, RangedWeaponSO weaponData)
    {
        this.transform.position = start.position;
        this.startPosition = start.position;
        this.targetTransform = target;

        this.speed = weaponData.ProjectileSpeed;
        this.damage = weaponData.Damage;
        //this.arcHeight = projectileArcHeight; // 동적 높이 계산으로 변경
        this.lastPosition = transform.position;

        if (targetTransform != null)
        {
            lastKnownPosition = targetTransform.position;
            float distance = Vector3.Distance(startPosition, lastKnownPosition);

            // 거리에 비례한 포물선 높이 동적 할당
            // ScanRange를 최대 사거리 기준으로 삼아 비율 계산
            float arcRatio = Mathf.Clamp01(distance / weaponData.ScanRange);
            this.arcHeight = weaponData.ProjectileArcHeight * arcRatio; // 최종 높이 결정

            flightDuration = distance / (speed + 0.001f); // 시간 = 거리 / 속력인대 여기서 speed에 0.001f를 더하는 것은
                                                          //만약 speed가 0이 되어도 아주 작은 값을 더해 오류가 발생하는 것을 막음
            timeElapsed = 0f;
        }
    }


    private void Update()
    {
        UpdateTargetPosition();
        MovementToTarget();
        RotationToTarget();
        CheckForArrival();
    }
    
    // 타겟이 죽었는지 확인, 살아있다면 마지막 위치를 갱신
    private void UpdateTargetPosition()
    {
        if (targetTransform != null)
        {
            lastKnownPosition = targetTransform.position;
        }
    }

    // 발사체의 이동 로직 메서드
    private void MovementToTarget()
    {
        /*
        // 수평 이동 로직(몬스터 추적)
        Vector3 nextHorizontalPos = Vector3.MoveTowards(
            new Vector3(transform.position.x, 0, transform.position.z),
            new Vector3(lastKnownPosition.x, 0, lastKnownPosition.z),
            speed * Time.deltaTime
        );

        timeElapsed += Time.deltaTime;
        float journeyFraction = timeElapsed / flightDuration;

        // 높이 계산 추가 로직
        // 비행 진행도에 따라 발사 높이에서 타겟 높이까지 부드럽게 변하도록 변경
        float baseHeight = Mathf.Lerp(startPosition.y, lastKnownPosition.y, journeyFraction);

        // 동적 journeyFraction 계산으로 수정
        float yOffset = 0;
        if (journeyFraction > 0)
        {
            journeyFraction = Mathf.Clamp01(journeyFraction); // 안전장치
            yOffset = Mathf.Sin(journeyFraction * Mathf.PI) * arcHeight;
        }

        // 최종 위치 적용
        transform.position = new Vector3(nextHorizontalPos.x, baseHeight + yOffset, nextHorizontalPos.z);
        */

        // 시간 기반으로 전체 여정의 진행도(0.0 ~ 1.0)를 계산합니다.
        timeElapsed += Time.deltaTime;
        float journeyFraction = timeElapsed / flightDuration;
        journeyFraction = Mathf.Clamp01(journeyFraction); // 안전장치

        // Lerp를 사용해 시작점과 목표점을 잇는 직선상의 현재 위치를 계산합니다.
        Vector3 basePosition = Vector3.Lerp(startPosition, lastKnownPosition, journeyFraction);

        // Sin 함수를 이용해 포물선의 높이(Y축 오프셋)를 계산합니다.
        float yOffset = Mathf.Sin(journeyFraction * Mathf.PI) * arcHeight;

        // 직선 위치에 포물선 높이를 더해 최종 위치를 결정합니다.
        transform.position = basePosition + new Vector3(0, yOffset, 0);
    }


    // 회전 처리 로직
    private void RotationToTarget()
    {
        Vector3 direction = transform.position - lastPosition;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        lastPosition = transform.position;
    }

    // 타겟이 죽어서 마지막 위치에 도달했을 때
    private void CheckForArrival()
    {
        string name = gameObject.name;
        name = name.Replace("(Clone)", "");

        if (targetTransform != null) return; // 타겟이 살아있다면 OnCollisionEnter가 동작

        float horizontalDistance = Vector3.Distance(
            new Vector3(transform.position.x, 0, transform.position.z),
            new Vector3(lastKnownPosition.x, 0, lastKnownPosition.z)
            );

        if (horizontalDistance < 0.2f)
        {
            //gameObject.SetActive(false);
            ObjectPoolManager.Instance.ResivePool(name, gameObject);
        }
    }


    // 살아있는 타겟 공격시 발동
    private void OnCollisionEnter(Collision collision)
    {
        string name = gameObject.name;
        name = name.Replace("(Clone)", "");

        // 부딪힌 상대가 지정한 타겟이 맞는지 확인
        if (targetTransform != null && collision.transform == targetTransform)
        {
            // 타겟이 맞다면 데미지를 준다.
            if(collision.gameObject.TryGetComponent<CharacterStats>(out var status))
            {
                status.TakeDamage(damage);
                ObjectPoolManager.Instance.ResivePool(name, gameObject);
            }
        }
    }
}
