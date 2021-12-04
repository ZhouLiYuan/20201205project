using App.Sound;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class SceneObject<T> where T : MonoBehaviour
{
    [Title("Universe Creator")]
    [ShowIf("@myObject != null")]
    [InlineEditor(InlineEditorObjectFieldModes.CompletelyHidden)]
    public T myObject;

    public void FindMyObject()
    {
        if (myObject == null)
            myObject = GameObject.FindObjectOfType<T>();
    }

    [ShowIf("@myObject != null")]
    [GUIColor(0.7f, 1f, 0.7f)]
    [ButtonGroup("Top Button", -1000)]//-1000确保置顶
    private void SelectSceneObject()
    {
        if (myObject != null)
            Selection.activeGameObject = myObject.gameObject;
    }

    //相关特性讲解https://zhuanlan.zhihu.com/p/408785062
    [ShowIf("@myObject == null")]  //从而确保是单例
    [Button]
    private void CreateManagerObject()
    {
        GameObject newManager = new GameObject();
        newManager.name = "新建单例 " + typeof(T).ToString();
        myObject = newManager.AddComponent<T>();
    }
}


//子类
//使用案例
public class SoundManagerRenderer : SceneObject<SoundManager>
{
    [ShowIf("@myObject != null")]
    [GUIColor(0.7f, 1f, 1f)]
    [ButtonGroup("Top Button")]
    private void SomeSoundManagerFunction1()
    {

    }

    [ShowIf("@myObject != null")]
    [GUIColor(0.7f, 0.7f, 1f)]
    [ButtonGroup("Top Button")]
    private void SomeSoundManagerFunction2()
    {

    }
}