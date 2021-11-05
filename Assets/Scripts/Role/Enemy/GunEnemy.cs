using Role;

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
    }
}

