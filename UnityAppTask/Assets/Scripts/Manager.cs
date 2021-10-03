using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField] public static Manager instance = null;
    [SerializeField] public SignalRListener listener;

    [SerializeField] public string urlHub;
    [SerializeField] public string urlGetBundle;
    [SerializeField] public string deviceIP;

    [SerializeField] private InputField serverIPInput;
    [SerializeField] private Text deviceIPLabel;

    [SerializeField] private GameObject panelSetting;

    [SerializeField] private Transform target;
    [SerializeField] public GameObject objectFromServer;

    [SerializeField] public bool animate;

    void Awake()
    {
        animate = false;
        Debug.Log(GetLocalIPAddress());
        deviceIP = GetLocalIPAddress();
    }

    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance == this)
            Destroy(gameObject);

        deviceIPLabel.text = "Device IP:" + GetLocalIPAddress();
    }



    //�������� �� ������������ ������� ��������, �������� ������� ���� � ������ �������� �� ���������, �� ����� ����������� ���������� � �����
    void Update()
    {
        if (animate){       
            animate = false;
            if(objectFromServer == null){
                Debug.Log("����� �� ����������");
                return;
            }
            else AnimTarget();
        }
    }



    //����� � �������� ��� �������� ������
    public void DownloadBundle()
    {
        try{
            if(!String.IsNullOrEmpty(urlGetBundle))
                StartCoroutine(LoadBundleFromServer());       
        }
        catch(Exception e){
            Debug.Log(e.Message);
        } 
    }

    private IEnumerator LoadBundleFromServer()
    {
        using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(urlGetBundle))
        {
            yield return uwr.SendWebRequest();
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(uwr.error);
            }
            else{
                AssetBundle bunlde = DownloadHandlerAssetBundle.GetContent(uwr);
                objectFromServer = Instantiate(bunlde.LoadAsset("Cube"),target) as GameObject;
            }
        }
    }

    //��������� url-�� ��� ������������� ���� ������� � ��������� ������
    public void SetServerIP()
    {
        if (!String.IsNullOrEmpty(serverIPInput.text))
        {
            string urlBase = serverIPInput.text.Trim();
            urlGetBundle = "http://"+ urlBase + "/home/getbundle";
            urlHub = "http://"+ urlBase + "/animation";

            listener.StartListening();
        }
    }

    //������� ��� ����
    public void ToggleSettingPanel()
    {
        panelSetting.SetActive(!panelSetting.activeSelf);
    }


    //��������� IP ������ � ��������� ���� �� ������ ���� 
    public static string GetLocalIPAddress()
    {
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());

        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return "Identification Error";
    }

    //������ ��� ���������/����������� ��� ����� �����
    public void OnTargetFound()
    {
        if (objectFromServer != null)
            objectFromServer.SetActive(true);
        else DownloadBundle();
    }

    public void OnTargetLost()
    {
        if (objectFromServer != null)
            objectFromServer.SetActive(false);
    }

    public void AnimTarget()
    {
        Debug.Log("Animate init");
        Transform targetTransform = Manager.instance.objectFromServer.transform;

        StartCoroutine(AnimCoroutine(targetTransform));
    }

    public IEnumerator AnimCoroutine(Transform target)
    {
        while (true)
        {
            target.Rotate(6f, 6f, 6f);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
