using UnityEngine;

public class GameDriver : MonoBehaviour {

    void Awake()
    {
        GameCore.Instance.Initialize();
    }

    void Start()
    {
        UIManager.Instance.ShowTopUI("LoginUI", true);
        Application.runInBackground = true;
    }

    public void FixedUpdate()
    {
        var dt = Time.fixedDeltaTime;
        var dtMs = (int)(dt * 1000);
        GameCore.Instance.RunOneFrame(dtMs);
    }
}
