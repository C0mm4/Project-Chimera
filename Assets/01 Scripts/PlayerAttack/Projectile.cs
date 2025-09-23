using UnityEngine;

/// <summary>
/// 발사체 이동 로직
/// </summary>
/// 
public class Projectile : MonoBehaviour
{
    private Transform targetTransform;      // 현재 타겟의 위치
    private Vector3 startPosition;          // 발사체 시작 위치(플레이어 or 무기)

    private Vector3 lastKnownPosition;      // 타겟이 죽었을 때 예외 처리용 위치

    private float speed;                    // 발사체 속도
    private float arcHeight;

    private float totalDistance;            // 전체 이동해야 할 거리
    private Vector3 lastPosition;

    public void Initialize(Transform start, Transform target, float projectileSpeed, float projectileArcHeight)
    {
        this.transform.position = start.position;
        this.startPosition = start.position;
        this.targetTransform = target;

        this.speed = projectileSpeed;
        this.arcHeight = projectileArcHeight;
        this.lastPosition = transform.position;

        if (targetTransform != null)
        {
            lastKnownPosition = targetTransform.position;
            totalDistance = Vector3.Distance(startPosition, lastKnownPosition);
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
        // 수평 이동 로직(몬스터 추적)
        Vector3 nextHorizontalPos = Vector3.MoveTowards(
            new Vector3(transform.position.x, 0, transform.position.z),
            new Vector3(lastKnownPosition.x, 0, lastKnownPosition.z),
            speed * Time.deltaTime
        );

        // 전체 이동 거리중 어느 정도 거리에 왔는가 (0~1, 진행도라 생각하면 됨)
        float distanceTraveled = Vector3.Distance(new Vector3(startPosition.x, 0, startPosition.z), nextHorizontalPos);
        float yOffset = 0;

        // 수직 이동 로직(포물선 이동)
        if (totalDistance > 0)
        {
            float journeyFraction = distanceTraveled / totalDistance;
            journeyFraction = Mathf.Clamp01(journeyFraction); // 안전장치
            yOffset = Mathf.Sin(journeyFraction * Mathf.PI) * arcHeight;
        }

        // 최종 위치 적용
        transform.position = new Vector3(nextHorizontalPos.x, startPosition.y + yOffset, nextHorizontalPos.z);
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

    // 목표에 도착했는지 확인
    private void CheckForArrival()
    {
        // 거리가 매우 가까워 지면 발사체 파괴(오브젝트 풀링 처리 필요)
        if (Vector3.Distance(transform.position, lastKnownPosition) < 0.1f)
        {
            Destroy(gameObject);
            // Todo: 이팩트, 사운드, 데미지 처리 등등
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        string name = gameObject.name;
        name = name.Replace("(Clone)", "");
        
        ObjectPoolManager.Instance.ResivePool(name, gameObject);
    }
}
