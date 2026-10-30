using UnityEngine;
using System.Collections;
using UnityEngine.AI;



public class EnemySpawner : MonoBehaviour
{
    //필요한 기능
    //랜덤 적을
    //랜덤한 위치에 생성
    //시간이 갈수록 생성량 증가!

    //5초마다
    //랜덤 위치에서 적 소환
    //적은 지하에서 y 0.1까지 올라오고 행동 시작

    [SerializeField] private GameObject[] enemyList;
    [SerializeField] private int spawnCount = 3;
    [SerializeField] private int spawnNumber = 0;

    [Header("구역 설정")]
    public Vector3 centerPoint;     // 스폰 중심 기준점
    public float spawnRadius = 20f; // 스폰 반경


    private void Awake()
    {
        //일단 미리 소환

    }

    void Start()
    {
        // 중심점을 이 스크립트가 붙은 오브젝트의 위치로 초기화
        centerPoint = transform.position;

        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator Spawn()
    {


        while(true)
        {

            yield return new WaitForSeconds(5f);

            if (spawnNumber >0 && spawnNumber % 5 == 0)
            {
                spawnCount++;
            }

            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 randomPoint;
                // 올바른 NavMesh 위치를 찾을 때까지 반복 시도하는 구문 (안전 장치)
                if (TryGetRandomNavMeshPoint(centerPoint, spawnRadius, out randomPoint))
                {
                    int rand = Random.Range(0, enemyList.Length);
                    Instantiate(enemyList[rand], randomPoint, Quaternion.identity);
                    //성공적으로 위치를 찾았다면 프리팹 생성
                    //일단 소환부터나 해보자



                }
                else
                {
                    Debug.LogWarning("NavMesh 위의 올바른 위치를 찾지 못했습니다.");
                }
            }

            spawnNumber++;
            StartCoroutine(Spawn());
        }

    }


    private bool TryGetRandomNavMeshPoint(Vector3 center, float radius, out Vector3 result)
    {
        // 1. 중심점 기준, 지정된 반경 내의 무작위 3D 구체 안의 점을 선택
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += center;

        NavMeshHit hit;

        // 2. SamplePosition(가상의 위치, 결과 담을 변수, 탐색 반경, NavMesh 레이어 마스크)
        // 가상의 randomDirection에서 'radius' 거리 안에 있는 가장 가까운 NavMesh 표면을 
        if(NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;


    }

    // 에디터 뷰에서 스폰 반경을 시각적으로 확인하기 위한 기즈모
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
