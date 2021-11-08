using Role;
using Role.SelectableRole;
using System.Collections.Generic;
using UnityEngine;


//需要用subString或者split /n之类的来获取敌人Attacker种类名称

/// <summary>
/// 为场景中的所有对应名称的敌人加装逻辑层脚本(使用前提，场景中的敌人全都生成完毕)
/// 和其他现成LoadAsset的资源不同 Attacker是敌人Gobj身体某个子物体，是现成的需要获得（控制层级一致）
/// </summary>
public static class WeaponManager
{
    //可以先通过weapon找到owner，再通过owner找到 Dictionary<Collider2D, BaseWeapon>
    public static Dictionary<Enemy, Dictionary<Collider2D, BaseWeapon>> en_weaponsDic = new Dictionary<Enemy, Dictionary<Collider2D, BaseWeapon>>();

    private static GameObject SpawnWeaponGobj(RoleEntity role, string AssetName) 
    {
        //脚本中Transform是world的而不是local的
        //由于transform会按照世界坐标自动偏移抵消父级的移动量所以需要再加回来
        var equipPoint = new Vector3((float)0.2, (float)-0.25, 0) + role.Transform.position;//double=>float

        //暂时把武器装备点写死
        //3d向量转化为欧拉角
        var rotation = Quaternion.Euler(new Vector3(0, 0, 270));

        Transform parentTransform = role.Transform;

        var Prefab = ResourcesLoader.LoadWeaponPrefab(AssetName);
         var weaponGobj = UnityEngine.Object.Instantiate(Prefab, equipPoint, rotation, parentTransform);
        return weaponGobj;
    }

    private static BaseWeapon SpawnWeapon(RoleEntity role, WeaponConfig weaponConfig)
    {
        BaseWeapon weapon = new BaseWeapon();/*{ AtkType = (AtkType)weaponConfig.Type, AtkValue = weaponConfig.Damage };*/

        //有则动态赋值，无则生成后动态赋值
        var weaponGobj = role.Transform.Find($"{weaponConfig.AssetName}").gameObject;
        if (!weaponGobj)
        {weaponGobj = SpawnWeaponGobj(role, weaponConfig.AssetName);}
        //weaponGobj.layer = 1 << LayerMask.NameToLayer(LayerManager.Attacker);
        weapon.Init(weaponGobj);
        weapon.InitProperties(weaponConfig);
        weapon.SetOwner(role.GameObject);
        return weapon;
    }

    public static BaseWeapon SpawnEnemyWeapon(Enemy enemy,WeaponConfig weaponConfig)
    {
        BaseWeapon weapon = SpawnWeapon(enemy, weaponConfig);
        weapon.Transform.tag = TagManager.Enemy;
        //避免重复添加相同的key(并不是为了获得value)
        if (enemy.availableWeapons.TryGetValue(weapon.collider2D, out var baseweapon)) { }
        else
        {
            //enemy.availableWeapons[weapon.collider2D] = weapon;
            enemy.availableWeapons.Add(weapon.collider2D, weapon);
        }

      
        if (en_weaponsDic.TryGetValue(enemy, out var weapons)){ return weapon;}
        else 
        {
            en_weaponsDic.Add(enemy, enemy.availableWeapons);
            return weapon;
        }
    }

    public static BaseWeapon SpawnPlayerWeapon(PlayerRole role, WeaponConfig weaponConfig)
    {
        BaseWeapon newWeapon = SpawnWeapon(role, weaponConfig);
        newWeapon.Transform.tag = TagManager.Player;
        //避免重复添加相同元素
        foreach (var weapon in role.availableWeapons)
        {
            if (weapon.AssetName == newWeapon.AssetName) return weapon;
        }
        role.availableWeapons.Add(newWeapon);
        return newWeapon;
    }

    //private static List<GameObject> attackerGobjs = new List<GameObject>();
    //private static List<BaseWeapon> attackers = new List<BaseWeapon>();

    //private static int GobjIndex;

    //private static Type StringToType(GameObject attacker)
    //{
    //    string typeName = attacker.name.Substring(8);
    //    var AttackerType = Type.GetType(typeName);
    //    return AttackerType;
    //}

    //public static void AddAttackers() 
    //{
    //    if (attackers.Count == 0) return;

    //    for (GobjIndex = 0;GobjIndex < attackers.Count; GobjIndex++)
    //    {
    //        var AttackeType = StringToType(attackerGobjs[GobjIndex]);

    //        //或者到这里采用一个switch语句的形式？
    //        var attackerInstance = AttackeType.GetMethod("InitAttackerScript").MakeGenericMethod(new Type[] {AttackeType});
    //        //attackers.Add(attackerInstance);
    //    }
    //}

    //public static TAttacker InitWeapon<TAttacker>() where TAttacker : BaseWeapon, new()
    //{
    //    TAttacker attacker = new TAttacker();
    //    attacker.Init(attackerGobjs[GobjIndex]);
    //    return attacker;
    //}
}

