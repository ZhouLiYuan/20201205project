using Role;
using UnityEngine;

public class GunEnemy : Enemy
{
    public override void Attack()
    {
        //GameObject obj = Instantiate(weapon, transform.position, transform.rotation);
        //Weapon.Projectile projectile = obj.GetComponent<Weapon.Projectile>();
        //projectile.direction = new Vector2(projectile.direction.x * currentFace, projectile.direction.y);
        //projectile.target = "Player";
    }

    public override void ChasePlayer()
    {

        Vector2 targetPosition = new Vector2(pl_Transform.position.x + attackRange, rg2d.position.y);
        Vector2 movementVector = Vector2.MoveTowards(rg2d.position, targetPosition, speed * Time.fixedDeltaTime);
        rg2d.MovePosition(movementVector);
    }
}

