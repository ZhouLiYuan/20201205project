using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    
    public static BaseWeapon SpawnEnemyWeapon(Enemy enemy,WeaponConfig weaponConfig)
    {
        BaseWeapon weapon = new BaseWeapon();/*{ AtkType = (AtkType)weaponConfig.Type, AtkValue = weaponConfig.Damage };*/

        //暂时把武器装备点写死
        //3d向量转化为欧拉角
        var rotationVector3 = new Vector3(0, 0, 270);
        var rotation = Quaternion.Euler(rotationVector3);
        //double=>float
        //这里的Transform是world的而不是local的
        //由于transform会按照世界坐标自动偏移抵消父级的移动量所以需要再加回来
        var equipPoint = new Vector3((float)0.2, (float)-0.25, 0) + enemy.Transform.position;
        Transform parentTransform = enemy.Find<Transform>("animator_top");

        //有则动态赋值，无则生成后动态赋值
        var weaponGobj = enemy.Find<Transform>("animator_top").Find($"{weaponConfig.AssetName}").gameObject;
        if (!weaponGobj) 
        {
            var Prefab = ResourcesLoader.LoadWeaponPrefab(weaponConfig.AssetName);
            weaponGobj = UnityEngine.Object.Instantiate(Prefab, equipPoint, rotation, parentTransform);
        }

        weaponGobj.transform.tag = TagManager.Enemy;
        weapon.Init(weaponGobj);
        weapon.InitProperties(weaponConfig);
        weapon.SetOwner(enemy.GameObject);
        enemy.availableWeapons[weapon.collider2D] = weapon;
        //避免重复添加相同的key(并不是为了获得value)
        if (en_weaponsDic.TryGetValue(enemy, out var weapons)){ return weapon;}
        else 
        {
            en_weaponsDic.Add(enemy, enemy.availableWeapons);
            return weapon;
        }
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

