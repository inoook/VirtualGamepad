using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingTest : MonoBehaviour
{
    [SerializeField] PingSender netwokrPing = null;

    [SerializeField] string m_serverIp = "192.168.1.1";

    // Start is called before the first frame update
    void Start()
    {
        netwokrPing.PingToServer(m_serverIp, (latency) => {
            Debug.Log("latency: "+ latency);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [SerializeField] Rect drawRect = new Rect(10, 10, 100, 100);
    void OnGUI() {
        GUILayout.BeginArea(drawRect);
        List<string> addressList = PingSender.GetMyIpAddress();

        foreach (string address in addressList) {
            GUILayout.Label(address);
        }
        GUILayout.EndArea();
    }
}
