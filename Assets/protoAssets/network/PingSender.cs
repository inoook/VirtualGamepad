using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class PingSender : MonoBehaviour
{
    public delegate void PingHandler(int latency);
    public event PingHandler eventPing;

    Coroutine _checkPinCoroutine = null;

    public void PingToServer(string ip, System.Action<int> onCompleteAct = null) {
        this.eventPing += (int latency) => {
            Debug.LogWarning("ping: "+latency);
        };
        if (_checkPinCoroutine != null) {
            StopCoroutine(_checkPinCoroutine);
        }
        _checkPinCoroutine = StartCoroutine(_CheckPing(ip, onCompleteAct));
    }

    public IEnumerator _CheckPing(string ip, System.Action<int> onCompleteAct = null) {
        Debug.Log("Pint to: "+ip);
        Ping serverPing = new Ping(ip);

        while (true) {
            yield return null;

            if (serverPing.isDone) {
                int latency = serverPing.time;
                if (eventPing != null) {
                    eventPing(latency);
                }
                onCompleteAct(latency);
                break;
            }
        }
    }
    
    //public static List<IPAddress> GetIp(System.Net.Sockets.AddressFamily addressFamily = System.Net.Sockets.AddressFamily.InterNetwork) {
    //    string hostname = Dns.GetHostName();
    //    IPAddress[] adrList = Dns.GetHostAddresses(hostname);
    //    List<IPAddress> ipList = new List<IPAddress>();

    //    foreach (IPAddress address in adrList) {
    //        if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) {
    //            ipList.Add(address);
    //        }
    //    }
    //    return ipList;
    //}

    public static List<string> GetMyIpAddress() {

        List<string> myIpAddressList = new List<string>();
        string hostname = Dns.GetHostName();

        // ホスト名からIPアドレスを取得する
        IPHostEntry ipInfo = Dns.GetHostEntry(hostname);

        foreach (IPAddress address in ipInfo.AddressList) {
            //Debug.Log(">> ip: " + address.ToString());
            if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) {
                myIpAddressList.Add(address.ToString());
            }
        }
        return myIpAddressList;
    }
}
