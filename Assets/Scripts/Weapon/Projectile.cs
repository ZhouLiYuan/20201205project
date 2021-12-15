using Role;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 教程地址https://www.youtube.com/watch?v=wkKsl1Mfp5M&t=0s
/// 需要两个Collider，一个trigger用作触发受击对象DamageReceiver的OnTriggerEnter2D，一个用于自身的OnCollisionEnter2D
/// </summary>
public class Projectile : MonoBehaviour
{
    //bullet和枪是配套的
    //自身属性
    public float speed;
    public float bulletLifeTime;
    public float maxLifeTime;

    public Rigidbody2D rb2d;
    //private Collider2D TriggerCollider;
    private Collider2D hitCollider;

    public RoleEntity weaponOwner;
    public BaseWeapon projectileOwner;


    private void OnEnable()
    {
        speed = 35f;//先统一子弹速度（后期可以写个表格或者Switch语句根据枪类型变化而变化）其余伤害数值统一从Gun获得
        bulletLifeTime = 0f;
        maxLifeTime = 1f;
        hitCollider = gameObject.GetComponent<CapsuleCollider2D>();
        //TriggerCollider = gameObject.GetComponent<CircleCollider2D>();
        //TriggerCollider.isTrigger = true;
        rb2d = gameObject.GetComponent<Rigidbody2D>();//由于有rigidbody，一定要小心会和自身其他collider碰撞
        //var index = LayerMask.NameToLayer(LayerManager.Attacker);
        //gameObject.layer = 1 << index;
        gameObject.tag = weaponOwner.GameObject.tag;

        //方向设置
        var dir = new Vector3(weaponOwner.Transform.localScale.x, 0, 0);
        Vector3 bulletFaceDir = new Vector3(transform.localScale.x * dir.x, transform.localScale.y, transform.localScale.z);
        transform.localScale = bulletFaceDir;//设置子弹本身的反转（显示）
        rb2d.velocity = dir * speed;//子弹速度方向
        if (gameObject.name.Contains("bullet")) rb2d.gravityScale = 0f;//子弹无重力

    }




    private void OnCollisionEnter2D(Collision2D other)
    {
        var targetName = other.gameObject.name;

        //过滤器
        if (weaponOwner.UniqueName == targetName || projectileOwner.UniqueName == targetName) return;//不能自己打自己
        if (!targetName.Contains("Enemy") && !targetName.Contains("Platform") && !targetName.Contains($"{PlayerManager.p1_Role.Name}")) return;//暂定只能打中几样
        //if (weaponOwner.HitCollider == other.collider || projectileOwner.collider2D == other.collider) return;
        //hitCollider.isTrigger = true; ;//防止受击对象被撞飞
        GenerateEffect(other.GetContact(0).point);//在第一个接触点生成消亡特效
        Debug.Log($"飞行道具击中{targetName}");
        //hitCollider.isTrigger = true;   //太晚了，子弹已经进入受击对象碰撞体并出来了，无法触发双方的OnTriggerEnter2D，只能是分开两个collider来

        //foreach (ContactPoint2D Hit in other.contacts)
        //{
        //    Vector2 hitPoint = Hit.point;
        //    GenerateEffect(hitPoint);
        //}

        //销毁飞行道具本身
        Destroy(gameObject);
       //DestroyImmediate(gameObject);

    }

    ///// <summary>
    ///// 只有当两者碰撞时才会调用函数,hitTarget为被击中对象
    ///// </summary>
    ///// <param name="hitTarget"></param>
    //private void OnTriggerEnter2D(Collider2D hitTarget)
    //{
    //    if (weaponOwner.HitCollider == hitTarget || projectileOwner.collider2D == hitTarget) return;//不能自己打自己

    //}

    //只做计时器可不可以用mono的Update？
    private void Update()
    {
        bulletLifeTime += Time.deltaTime;
        if (bulletLifeTime >= maxLifeTime) Destroy(gameObject);
    }

    private void OnDestroy()
    {

    }

    private void GenerateEffect(Vector3 spawnPos)
    {
        //特效生成  
        var hitEffectPrefab = ResourcesLoader.LoadEffectPrefab("ef_hit01");
        var hitEffectGobj = Object.Instantiate(hitEffectPrefab, spawnPos, Quaternion.identity);
    }

}
