using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//区分：attacker是武器 持有者    weapon是武器
//一次只能收到一个对象的攻击
//需要和 受击框collider2d 在一个层级
public class DamageReceiver : MonoBehaviour
{
    //受动者(脚本拥有者) 这两个信息要不要也放在DamageData里面
    public Collider2D ownerCollider;
    public LayerMask ownerLayer;

    private BaseWeapon attackerWeapon;
    //根据武器 攻击类型 或者角色 盔甲类型生成不同种类（倾向于前者）

    private void OnEnable()
    {
        //根据BaseDamageReceiver所在层级换API获取
        ownerCollider = transform.GetComponent<Collider2D>();
        ownerLayer = 1 << gameObject.layer;
    }

    //配合Edit->Project Setting ->physics设置可以相互碰撞的图层
    //只有Attacker和attackble可以相互碰撞
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(TagManager.Player)  && !collision.gameObject.CompareTag(TagManager.Enemy)) return;
        if (collision.gameObject.layer != LayerMask.NameToLayer("Attacker")) return;
        //获取武器
        GameObject weaponGobj = collision.gameObject;
        LayerMask colliderLayer = weaponGobj.layer;

        var attackerGobj = weaponGobj.transform.parent.gameObject;
        LayerMask attackerLayer = attackerGobj.layer;

        //碰撞后只要父级名称不同，就可以相互伤害(父级的图层并不是同一层才能受伤可以避免敌人友军伤害)
        if ((attackerLayer != gameObject.layer) && attackerGobj.name!= gameObject.name)
        {
            var spawnPos = (collision.transform.position + transform.position) / 2;//希望特效更靠近受击方
            spawnPos.y = transform.position.y;

            //当攻击者是Enemy
            if (attackerLayer == LayerMask.NameToLayer("Enemy"))
            {
                var attackerCollider = attackerGobj.transform.GetComponent<Collider2D>();
                var attacker = EnemyManager.GetInstanceByCollider(attackerCollider);
                attackerWeapon = attacker.availableWeapons[collision];
                PlayerManager.m_Role.OnAttacked += attackerWeapon.ATK;
                PlayerManager.m_Role.isAttacked = true;
                if(PlayerManager.m_Role.InvincibleTime == 0) GenerateEffect(spawnPos);
            }

            //当攻击者是 Player
            else if (attackerLayer == LayerMask.NameToLayer("Player"))
            {
                //从receiver出发找
                var enemyCollider = transform.gameObject.GetComponent<Collider2D>();
                var enemy = EnemyManager.GetInstanceByCollider(enemyCollider);
                //if (enemy.IsCurrentHitOver) enemy.animator.SetTrigger("Damaged");
                if (enemy.IsCurrentHitOver == false) return;
                //避免在同一次攻击中发生连续碰撞
                enemy.animator.SetTrigger("Damaged");
                GenerateEffect(spawnPos);
                enemy.IsCurrentHitOver = false;
            }
            else { return; }

            //Debug.Log($"受击方{ownerGobj.name} 所在图层{LayerMask.LayerToName(ownerLayer)}  攻击碰撞体{collision.gameObject.name} 攻击方{attackerGobj.name}所在图层{LayerMask.LayerToName(attackerLayer)}");
          
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(TagManager.Player) && !collision.gameObject.CompareTag(TagManager.Enemy)) return;
        if (collision.gameObject.layer != LayerMask.NameToLayer("Attacker")) return;


        if (gameObject.tag == TagManager.Player)
        {
            PlayerManager.m_Role.OnAttacked -= attackerWeapon.ATK;
            //这里不知道为啥没调用到，导致角色一直处于可以被伤害的状态
            PlayerManager.m_Role.isAttacked = false;
        }
        else if (gameObject.tag == TagManager.Enemy) 
        {
            var enemyCollider = transform.gameObject.GetComponent<Collider2D>();
            var enemy = EnemyManager.GetInstanceByCollider(enemyCollider);
            enemy.IsCurrentHitOver = true;
        }

    }

    private void GenerateEffect(Vector3 spawnPos) 
    {
        //特效生成  
        var hitEffectPrefab = ResourcesLoader.LoadEffectPrefab("ef_hit01");
        var hitEffectGobj = Object.Instantiate(hitEffectPrefab, spawnPos, Quaternion.identity);
    }

    //private void OnDestroy()
    //{
    //    PlayerManager.m_Role.OnAttacked -= attackerWeapon.ATK;
    //    //PlayerManager.m_Role.isAttacked = false;
    //}
}
