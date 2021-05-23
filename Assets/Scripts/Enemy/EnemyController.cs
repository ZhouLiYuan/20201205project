//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//[RequireComponent(typeof(Rigidbody))]
//public class EnemyController : MonoBehaviour
//{
//    public GameObject m_Player;
//    public Transform[] m_Path;
//    private En_FSMSystem m_FSM;

//    public void SetTransition(En_Transition t) { m_FSM.PerformTransition(t); }

//    public void Start()
//    {
//        MakeFSM();
//    }

//    public void FixedUpdate()
//    {
//        m_FSM.CurrentState.CheckTransition(m_Player, gameObject);
//        m_FSM.CurrentState.Act(m_Player, gameObject);
//    }

//    // NPC有两个状态: FollowPath(沿着路径巡逻) 和 ChasePlayer(追寻玩家)
//    // 当在FollowPath状态时，SawPlayer转换被触发时，将变为ChasingPlayer状态
//    // 当在ChasePlayer状态时，LostPlayer转换被触发，将变为FollowPath状态
//    private void MakeFSM()
//    {
//        FollowPathState follow = new FollowPathState(m_Path);
//        follow.AddTransition(En_Transition.SawPlayer, StateID.ChasingPlayer);

//        ChasePlayerState chase = new ChasePlayerState();
//        chase.AddTransition(En_Transition.LostPlayer, StateID.FollowingPath);

//        m_FSM = new En_FSMSystem();
//        m_FSM.AddState(follow);
//        m_FSM.AddState(chase);
//    }
//}

///// <summary>
///// FollowPath(沿着路径巡逻)
///// </summary>
//public class FollowPathState : EnemyState
//{
//    private int currentWayPoint;
//    private Transform[] waypoints;

//    public FollowPathState(Transform[] wp)
//    {
//        waypoints = wp;
//        currentWayPoint = 0;
//        stateID = StateID.FollowingPath;
//    }

//    public override void CheckTransition(GameObject player, GameObject npc)
//    {
//        //RaycastHit hit;
//        //if (Physics.Raycast(npc.transform.position, npc.transform.forward, out hit, 15f))
//        //{
//        //    if (hit.transform.gameObject.CompareTag("Player"))
//        //        npc.GetComponent<NPCControl>().SetTransition(Transition.SawPlayer);
//        //}

//        // 当Player距离NPC小于15米时，触发SawPlayer状态
//        Collider[] colliders = Physics.OverlapSphere(npc.transform.position, 5f);
//        if (colliders.Length <= 0)
//        {
//            return;
//        }
//        // 需要设置场景中Player的Tag为Player
//        if (colliders[0].transform.gameObject.CompareTag("Player"))
//            npc.GetComponent<NPCControl>().SetTransition(En_Transition.SawPlayer);
//    }

//    public override void Act(GameObject player, GameObject npc)
//    {
//        // 沿着路径点巡逻
//        Rigidbody rigidbody = npc.GetComponent<Rigidbody>();
//        Vector3 vel = rigidbody.velocity;
//        // 计算移动方向
//        Vector3 moveDir = waypoints[currentWayPoint].position - npc.transform.position;
//        // 如果距离小于1，前往下一个路径点
//        if (moveDir.magnitude < 1)
//        {
//            currentWayPoint++;
//            if (currentWayPoint >= waypoints.Length)
//            {
//                currentWayPoint = 0;
//            }
//        }
//        else
//        {
//            vel = moveDir.normalized * 10;
//            // 面向路径点
//            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
//                                                      Quaternion.LookRotation(moveDir),
//                                                      5 * Time.deltaTime);
//            npc.transform.eulerAngles = new Vector3(0, npc.transform.eulerAngles.y, 0);
//        }
//        rigidbody.velocity = vel;
//    }

//}

///// <summary>
///// ChasePlayer(追寻玩家)
///// </summary>
//public class ChasePlayerState : EnemyState
//{
//    public ChasePlayerState()
//    {
//        stateID = StateID.ChasingPlayer;
//    }

//    public override void CheckTransition(GameObject player, GameObject npc)
//    {
//        // 如果玩家距离NPC超出30米的距离，触发LostPlayer转换
//        if (Vector3.Distance(npc.transform.position, player.transform.position) >= 30)
//            npc.GetComponent<NPCControl>().SetTransition(En_Transition.LostPlayer);
//    }

//    public override void Act(GameObject player, GameObject npc)
//    {
//        Rigidbody rigidbody = npc.GetComponent<Rigidbody>();
//        Vector3 vel = rigidbody.velocity;
//        // 找到玩家的方向
//        Vector3 moveDir = player.transform.position - npc.transform.position;

//        // 面向路径点
//        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
//                                                  Quaternion.LookRotation(moveDir),
//                                                  5 * Time.deltaTime);
//        npc.transform.eulerAngles = new Vector3(0, npc.transform.eulerAngles.y, 0);
//        vel = moveDir.normalized * 10;
//        rigidbody.velocity = vel;
//    }

//}
