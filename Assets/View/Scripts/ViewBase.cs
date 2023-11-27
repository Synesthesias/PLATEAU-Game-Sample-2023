using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBase : MonoBehaviour
{
    public virtual IEnumerator Wait()
    {
        yield return null;
    }

    //��ʂ�j������
    public void DestroyView()
    {
        Debug.Log("Destroy");
        Destroy(this.gameObject);
    }

    /// ��ʂ𐶐�����
    /// <typeparam name="T">View�̌p��</typeparam>
    /// <param name="viewName">View��</param>
    public static T Instantiate<T>(string viewName) where T : ViewBase
    {
        var prefab = Resources.Load<T>("Prefabs/ViewPrefabs/"+viewName);
        prefab.name = viewName;
        var view = Instantiate(prefab);
        view.name = viewName;
        return view;
    }

}