using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//区分开attacker是武器持有者    weapon是武器
//一次只能收到一个对象的攻击
//需要和 受击框collider2d 在一个层级
public class DamageReceiver : MonoBehaviour
{
    //受动者(脚本拥有者) 这两个信息要不要也放在DamageData里面
    public GameObject ownerGobj;
    public Collider2D ownerCollider;
    public LayerMask ownerLayer;

    private void OnEnable()
    {
        //根据BaseDamageReceiver所在层级换API获取
        ownerGobj = transform.parent.gameObject;
        ownerCollider = transform.GetComponent<Collider2D>();
        //ownerLayer = ownerGobj.layer;

    }

    //配合Edit->Project Setting ->physics设置可以相互碰撞的图层
    //只有Attacker和attackble可以相互碰撞
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //获取武器
        GameObject weaponGobj = collision.gameObject;
        LayerMask colliderLayer = weaponGobj.layer;
        //施动者: 从武器层级网上找 找到爷层级Gobj
        GameObject attackerGobj = weaponGobj.transform.parent.parent.gameObject;
        LayerMask attackerLayer = attackerGobj.layer;

        //碰撞后只要父级的图层并不是同一层，就可以相互伤害
        if (colliderLayer == LayerMask.NameToLayer("Attacker") && attackerLayer != ownerLayer)
        {
            //当攻击者是Enemy
            if (attackerLayer == LayerMask.NameToLayer("Enemy"))
            {
                var attackerCollider = attackerGobj.transform.Find("animator_top").GetComponent<Collider2D>();
                var attacker = EnemyManager.GetEnemyByCollider(attackerCollider);
                var weapon = attacker.availableWeapons[collision];
                PlayerManager.m_Role.OnAttacked += weapon.ATK;
                PlayerManager.m_Role.isAttacked = true;

                //var hitEffectPrefab = ResourcesLoader.LoadEffectPrefab("ef_hit01");
                ////Quaternion.identity 不旋转
                //var hitEffectGobj = Object.Instantiate(hitEffectPrefab, PlayerManager.m_Role.Transform.position, Quaternion.identity);
            }

            //当攻击者是 Player
            else if (attackerLayer == LayerMask.NameToLayer("Player"))
            {
                //从receiver出发找
                var enemyCollider = transform.gameObject.GetComponent<Collider2D>();
                var enemy = EnemyManager.GetEnemyByCollider(enemyCollider);
                if(enemy.IsCurrentHitOver)enemy.en_animator.SetTrigger("Damaged");
            }
            else { }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
       
    }
}
