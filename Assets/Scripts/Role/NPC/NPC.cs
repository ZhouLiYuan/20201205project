using UnityEngine;
using System.Collections.Generic;
using System;

namespace Role
{
    namespace NPCs
    {
        public class NPC : RoleEntity
        {
            //层级
            public SpriteRenderer spriteRenderer;

            public enum InteractState { None, Talk, Trade, Quest }
            public InteractState interactState = InteractState.None;

            public CircleCollider2D interactCollider;
            public float IntercatRange = 2f;
            public bool isInteractingWithPlayer = false;

            //玩家正在交互的面板
            public NPCInteractablePanel interactingPanel;
            //NPC对话可能会需要访问上一个面板的(模仿状态机)
            public Dictionary<PlayerInteractDetect, BasePanel> InteractablePanels = new Dictionary<PlayerInteractDetect, BasePanel>();
            public PlayerInteractDetect PlayerInteractDetect { get; private set; }
            //FSM
            FSM InteractFsm;

            //对话内容相关Key
            [SerializeField]
            public int currentStoryID => StoryManager.currentStoryID;
            public string currentPlaceName;

            public Updater updater;

            public override void Init(GameObject obj)
            {
                base.Init(obj);
                //obj.name = NPCManager.CreateUniqueName();
                animator = obj.GetComponent<Animator>();

                spriteRenderer = obj.GetComponent<SpriteRenderer>();
                interactCollider = obj.GetComponent<CircleCollider2D>();
                interactCollider.radius = IntercatRange;
                PlayerInteractDetect = obj.GetComponent<PlayerInteractDetect>();

                //updater = GameObject.FindObjectOfType<Updater>();
                updater = Updater.AddUpdater(obj);
                updater.AddUpdateFunction(OnUpdate);
                updater.AddFixedUpdateFunction(OnFixedUpdate);
                InitFSM();

                //NPCManager.nameDic[UniqueName] = this;
            }

            public void InitProperties(NPCConfig config)
            {
                base.InitProperties(config);
            }


            private void InitFSM()
            {
                //分不同的FSM其实就是分层处理
                InteractFsm = new FSM();
                InteractFsm.AddState<NPCIdleState>().SetRole(this);
                InteractFsm.AddState<NPCPreInteractState>().SetRole(this);
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
                UIManager.SetInteractUIPosition(owner, interactingPanel.m_gameObject);
                //生成位置还需要具体调整RectTransform(看看这种情况需不要单独弄个小canvas) 小canvas.transform =owner.transform
                //interactingPanel.m_transform.SetParent(小canvas transform, false);
            }


            public void SetOutlineActive(bool IsNearest)
            {
                if (IsNearest) spriteRenderer.material = ResourcesLoader.LoadMaterial("Sprite_OutlineColor");
                else { spriteRenderer.material = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Material>("Sprites-Default.mat"); }
            }

            public void LookAtPlayer()
            {
                var playerPosX = PlayerManager.m_Role.GameObject.transform.position.x;
                //在Player右边，且未反转过（当前向右）
                if (Transform.position.x > playerPosX && spriteRenderer.flipX == false) spriteRenderer.flipX = true;
                //在Player左边，且反转过（当前向左）
                else if (Transform.position.x < playerPosX && spriteRenderer.flipX == true) spriteRenderer.flipX = false;
                else { }
            }

        }
    }
}