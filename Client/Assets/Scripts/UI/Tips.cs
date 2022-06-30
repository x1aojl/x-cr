using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Swift;
using Swift.Math;

public class Tips : UIBase {

    public GameObject Tip = null;
    public GameObject SmallTip = null;

    Dictionary<GameObject, float> tips = new Dictionary<GameObject, float>();

    ServerPort sp;

    void Start()
    {
        sp = GameCore.Instance.Get<ServerPort>();
        sp.OnMessage("Message", OnMessage);
    }

    void OnMessage(IReadableBuffer data)
    {
        var msg = data.ReadString();
        AddTip(msg);
    }

    public Vec2 WorldCenter = Vec2.Zero;
    public void AddTipImpl(string msg)
    {
        AddTip(msg, WorldCenter);
    }

    public void AddTip(string msg, Vec2 wp)
    {
        var tip = Instantiate(Tip) as GameObject;
        tip.gameObject.SetActive(true);
        tip.GetComponentInChildren<Text>().text = msg;
        tip.transform.SetParent(transform, false);

        var sp = UIManager.Instance.World2UI(wp);
        tip.GetComponent<RectTransform>().anchoredPosition = new Vector2((float)sp.x, (float)sp.y);

        tips[tip] = 0;
    }

    public void AddSmallTipImpl(string msg)
    {
        AddSmallTip(msg, WorldCenter);
    }

    public void AddSmallTip(string msg, Vec2 wp)
    {
        var tip = Instantiate(SmallTip) as GameObject;
        tip.gameObject.SetActive(true);
        tip.GetComponentInChildren<Text>().text = msg;
        tip.transform.SetParent(transform, false);

        var sp = UIManager.Instance.World2UI(wp);
        tip.GetComponent<RectTransform>().anchoredPosition = new Vector2((float)sp.x, (float)sp.y);

        tips[tip] = 0;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var tip in tips.Keys.ToArray())
        {
            var time = tips[tip];
            time += Time.deltaTime;
            if (time > 2)
            {
                tips.Remove(tip);
                Destroy(tip.gameObject);
            }
            else
                tips[tip] = time;
        }
    }
}
