using UnityEngine;
using Swift;
using Swift.Math;

public class UIManager : MonoBehaviour {

    public Tips Tips;
    
    Canvas IndicateCanvas;
    Camera IndicateUICamera;

    Canvas Canvas;
    Camera UICamera;
    RectTransform UICavansRect;

    void Start()
    {
        GameCore.Instance.OnMainConnectionDisconnected += OnDisconnected;
    }

    private void OnDisconnected(Connection conn, string reason)
    {
        ClearScene();
        
        ShowTopUI("MainArea", false);
        ShowTopUI("MainMenu", false);
        ShowTopUI("LoginUI", true);

        Tips.AddTip("网络连接中断");
    }

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                var gd = FindObjectOfType<GameDriver>();
                var uiRootGo = gd.transform.Find("UIRoot").gameObject;
                instance = uiRootGo.AddComponent<UIManager>();
                instance.Tips = uiRootGo.GetComponentInChildren<Tips>();
                instance.Canvas = uiRootGo.GetComponent<Canvas>();
                instance.UICavansRect = instance.Canvas.GetComponent<RectTransform>();
                instance.UICamera = instance.Canvas.worldCamera;
            }

            return instance;
        }
    } static UIManager instance = null;

    public Vec2 World2UI(Vec2 wp) { return World2UI(new Vector3((float)wp.x, 0, (float)wp.y)); }
    public Vec2 World2UI(Vector3 wp)
    {
        var vp = UICamera.WorldToViewportPoint(wp);
        var sp = new Vec2(UICavansRect.rect.width * vp.x, UICavansRect.rect.height * vp.y);
        return sp;
    }

    // 显示/隐藏 UI
    public UIBase ShowUI(Transform parent, string uiName, bool visible)
    {
        var t = parent.Find(uiName);
        if (t == null)
            return null;

        var ui = t.GetComponent<UIBase>();
        if (visible)
            ui.Show();
        else
            ui.Hide();

        return ui;
    }

    // 显示/隐藏 UI
    public UIBase ShowTopUI(string uiName, bool visible)
    {
        return ShowUI(transform, uiName, visible);
    }

    // 清空场景
    public void ClearScene()
    {
    }
}
