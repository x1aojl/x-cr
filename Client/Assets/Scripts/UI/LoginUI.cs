using UnityEngine;
using UnityEngine.UI;
using Swift;
using SpaceX;

public class LoginUI : UIBase
{
    public InputField ServerAddressInputField;
    public InputField AccountInputField;
    public InputField PasswordInputField;
    public Text       TipText;

    void Start()
    {
        var serverAddress = PlayerPrefs.GetString("ServerAddress", "127.0.0.1");
        var account = PlayerPrefs.GetString("Account", "");
        var password = PlayerPrefs.GetString("Password", "");
        ServerAddressInputField.text = serverAddress;
        AccountInputField.text = account;
        PasswordInputField.text = password;
    }

    public override void Show()
    {
        base.Show();
        SetTips("");
    }

    // 执行登录操作
    public void OnLogin()
    {
        var serverAddress = ServerAddressInputField.text;
        var account = AccountInputField.text;
        var password = PasswordInputField.text;

        if (account == null || account.Trim().Length == 0)
        {
            SetTips("用户名不能为空");
            return;
        }

        // 连接服务器
        SetTips("连接服务器 ...");
        GameCore.Instance.Get<NetCore>().Connect2Peer(serverAddress, 9001, (conn, reason) =>
        {
            if (conn == null)
                SetTips(reason);
            else
            {
                GameCore.Instance.ServerConnection = conn;

                // 登录
                SetTips("请求登录 ...");
                var buff = conn.Request2Srv("Login", (data) =>
                {
                    var ok = data.ReadBool();
                    if (ok)
                    {
                        PlayerPrefs.SetString("ServerAddress", serverAddress);
                        PlayerPrefs.SetString("Account", account);
                        PlayerPrefs.SetString("Password", password);

                        SetTips("登录成功");
                        GameCore.Instance.MeID = account;
                        GameCore.Instance.MeInfo = data.Read<UserInfo>();
                        Hide();
                    }
                    else
                    {
                        SetTips("登录失败");
                        conn.Close();
                    }
                    
                }, (conntected) =>
                {
                    SetTips("登录超时");
                    if (conntected)
                        conn.Close();
                });

                buff.Write(account);
                buff.Write(password);
                conn.End(buff);
            }
        });
    }

    void SetTips(string tips)
    {
        TipText.text = tips;
    }
}
