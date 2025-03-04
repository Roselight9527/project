using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Server : MonoBehaviour
{
	#region 公共变量
	[Header("网络设置")]
	public string ipAdress = "127.0.0.1";  // 服务器IP地址
	public int port = 54010;               // 服务器端口号
	[Min(0)] public float waitingMessagesFrequency = 1f;  // 等待消息的频率
	[Tooltip("0表示禁用超时")] 
	[Min(0)] public float receivingTimeOut = 0f;  // 接收消息超时时间（单位秒）
	#endregion

	#region 私有变量
	private TcpListener m_Server = null;      // TcpListener对象，用于监听客户端连接
	private TcpClient m_Client = null;         // 当前连接的客户端
	private NetworkStream m_NetStream = null;  // 网络流，用于传输数据
	private byte[] m_Buffer = new byte[49152]; // 缓存数据
	private int m_BytesReceived = 0;           // 已接收的字节数
	private float m_EllapsedTime = 0f;         // 已经过的时间
	private bool m_TimeOutReached = false;     // 是否超时
	[SerializeField] [TextArea] private string m_ReceivedMessage = "";  // 接收到的消息
	protected string ReceivedMessage
	{
		get { return m_ReceivedMessage; }
		set
		{
			m_ReceivedMessage = value;
			OnRecivedMessageChange();
		}
	}
	private IEnumerator m_ListenClientMsgsCoroutine = null;  // 客户端消息监听协程

	#endregion

	#region 委托变量
	protected Action OnServerStarted = null;        // 服务器启动时触发的委托
	protected Action OnServerClosed = null;         // 服务器关闭时触发的委托
	protected Action OnClientConnected = null;      // 客户端连接时触发的委托
	protected Action OnClientDisconnected = null;   // 客户端断开时触发的委托
	protected Action OnUnsuccessfulStart = null;    // 服务器启动失败时触发的委托
	#endregion

	protected virtual void OnRecivedMessageChange() { }

	// 启动服务器并等待客户端连接
	protected virtual void StartServer()
	{
		try
		{
			// 设置并启用服务器
			IPAddress ip = IPAddress.Parse(ipAdress);  // 解析IP地址
			m_Server = new TcpListener(ip, port);      // 创建TcpListener对象
			m_Server.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);  // 允许地址重用
			m_Server.Start();  // 启动服务器
			ServerLog($"服务器启动 {ipAdress}::{port}", Color.green);  // 输出日志
			// 开始异步等待客户端连接
			m_Server.BeginAcceptTcpClient(ClientConnected, null);
			OnServerStarted?.Invoke();  // 触发服务器启动委托
		}
		catch (Exception)
		{
			OnUnsuccessfulStart?.Invoke();  // 触发启动失败委托
			ServerLog("异常: 端口或IP不正确", Color.red);  // 输出错误日志
		}
	}

	// 更新函数，检查是否有客户端连接
	protected virtual void Update()
	{
		// 如果有客户端连接且没有开始监听客户端消息
		if (m_Client != null && m_ListenClientMsgsCoroutine == null)
		{
			// 启动监听客户端消息的协程
			m_ListenClientMsgsCoroutine = ListenClientMessages();
			StartCoroutine(m_ListenClientMsgsCoroutine);
		}
	}

	// 当"BeginAcceptTcpClient"检测到新客户端连接时调用的回调
	private void ClientConnected(IAsyncResult res)
	{
		// 设置客户端引用
		m_Client = m_Server.EndAcceptTcpClient(res);
		OnClientConnected?.Invoke();  // 触发客户端连接委托
	}

	#region 服务器与客户端通信
	// 协程，等待客户端消息，当客户端连接时监听消息
	private IEnumerator ListenClientMessages()
	{
		// 重置接收数据
		m_BytesReceived = 0;
		m_Buffer = new byte[49152];

		// 设置客户端的网络流信息
		m_NetStream = m_Client.GetStream();
		m_EllapsedTime = 0f;
		m_TimeOutReached = false;

		// 在客户端连接的情况下，持续等待消息
		do
		{
			ServerLog("服务器正在监听客户端消息...", Color.white);  // 输出日志
			// 开始异步读取客户端数据，并通过MessageReceived函数处理数据
			m_NetStream.BeginRead(m_Buffer, 0, m_Buffer.Length, MessageReceived, m_NetStream);

			// 如果有消息被接收，处理它
			if (m_BytesReceived > 0)
			{
				OnMessageReceived(m_ReceivedMessage);  // 处理接收到的消息
				m_BytesReceived = 0;
			}

			yield return new WaitForSeconds(waitingMessagesFrequency);  // 等待指定时间后再继续
			// 检查是否超时
			m_EllapsedTime += waitingMessagesFrequency;
			if (m_EllapsedTime >= receivingTimeOut && receivingTimeOut != 0)
			{
				ServerLog("接收消息超时", Color.red);  // 超时日志
				ServerLog("切记关闭客户端", Color.black);  // 提示关闭客户端
				CloseClientConnection();  // 关闭客户端连接
				m_TimeOutReached = true;
			}
		} while ((m_BytesReceived >= 0 && m_NetStream != null && m_Client != null) && (!m_TimeOutReached));
		// 通信结束
	}

	// 处理接收到的消息
	protected virtual void OnMessageReceived(string receivedMessage)
	{
		ServerLog($"服务器接受到: <b>{receivedMessage}</b>", Color.green);
		switch (receivedMessage)
		{
			case "Close":
				// 关闭客户端连接
				CloseClientConnection();
				break;
			default:
				ServerLog($"服务器接受到消息 <b>{receivedMessage}</b>", Color.red);  // 处理其他消息
				break;
		}
	}

	// 向客户端发送自定义消息
	protected void SendMessageToClient(string messageToSend)
	{
		// 如果没有连接客户端，提前退出
		if (m_NetStream == null)
		{
			ServerLog("Socket错误: 至少启动一个客户端", Color.red);  // 输出错误日志
			return;
		}

		// 构建要发送的消息
		byte[] encodedMessage = Encoding.UTF8.GetBytes(messageToSend);  // 将消息编码为字节

		// 开始同步写入消息
		m_NetStream.Write(encodedMessage, 0, encodedMessage.Length);
		ServerLog($"已发送给客户端: <b>{messageToSend}</b>", Color.yellow);  // 输出日志
	}

	// 当"BeginRead"结束时的异步回调函数，等待客户端的消息响应
	private void MessageReceived(IAsyncResult result)
	{
		if (result.IsCompleted && m_Client.Connected)
		{
			// 构建从客户端接收到的消息
			m_BytesReceived = m_NetStream.EndRead(result);  // 结束异步读取
			ReceivedMessage = Encoding.UTF8.GetString(m_Buffer, 0, m_BytesReceived);  // 解码消息为字符串
		}
	}
	#endregion

	#region 关闭服务器和客户端连接
	// 关闭服务器和客户端连接
	protected virtual void CloseServer()
	{
		ServerLog("服务器关闭", Color.red);  // 输出日志
		// 关闭客户端连接
		if (m_Client != null)
		{
			ServerLog("切记关闭客户端！", Color.blue);  // 提醒关闭客户端
			m_NetStream?.Close();  // 关闭网络流
			m_NetStream = null;
			m_Client.Close();  // 关闭客户端连接
			m_Client = null;
			OnClientDisconnected?.Invoke();  // 触发客户端断开委托
		}
		// 关闭服务器
		if (m_Server != null)
		{
			m_Server.Stop();  // 停止服务器
			m_Server = null;
		}

		if (m_ListenClientMsgsCoroutine != null)
		{
			StopCoroutine(m_ListenClientMsgsCoroutine);  // 停止协程
			m_ListenClientMsgsCoroutine = null;
		}

		OnServerClosed?.Invoke();  // 触发服务器关闭委托
	}

	// 关闭与客户端的连接
	protected virtual void CloseClientConnection()
	{
		ServerLog("关闭与客户端的连接", Color.red);  // 输出日志
		// 重置默认值
		if (m_ListenClientMsgsCoroutine != null)
		{
			StopCoroutine(m_ListenClientMsgsCoroutine);  // 停止协程
			m_ListenClientMsgsCoroutine = null;
		}

		m_Client.Close();  // 关闭客户端连接
		m_Client = null;

		OnClientDisconnected?.Invoke();  // 触发客户端断开委托

		// 等待接受新的客户端连接
		m_Server.BeginAcceptTcpClient(ClientConnected, null);
	}
	#endregion

	#region 服务器日志
	protected virtual void ServerLog(string msg, Color color)
	{
		Debug.Log($"<b>Server:</b> {msg}");
	}
	protected virtual void ServerLog(string msg)
	{
		Debug.Log($"<b>Server:</b> {msg}");
	}
	#endregion

}
