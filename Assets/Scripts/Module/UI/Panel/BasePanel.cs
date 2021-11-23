using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 用抽象类做通用 模版
/// </summary>
public class BasePanel : Entity
{
    //代替原本的UIInfo
    public virtual string Path { get; }

    public CanvasGroup canvasGroup;

    /// <summary>
    /// 用传入的参数(scene中的具体Gobj) 初始化抽象类字段
    /// </summary>
    /// <param name="name"></param>
    /// <param name="obj"></param>
    public override void Init(GameObject obj)
    {
        base.Init(obj);
        canvasGroup = obj.AddComponent<CanvasGroup>();
    }

    public virtual void OnOpen() { }
    public virtual void OnUpdate(float deltaTime) { }
    public virtual void OnPause()
    {
        UIManager.GetOrAddComponent<CanvasGroup>(this.GameObject).blocksRaycasts = false;
    }
    public virtual void OnResume()
    {
        UIManager.GetOrAddComponent<CanvasGroup>(this.GameObject).blocksRaycasts = true;
    }
    public virtual void OnClose()
    {
    }

    public void Close()
    {
        OnClose();
        UIManager.ClosePanel(this);
    }
    public void Show()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
    }
    public void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
    }
}



