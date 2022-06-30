using UnityEngine;

public class UIBase : MonoBehaviour
{
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public UIBase ShowChildUI(string uiName, bool visible)
    {
        return UIManager.Instance.ShowUI(transform, uiName, visible);
    }

    public UIBase ShowTopUI(string uiName, bool visible)
    {
        return UIManager.Instance.ShowTopUI(uiName, visible);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    public void AddTip(string tip)
    {
        UIManager.Instance.Tips.AddTipImpl(tip);
    }

    public void AddSmallTip(string tip)
    {
        UIManager.Instance.Tips.AddSmallTipImpl(tip);
    }
}
