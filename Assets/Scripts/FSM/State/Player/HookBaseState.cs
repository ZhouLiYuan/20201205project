using System;
using UnityEngine;


namespace Role.SelectableRole
{
        public class HookBaseState : PlayerRoleState
        {
            //Hook相关
            protected GameObject HookGobj => Role.hookGobj;
            protected Transform HookTransform => HookGobj.transform;
            protected Vector3 HookPos => HookGobj.transform.position;

            //hookable逻辑Gobj与其父级Owner的信息
            protected Vector3 TargetPos => Role.TargetTransform.position;
            protected Transform TargetOwnerTransform => Role.lockTarget.transform.parent;
            protected Vector3 TargetOwnerPos => Role.lockTarget.transform.parent.position;

            //Hook与Target关系
            protected Vector3 direction;//玩家到目标的方向
            protected float HookToTargetDistance;
            protected float MinDistance => Role.minDistance;

            //禁用重力 和 输入对左右移动的控制
            public override void OnEnter()
            {
                base.OnEnter();
            }

            public override void OnUpdate(float deltaTime)//如果途中遇到障碍物就会打断取消返回默认状态
            {
                base.OnUpdate(deltaTime);
            }

            public override void OnExit()
            {
                base.OnExit();
                Role.hookGobj.transform.position = Role.Transform.position + Role.hookLocaloffsetPosSlash;//Hook回归原为（World换算到Local）
                Role.lockTarget = null;
            }

            protected void GenerateHookEffect(Vector3 spawnPos)
            {
                //特效生成  
                var hitEffectPrefab = ResourcesLoader.LoadEffectPrefab("ef_grap01");
                UnityEngine.Object.Instantiate(hitEffectPrefab, spawnPos, Quaternion.identity);
            }
        }
}