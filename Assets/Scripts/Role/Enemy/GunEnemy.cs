using Role;
using UnityEngine;

public class GunEnemy : Enemy
{
    public override void Attack()
    {
        //Spawn Gun_bullet
        var bulletPrefab = ResourcesLoader.LoadProjectilePrefab($"{currentWeapon.AssetName}_bullet");//子弹名=枪名_bullet
        var bulletGo = Object.Instantiate(bulletPrefab, currentWeapon.Transform.position, Quaternion.identity);//不应以任何Gobj为父级
        var bullet = bulletGo.GetComponent<Projectile>();
        bullet.enabled = false;
        bullet.weaponOwner = this;
        bullet.projectileOwner = currentWeapon;
        bullet.enabled = true;//赋值完成后再调用OnEnable函数
    }

    public override void ChasePlayer()
    {
        Vector2 targetPosition = new Vector2(pl_Transform.position.x + attackRange, rg2d.position.y);
        Vector2 movementVector = Vector2.MoveTowards(rg2d.position, targetPosition, speed * Time.fixedDeltaTime);
        rg2d.MovePosition(movementVector);
    }
}

