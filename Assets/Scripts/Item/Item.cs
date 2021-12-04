using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//收集要素的规划可以参考光遇

//货币 蜡烛 暴风蜡烛 活动蜡烛(飞行季) 心
//🕯= 100烛光
//心收集方式 1🕯x3 = ♥(限量) 2 留言（蜡烛x2）100👍 = 1♥  3朋友每日送 5👍

//中小循环(消耗:普通蜡烛和心 => 交友,购买皮肤)

//大循环 （消耗：暴风蜡烛 =>解锁 仙灵,玩家关系）
//暴风蜡烛构成了多周目跑图 收集翅膀的动力 
//翅膀依据玩家技术在 原罪献祭 可以兑换暴风蜡烛
//管理 1可收集翅膀Dic（key应该是位置） 2玩家拥有的翅膀List（栈式管理，会先失去最先收集的翅膀归还到原处）

//整体游戏进度推进（表情收集,交友,购买皮肤）(不可逆,非循环)

public class Item : Entity
{
   public ItemType m_itemType;
}

public enum ItemType 
{
    Money,
    Healer,
    Mask
}