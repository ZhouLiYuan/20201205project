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

    public CircleCollider2D interactCollider;
    public float IntercatRange = 2f;
    public bool isInteractingWithPlayer = false;

    public InteractableData interactingData;
    //玩家正在交互的面板
    public NPCInteractablePanel interactingPanel;
    //NPC对话可能会需要访问上一个面板的
    public Dictionary<PlayerInteractDetect, InteractableData> InteractablePanels = new Dictionary<PlayerInteractDetect, InteractableData>();
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
        animator = obj.GetComponent<Animator>();
        rb2d = obj.GetComponent<Rigidbody2D>();
        interactCollider = obj.GetComponent<CircleCollider2D>();
        interactCollider.radius = IntercatRange;
        

    
        //updater = GameObject.FindObjectOfType<Updater>();
        updater = Updater.AddUpdater(obj);
        updater.AddUpdateFunction(OnUpdate);
        updater.AddFixedUpdateFunction(OnFixedUpdate);
        InitFSM();
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
        InteractFsm.AddState<NPCIdleState>().SetRole(this);
        InteractFsm.AddState<NPCInteractState>().SetRole(this);
    }

    private void OnUpdate(float deltaTime) 
    {
        InteractFsm.Update(deltaTime);
    }

    private void OnFixedUpdate(float fixedDeltaTime)
    {
        InteractFsm.FixedUpdate(fixedDeltaTime);
    }

    // interactingPanel持有者就是NPC本身
    public void Interact(GameObject owner)
    {
        interactingPanel = UIManager.Open<NPCInteractablePanel>();//今后可能根据角色不同对应面板也不同
        interactingPanel.SetOwner(owner);
        interactingPanel.SetHintUI(owner);

        //生成位置还需要具体调整RectTransform(看看这种情况需不要单独弄个小canvas) 小canvas.transform =owner.transform
        //interactingPanel.m_transform.SetParent(小canvas transform, false);
    }

}
