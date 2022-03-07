//http://www.switchonthecode.com/tutorials/csharp-tutorial-simple-threaded-tcp-server
//
//fwsteal 
//08/04/2010 - 11:41

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;

public enum SEND_DATAMODE
{
    THREAD, QUEUE
}

public delegate void MessageReceivedHandler(TcpClient client, string message);

public class TCPServer : MonoBehaviour
{
    public class ConnectClient {
        public TcpClient client;
        public ClientStatus status;

        public ConnectClient(TcpClient client_, ClientStatus status_) {
            client = client_;
            status = status_;
        }
    }

    public class MsgData{
        public TcpClient client;
        public string msg;

        public MsgData(TcpClient client_, string msg_) {
            this.client = client_;
            this.msg = msg_;
        }
    }

    public static string GetSendModeStr(SEND_DATAMODE dataMode) {
        return ((int)dataMode).ToString();
    }
    
    //a simple threaded server that accepts connections and read data from clients.
    private TcpListener tcpListener; //wrapping up the underlying socket communication
    private Thread listenThread; //listening for client connections
    public int iPort = 11999; //server port

	public event MessageReceivedHandler MessageReceived;
    public event MessageReceivedHandler MessageReceivedQueue;
    
    public enum ClientStatus {
		Connect, Disconnect, DisconnectAfter, Error
    }
	public delegate void ClientStatusHandler(ClientStatus status, TcpClient client);
	public event ClientStatusHandler eventClientConnectStatus;

	public List<TcpClient> clients;

    private Queue messageQueue;
    private Queue statusQueue;

    public void Setup()
    {
        messageQueue = Queue.Synchronized(new Queue());
        statusQueue = Queue.Synchronized(new Queue());

        this.tcpListener = new TcpListener(IPAddress.Any, iPort);
        this.listenThread = new Thread(new ThreadStart(ListenForClients));
        this.listenThread.Start();
    }

	public void Close()
	{
		DeInit();
	}
    
	void DeInit()
	{
        if (this.clients != null) {
            foreach (TcpClient client in this.clients) {
                client.Close();
            }
            clients.Clear();
        }

        if (this.tcpListener != null){
            this.tcpListener.Stop();
            this.tcpListener = null;
        }

        if (this.listenThread != null) {
            this.listenThread.Abort();
            this.listenThread = null;
        }
    }

    private void ListenForClients()
    {
        this.tcpListener.Start(); //start tcplistener
		this.clients = new List<TcpClient>();
		//this.tcpListener.BeginAcceptTcpClient(AcceptTcpClientCallback, null);

        //sit in a loop accepting connections
        while (true)
        {
            try {
                //block until a client has connected to the server
                TcpClient client = this.tcpListener.AcceptTcpClient();

                // ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address; // ipAdress
                lock (clients) {
                    statusQueue.Enqueue(new ConnectClient(client, ClientStatus.Connect));
                    /*
                    if(eventClientConnectStatus != null){
                        eventClientConnectStatus(ClientStatus.Connect, client);
                    }
                    */
                    clients.Add(client);
                }

                //when connected - create a thread to handle communication with a connected client
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                //pass the tcpclient object returned by the accepttcpclient call to our new thread
                clientThread.Start(client);
            } catch (Exception ex) {
                Debug.LogWarning(ex);
                break;
            }
        }
    }

    private void HandleClientComm(object obj) {
        try {
            using (TcpClient client = (TcpClient)obj) {
                using (NetworkStream stream = client.GetStream()) {
                    NetworkStream clientStream = client.GetStream();

                    byte[] message = new byte[4096];
                    int bytesRead;

                    while (true) {
                        bytesRead = 0;

                        try {
                            //block until a client sends a message and is received
                            bytesRead = clientStream.Read(message, 0, 4096);
                        }
                        catch (System.Exception ex) {
                            //a socket error has occurred
                            Debug.LogWarning(ex);
                            break;
                        }

                        if (bytesRead == 0) {
                            //the client has disconnected from the server
                            //statusQueue.Enqueue(ClientStatus.Disconnect);
                            statusQueue.Enqueue(new ConnectClient(client, ClientStatus.Disconnect));

                            break;
                        }

                        //message has successfully been recieved
                        System.Text.Encoding encoder = Encoding.GetEncoding("utf-8");
                        string smessage = encoder.GetString(message, 0, bytesRead);

                        if (smessage.IndexOf(TCP.END_Code) > 0) {
                            smessage = smessage.Replace(TCP.END_Code, "");

                            Debug.Log("smessage: " + smessage);

                            // コマンドにデータの処理方法分岐
                            string cmd = smessage.Substring(0, 1);
                            smessage = smessage.Substring(1);

                            if (cmd == ((int)SEND_DATAMODE.THREAD).ToString()) {
                                if (this.MessageReceived != null) {
                                    this.MessageReceived(client, smessage); // dispatch
                                }
                            }
                            else {
                                // queueに追加
                                MsgData msgData = new MsgData(client, smessage);
                                messageQueue.Enqueue(msgData);
                            }
                        }
                    }

                    // close
                    client.Close();
                }
            }
        }
        catch (System.Exception ex) {
            Debug.LogWarning("error: " + ex);
        }
    }

	public int GetClientCount()
	{
		return clients.Count;
	}
    public int GetClientId(TcpClient client) {
        return clients.IndexOf(client);
    }
    public TcpClient GetClientById(int id) {
        return clients[id];
    }

    void OnDisable()
	{
		DeInit();
	}
	void OnApplicationQuit()
    {
		DeInit();
	}

	//
	public void Send(string msgStr){
		foreach (TcpClient client in this.clients)
		{
			Write(client, msgStr);
		}
	}
    public void Send(int i, string msgStr) {
        TcpClient client = this.clients[i];
        Write(client, msgStr);
    }
    public void Send(int[] ids, string msgStr) {
        for (int i = 0; i < ids.Length; i++) {
            int id = ids[i];
            TcpClient client = this.clients[id];
            Write(client, msgStr);
        }
    }
    public void Send(int[] ids, string[] msgStrs) {
        for (int i = 0; i < ids.Length; i++) {
            int id = ids[i];
            string msg = msgStrs[i];
            TcpClient client = this.clients[id];
            Write(client, msg);
        }
    }
    private void Write(TcpClient tcpClient, string data)
	{
        //System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();
        System.Text.Encoding encoder = Encoding.GetEncoding("utf-8");
        byte[] bytes = encoder.GetBytes(data + TCP.END_Code);
		Write(tcpClient, bytes);
	}
	private void Write(TcpClient tcpClient, byte[] bytes)
	{
        if (tcpClient.Client.Connected) {
            NetworkStream networkStream = tcpClient.GetStream();
            networkStream.BeginWrite(bytes, 0, bytes.Length, WriteCallback, tcpClient);
        }
	}
	private void WriteCallback(IAsyncResult result)
	{
		TcpClient tcpClient = result.AsyncState as TcpClient;
		NetworkStream networkStream = tcpClient.GetStream();
		networkStream.EndWrite(result);
	}

	//
	void Update()
	{
        lock (messageQueue.SyncRoot) {
            if (messageQueue.Count > 0) {

                if (this.MessageReceivedQueue != null) {
                    MsgData msgData = (MsgData)(messageQueue.Dequeue());
                    this.MessageReceivedQueue(msgData.client, msgData.msg);
                }
            }
        }

        lock (statusQueue.SyncRoot){
			if(statusQueue.Count > 0){
				ConnectClient connectClient = (ConnectClient)(statusQueue.Dequeue());
				ClientStatus status = connectClient.status;
				TcpClient tcpClient = connectClient.client;
				//ClientStatus status = (ClientStatus)(statusQueue.Dequeue());
				
				if(eventClientConnectStatus != null){
					if(status == ClientStatus.Connect){
						eventClientConnectStatus(ClientStatus.Connect, tcpClient);
					}
					if(status == ClientStatus.Disconnect){
						eventClientConnectStatus(ClientStatus.Disconnect, tcpClient);
                        this.clients.Remove(tcpClient);

                        eventClientConnectStatus(ClientStatus.DisconnectAfter, null);
                    }
                }
			}
		}
	}

    public int GetQueueCount() {
        return messageQueue.Count;
    }
}

