using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;

public class Zombie : LivingEntity
{
    public enum Status
    {
        Idle,
        Trace,
        Attack,
        Die,
    }

    public Transform target;
    public LayerMask targetLayer;

    public float traceDistance = 10f;
    public float attackDistance = 1f;

    public ParticleSystem hitEffect;
    public Collider zombieCollider;
    public Renderer zombieRenderer;
    public AudioClip hitClip;
    public AudioClip deathClip;
    public Image uiZombieHealth;


    private NavMeshAgent agent;
    private Animator zombieAnimator;
    private AudioSource zombieAudioSource;

    public ZombieData data;


    public float damage = 10;
    public float attackInterval = 1f;
    private float lastAttackTime;

    private Status currentStatus;

    public static readonly int IdHasTarget = Animator.StringToHash("hasTarget");
    public static readonly int IdDie = Animator.StringToHash("isDead");


    public Status CurrentStatus
    {
        get => currentStatus;
        set
        {
            currentStatus = value;
            Debug.Log(currentStatus);
            switch(currentStatus)
            {
                case Status.Idle: //대기 상태일때
                    zombieAnimator.SetBool(IdHasTarget, false);//추적 애니메이션 아님
                    agent.isStopped = true; //이동중지
                    break;
                case Status.Trace:
                    zombieAnimator.SetBool(IdHasTarget, true);
                    agent.isStopped = false;
                    break;
                case Status.Attack:
                    zombieAnimator.SetBool(IdHasTarget, false);
                    agent.isStopped = true;
                    break;
                case Status.Die:
                    agent.isStopped = true;
                    zombieAnimator.SetTrigger(IdDie);
                    zombieCollider.enabled = false;
                    zombieAudioSource.PlayOneShot(deathClip);
                    break;

            }
        }
    }

    //초기화 코드
    public void Setup()
    {
        startingHealth = data.maxHp; //최대체력
        damage = data.damage;//초기데미지
        agent.speed = data.speed;//초기스피드
        zombieRenderer.material.color = data.skinColor;//기본색상

    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        zombieAnimator = GetComponent<Animator>();
        zombieAudioSource = GetComponent<AudioSource>();
        Setup();
    }

    protected override void OnEnable()
    {
        base.OnEnable();


        //!@# ???
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }


        zombieCollider.enabled = true;

        CurrentStatus = Status.Idle;

        uiZombieHealth.fillAmount = Health / startingHealth;

    }


    private void OnDisable()
    {
        if(agent.isOnNavMesh)
        {
            agent.ResetPath();
        }
    }

    private void Update()
    {
        switch (currentStatus)
        {
            case Status.Idle:
                UpdateIdle();
                break;
            case Status.Trace:
                UpdateTrace();
                break;
            case Status.Attack:
                UpdateAttack();
                break;
            case Status.Die:
                UpdateDie();
                break;
        }


    }

    private void UpdateDie(){}

    private void UpdateAttack()
    {
        //타겟이 사라지거나 공격범위에서 벗어났을경우
        if(target == null
            || Vector3.Distance(transform.position, target.position) > attackDistance)
        {
            CurrentStatus = Status.Trace;
            return;
        }

        zombieAnimator.SetBool("hasTarget", true);
        if(Time.time >= lastAttackTime + attackInterval) //!@# 원리가 이해잘안됨
        {
            lastAttackTime = Time.time;
            IDamageable damageable = target.GetComponent<IDamageable>();
            var hitNormal = (transform.position - target.position).normalized; //!@# nomalized?
            damageable.OnDamage(damage, target.position, hitNormal);
        }
        zombieAnimator.SetBool("hasTarget", false);
    }

    private void UpdateTrace()
    {
        if(
            target != null
            && Vector3.Distance(transform.position, target.position) < attackDistance)
        {
            CurrentStatus = Status.Attack;
            return;
        }

        if(target == null || Vector3.Distance(transform.position, target.position) > traceDistance)
        {
            target = null;
            CurrentStatus = Status.Idle;
            return;
        }

        if(target == null || Vector3.Distance(transform.position, target.position) > traceDistance)
        {
            target = null;
            CurrentStatus = Status.Idle;
            return;
        }
        agent.SetDestination(target.position);

    }

    private void UpdateIdle()
    {
        if(target != null && Vector3.Distance(transform.position, target.position) < traceDistance) //!@#
        {
            CurrentStatus = Status.Trace;
            return;
        }
        target = FindTarget(traceDistance);
    }

    private Transform FindTarget(float radius)
    {
        //오버랩 스피어 안에 들어온에들 (레이어 맞는애들) 배열에 집어넣음
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, targetLayer);

        Transform closet = null; //!@# 몰라
        float closetDistance = float.MaxValue; 

        for (int i = 0; i < colliders.Length; i++) //충돌체 전부 비교
        {
            float distance = Vector3.Distance(transform.position, colliders[i].transform.position);
            if(distance < closetDistance)
            {
                closetDistance = distance;
                closet = colliders[i].transform;
            }
        
        }
        return closet;
    }



    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (IsDead)
        {
            return;
        }
        base.OnDamage(damage, hitPoint, hitNormal);

        uiZombieHealth.fillAmount = Health / startingHealth;
        zombieAudioSource.PlayOneShot(hitClip);
        hitEffect.transform.position = hitPoint;
        hitEffect.transform.forward = hitNormal;
        hitEffect.Play();
        
    }

    public void StartSinking()
    {
        transform.DOMoveY(-5f, 10f)
    .SetRelative(true)
    .SetEase(Ease.Linear) // 등속도로 부드럽게 내려가기 (취향에 따라 Ease.InQuad 등으로 변경 가능)
    .OnComplete(() =>
    {
        // 이동이 끝나면 오브젝트 파괴
        gameObject.SetActive(false);
    });
    }

    public override void Die()
    {
        base.Die();

        CurrentStatus = Status.Die;
    }

}
