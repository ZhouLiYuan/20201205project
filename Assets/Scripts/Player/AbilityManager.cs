using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    //AbilitySelectCanvas
    [SerializeField] private Button a_button;

    private void OnEnable()
    {
        //这两个虚拟方法看得有点懵
        a_button.Select();
        a_button.OnSelect(null);
    }
}
