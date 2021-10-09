using UnityEngine;
using System.Collections.Generic;
using System;

public class NPC : Entity
{
    //层级
    //public GameObject NPCGobj;
    //public Transform topNodeTransform;
    public GameObject animatorGobj;

    public Animator animator;
    public Rigidbody2D rb2d;

    public CircleCollider2D InteractCollider;
    public float IntercatRange = 2f;
    public bool isInteractingWithPlayer = false;

    public InteractableData interactingData;
    //玩家正在交互的面板
    public NPCInteractablePanel interactingPanel;
    //NPC对话可能会需要访问上一个面板的

    //FSM
    FSM InteractFsm;

    //对话内容相关Key
    [SerializeField]
    public int currentStoryID =>StoryManager.currentStoryID;
    public string currentPlaceName;

    public Updater updater;

    public override void Init(GameObject obj) 
    {
        base.Init(obj);
        //NPCGobj = obj;
        InteractCollider.radius = IntercatRange;
        rb2d = obj.GetComponent<Rigidbody2D>();

        InitFSM();
        updater = GameObject.FindObjectOfType<Updater>();
        updater.AddUpdateFunction(OnUpdate);
        updater.AddFixedUpdateFunction(OnFixedUpdate);
    }

    public void InitProperties(NPCConfig config)
    {
        base.InitProperties(config);
        NPCManager.nameDic[name] = this;
    }


    private void InitFSM()
    {
        //分不同的FSM其实就是分层处理
        InteractFsm = new FSM();
        InteractFsm.AddState<NPC_IdleState>().SetRole(this);
        InteractFsm.AddState<NPC_InteractState>().SetRole(this);
    }

    private void OnUpdate(float deltaTime) 
    {
        InteractFsm.Update(deltaTime);
    }

    private void OnFixedUpdate(float fixedDeltaTime)
    {
        InteractFsm.FixedUpdate(fixedDeltaTime);
    }

    //持有者就是NPC本身
    //public event Action<GameObject> OnInteract;
    public void Interact(/*GameObject owner*/)
    {
        interactingPanel = UIManager.Open<NPCInteractablePanel>();
        //生成位置还需要具体调整(看看这种情况需不要单独弄个小canvas)
        //interactingPanel.m_transform.SetParent(owner.transform, false);
        interactingPanel.m_transform = this.Transform;
        //OnInteract?.Invoke(owner);
    }

}
