using StarterAssets;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class EnemyController : MonoBehaviour
{

    /*
     * 아이디어
     * 1. 각성상태인가?
     * 1.1 각성상태 -> 공격중인가?
     *      1.1.1 공격중임 -> 애니메이션이 끝나고, 대기시간이 지날때까지 대기
     *      1.1.2 공격중이 아님 -> 플레이어와 먼가?
     *              1.1.2.1 멀지않다->공격시작
     *              1.1.2.2 멀다->추적 시작
     *              
     * 1.2각성상태아님 -> 플레이어가 Null인가
     *      1.2.1 Null임 -> 대기
     *      1.2.2 Null이 아님 -> 시야안에 플레이어가 있는가?
     *          1.2.2.1 있다 -> 각성
     *          1.2.2.1 없다 -> 대기
     */
    private BehaviorTreeDS _behaviorTree;
    private Transform _playerTransform;
    public float moveSpeed=1f;

    bool isWakeup = false;
    bool isAttack = false;
    bool isDetect = false;
    bool isDie = false;

    float detectRange = 60f;

    float rangeAttack = 2f;

    float attackCoolTime = 4f;
    float attackCurrentTime = 0f;
    EnemyView view;

    CapsuleCollider _enemyCollider;
    EnemyPlayerDetector _enemyPlayerDetector;
    EnemyWakeUpOther _enemyWakeUpOther;
    NavMeshAgent agent;
    [SerializeField] SphereCollider _enemyHandCollider;

    private void Awake()
    {
        view = GetComponent<EnemyView>();
        _enemyPlayerDetector = transform.GetChild(2).GetComponent<EnemyPlayerDetector>();
        _enemyWakeUpOther = transform.GetChild(3).GetComponent<EnemyWakeUpOther>();
        _enemyCollider = transform.GetComponent<CapsuleCollider>();
        if (_enemyHandCollider == null)
            _enemyHandCollider = GetComponentInChildren<EnemyHandController>().GetComponent<SphereCollider>();
        
        agent = transform.GetComponent<NavMeshAgent>();
        
    }
    private void OnEnable()
    {
        _enemyPlayerDetector.PlayerDetect += DetectPlayer;
    }
    private void OnDisable()
    {
        _enemyPlayerDetector.PlayerDetect -= DetectPlayer;
    }
    private void DetectPlayer(bool isDetect)
    {
        //Debug.Log("WakeUP : " + isDetect);
        this.isDetect = isDetect;
        view.EnemyDetect(this.isDetect);
    }

    private void Start()
    {
        //_playerTransform = FindObjectOfType<PlayerController>().transform;
        _playerTransform = FindObjectOfType<ThirdPersonController>().transform;
        BehaviorSetting();
        agent.stoppingDistance = rangeAttack;
        _enemyHandCollider.enabled = false;
    }
    //해당 함수가 실행되어야만 몬스터가 일어남!
    public void ForceWakeUP()
    {
        //Debug.Log("Enemy WakeUp");
        isWakeup = true;
        view.EnemyWakeUp();
        _enemyWakeUpOther.gameObject.SetActive(true);
    }


    
    private void BehaviorSetting()
    {
        SelectorNode rootSelector = new SelectorNode();

        ConditionalNode dieCondition = new ConditionalNode(
            ()=>
                {
                    return isDie;
                },
            ()=>
                {
                    
                }
            );
        rootSelector.AddChild( dieCondition );

        // ------------------- Behavior :  공격 상태인지 확인하기 위함.
        ConditionalNode attackCondition = new ConditionalNode(
            () =>
                {
                    return isAttack;
                },
            () =>
            {
                //공격중인 상황이라면? 시간초가 지났는지 확인할 것.
                if(Time.time-attackCurrentTime>attackCoolTime)
                {
                    isAttack = false;
                }
            }
            );

        rootSelector.AddChild(attackCondition);
        // ------------------- Behavior :  공격 상태인지 확인하기 위함.
        // ------------------- Behavior :  현재 꺠어나있는 상태인지 체크
        SequenceNode wakeupSequence = new SequenceNode();
        ConditionalNode wakeupCondition = new ConditionalNode(
                () => 
                { 
                    return isWakeup; 
                },
                () => 
                { 
                }
                                                             );
        SelectorNode disCheckSelector = new SelectorNode();
        ConditionalNode disFarCheck = new ConditionalNode(
                () =>
                {
                    float dis = Vector3.Distance(transform.position, _playerTransform.position);
                    return dis > rangeAttack;       //공격거리보다 거리가 더 멀다면...추적해야겠지?
                },
                () =>
                {
                    //거리가 멀 경우 계속 추적. 위치 갱신함.
                    agent.isStopped = false;
                    view.EnemyChase(true);
                    agent.destination = _playerTransform.position;
                }

            );
        ConditionalNode disCloseCheck = new ConditionalNode(
            () =>
            {
                float dis = Vector3.Distance(transform.position, _playerTransform.position);
                return dis <= rangeAttack;       //공격거리보다 거리가 더 작다? 도착!
            },
            () => 
            {
                //Debug.Log("EnemyAttack!");
                agent.velocity = Vector3.zero;
                attackCurrentTime = Time.time;
                agent.isStopped = true;
                view.EnemyChase(false);
                isAttack = true;        //공격중임을 표시하고
                view.EnemyAttack();     //공격 애니메이션 실행
                transform.LookAt(_playerTransform.position);
                //이때 공격 애니메이션을 실행할 것.
                //공격 후 1초뒤에 주먹 콜라이더 켜기!
                StartCoroutine(OneSecondLaterPunch());
            }
            );

        wakeupSequence.AddChild(wakeupCondition);
        wakeupSequence.AddChild(disCheckSelector);

        disCheckSelector.AddChild(disFarCheck);
        disCheckSelector.AddChild(disCloseCheck);
        rootSelector.AddChild(wakeupSequence);
        // ------------------- Behavior :  현재 깨어나있는 상태인지 체크
        // ------------------- Behavior :  현재 감지 상태인지 체크

        SequenceNode detectSequence = new SequenceNode();
        ConditionalNode detectCondition = new ConditionalNode(
            () =>
                {
                    return isDetect;
                },
            () =>
                {
                    //감지상태가 맞을 경우, 별도로 실행할 것은 현재 없음.
                }
            );
        ConditionalNode detectAngleCondition = new ConditionalNode(
            () =>
                {
                    //이 상태의 경우 감지상태가 맞을 경우 실행됨. 플레이어가 뛰고있는지, 범위 안에 들어있는지 체크하고, 맞을 경우 각성하기.
                    Vector3 directionToPlayer = (_playerTransform.position - transform.position).normalized;
                    Vector3 enemyForward = transform.forward.normalized;
                    float angle = Vector3.Angle(directionToPlayer, enemyForward);
                    //Debug.Log("current Angle : " + angle);
                    return (angle>-detectRange && angle< detectRange);
                },
            () =>
                {
                    ForceWakeUP();
                    //주변을 꺠우는 코드도 필요함.
                    _enemyWakeUpOther.transform.gameObject.SetActive(true);
                }
            );
        detectSequence.AddChild(detectCondition);
        detectSequence.AddChild(detectAngleCondition);
        rootSelector.AddChild(detectSequence);

        // ------------------- Behavior :  현재 감지 상태인지 체크




        _behaviorTree = new BehaviorTreeDS(rootSelector);
    }

        
    public void EnemyHitByPlayer(Vector3 hitPosition)
    {
        //몬스터가 플레이어에게 피격당했을 경우, 실행될 구문
        if(isDie==false)
        {
            //Debug.Log("EnemyDie Active!");
            isDie = true;
            isWakeup = false;
            isAttack = false;
            isDetect = false;
            _enemyPlayerDetector.gameObject.SetActive(false);
            _enemyWakeUpOther.gameObject.SetActive(false);
            _enemyCollider.enabled = false;
            agent.isStopped = true;
            view.EnemyDie(hitPosition);
        }
    }

    private void Update()
    {
        _behaviorTree.Tick();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("EnemyForceWakeUp"))
        {
            //랜덤하게 시간 지난 후0~1초
            StartCoroutine(WaitAndWakeup());
        }
        if(other.CompareTag("PlayerWeapon"))
        {
            //Debug.Log("Hit");
        }
    }

    IEnumerator WaitAndWakeup()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.5f));
        ForceWakeUP();
    }










    private void OnDrawGizmos()
    {
        if(_enemyPlayerDetector!=null)
        {
            //몬스터의 detect범위를 보여줌
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_enemyPlayerDetector.transform.position, _enemyPlayerDetector.detectRange);

            //몬스터의 인식각도를 보여줌
            Gizmos.color = Color.blue;
            Vector3 enemyForward = transform.forward;
            Vector3 enemyPosition = transform.position;
            Vector3 leftVector = Quaternion.Euler(0, -detectRange, 0) * enemyForward;
            Vector3 rightVector= Quaternion.Euler(0, detectRange, 0) * enemyForward;
            Gizmos.DrawLine(enemyPosition, enemyPosition + leftVector * _enemyPlayerDetector.detectRange);
            Gizmos.DrawLine(enemyPosition, enemyPosition + rightVector * _enemyPlayerDetector.detectRange);
        }


    }

    WaitForSeconds OneSecond = new WaitForSeconds(0.8f);
    WaitForSeconds DotOneSecond = new WaitForSeconds(0.2f);
    IEnumerator OneSecondLaterPunch()
    {
        yield return OneSecond;
        _enemyHandCollider.enabled = true;
        yield return DotOneSecond;
        _enemyHandCollider.enabled = false;
    }





    /* Trash Can
     
    /*
        SelectorNode wakeupSelector = new SelectorNode();
        ConditionalNode isAttackCondition = new ConditionalNode(IsAttackCondition, IsAttackAction);
        ConditionalNode canAttackCondition = new ConditionalNode(CanAttackCondition, CanAttackAction);
        ConditionalNode traceCondition = new ConditionalNode(TraceCondition, TraceAction);
        wakeupSelector.AddChild(isAttackCondition);
        wakeupSelector.AddChild(canAttackCondition);
        wakeupSelector.AddChild(traceCondition);
        rootSelector.AddChild(wakeupSelector);
        */
    /*
    ConditionalNode wakeupCondition = new ConditionalNode(WakeUpCondition, WakeUpAction);
    ConditionalNode idleCondition = new ConditionalNode(IdleCondition, IdleAction);
    rootSelector.AddChild(wakeupCondition);
    rootSelector.AddChild(idleCondition);
    */
    // 비각성 상태
    /*
    ConditionalNode playerApproachNode = new ConditionalNode(PlayerApproachCondition, PlayerApproachAction);

    SelectorNode SelectorTest = new SelectorNode();
    ConditionalNode testSequence1 = new ConditionalNode(TestSequence1, LookAtPlayerAction);
    ConditionalNode testSequence2 = new ConditionalNode(TestSequence2, LookAtPlayerAction);

    SelectorTest.AddChild(testSequence1);
    SelectorTest.AddChild(testSequence2);

    Func<bool> lookatPlayerCondition = new Func<bool>(LookAtPlayerCondition);
    ConditionalNode lookatPlayerNode = new ConditionalNode(lookatPlayerCondition, LookAtPlayerAction);
    Func<bool> moveToPlayerCondition = new Func<bool>(MoveToPlayerondition);
    ConditionalNode moveToPlayerNode = new ConditionalNode(moveToPlayerCondition, MoveToPlayerAction);

    rootSelector.AddChild(playerApproachNode);
    rootSelector.AddChild(SelectorTest);
    //rootSelector.AddChild(lookatPlayerNode);
    rootSelector.AddChild(moveToPlayerNode);
    */

    /*
    
    // ------------------------------------ 각성상태에 따른 함수 구현 ------------------------------------ 
    private bool IsAttackCondition()
    {
        return isAttack;
    }
    private void IsAttackAction()
    {
        if (attackFlag == false)
        {
            Debug.Log("2초간 공격대기");
            attackFlag = true;
            StartCoroutine(Attacking());
        }
    }
    IEnumerator Attacking()
    {
        //공격 애니메이션 시간 동안 기다림.
        Debug.Log("Attack Start");
        yield return new WaitForSeconds(2);
        isAttack = false;
        attackFlag = false;
        Debug.Log("Attack End");
    }
    
    private bool CanAttackCondition()
    {
        //플레이어 거리가 짧은지 확인함. 참이면 공격 시작.
        float dis = Vector3.Distance(transform.position, _playerTransform.position);
        return rangeAttack > dis;       //참이면 공격
    }
    private void CanAttackAction()
    {
        isAttack = true;
        //공격 애니메이션 시작
        Debug.Log("PlayerDisCheckAction : Attack Start");
    }
    private bool TraceCondition()
    {
        float dis = Vector3.Distance(transform.position, _playerTransform.position);
        return (isWakeup==true) && (rangeAttack <= dis);       //참이면 추적
    }
    private void TraceAction()
    {
        agent.destination = _playerTransform.position;

        //Vector3 moveDirection = (transform.position- _playerTransform.position).normalized;
        //transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        //Debug.Log("Trace!!");
        //플레이어를 추적...
    }
    */
    // ------------------------------------ 각성상태에 따른 함수 구현 ------------------------------------ 
    // ------------------------------------ 비 각성상태에 따른 함수 구현 ------------------------------------ 
    /*
    private bool WakeUpCondition()
    {
        return isWakeup;
    }
    private void WakeUpAction()
    {
        //wakeup은 자식인 detector가 관리함.
        Debug.Log("Idle->WakeUp");
        isAttack = true;
    }
    private bool IdleCondition()
    {
        return true;
    }
    private void IdleAction()
    {
        
    }
    */

    // ------------------------------------ 비 각성상태에 따른 함수 구현 ------------------------------------ 
    /*

    private bool PlayerApproachCondition()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        Debug.Log("PlayerApproachCondition : " + (distanceToPlayer > 20f));
        return distanceToPlayer > 20f;
    }
    private void PlayerApproachAction()
    {
        Debug.Log("Wait to Player...");
    }
    private bool LookAtPlayerCondition()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        Debug.Log("LookAtPlayerCon : " + (distanceToPlayer >= 10f));
        return distanceToPlayer >= 10f;
    }
    private void LookAtPlayerAction()
    {
        Debug.Log("LookAtPlayerAction");
        Vector3 lookDirection = _playerTransform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(lookDirection);
    }
    private bool MoveToPlayerondition()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        Debug.Log("MoveToPlayerCon : " + (distanceToPlayer < 10f));
        return distanceToPlayer < 10f;
    }
    private void MoveToPlayerAction()
    {
        Debug.Log("MoveToPlayerAction");
        Vector3 moveDirection = (transform.position- _playerTransform.position).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
    
    private bool TestSequence1()
    {
        float dis = Vector3.Distance(transform.position, _playerTransform.position);
        Debug.Log("Test1. >=15");
        return dis >= 15;
    }
    private bool TestSequence2()
    {
        float dis = Vector3.Distance(transform.position, _playerTransform.position);
        Debug.Log("Test2. >=10");
        return dis >= 10;
    }
     */
}
