using UnityEngine.UI;

public class OptionPanel : BasePanel
{
    //static readonly string path = "Assets/AssetBundles_sai/UI/Panel/OptionPanel.prefab";

    //因为每种Panel名不一样，传入泛型时，内部成员需要用实例来区分开来（所以就不能用静态成员了）
    public override string Path => "Assets/AssetBundles_sai/UI/Panel/OptionPanel.prefab";

    ///// <summary>
    ///// 创建UIInfo实例时，会调用其 有参构造函数（需要传入string类型 数据）
    ///// </summary>
    //public OptionPanel() : base(new UIInfo(path)) { }


    public override void OnOpen()
    {
        Init();
    }

    private void Init() 
    {
        Find<Button>("CloseButton").onClick.AddListener(()=>{ UIManager.ClosePanel(this); });
    }
}
