using System;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Client : MonoBehaviour
{
	#region 公共变量
	[Header("网络设置")]
	public string ipAdress = "127.0.0.1"; // 服务器的 IP 地址
	public int port = 54010; // 服务器的端口
	public float waitingMessagesFrequency = 1; // 每隔多久检查一次消息
	#endregion

	#region 私有变量
	private TcpClient m_Client; // 客户端实例
	private NetworkStream m_NetStream = null; // 网络流，用于发送和接收数据
	private byte[] m_Buffer = new byte[49152]; // 接收数据的缓冲区
	private int m_BytesReceived = 0; // 已接收到的字节数
	private string m_ReceivedMessage = ""; // 接收到的消息
	private IEnumerator m_ListenServerMsgsCoroutine = null; // 监听服务器消息的协程

	[Tooltip("此值应该大于或等于服务器的消息等待时间频率")]
	[Min(0)] private float m_DelayedCloseTime = 0.5f; // 延迟关闭客户端的时间
	#endregion

	#region 委托变量
	protected Action OnClientStarted = null;    // 客户端启动时触发的委托
	protected Action OnClientClosed = null;     // 客户端关闭时触发的委托
	#endregion

	// 启动客户端并建立与服务器的连接
	protected void StartClient()
	{
		// 如果客户端已经存在，直接返回
		if (m_Client != null)
		{
			ClientLog($"当前客户端已连接到 {ipAdress}::{port}", Color.red);
			return;
		}

		try
		{
			// 创建新的客户端实例
			m_Client = new TcpClient();
			// 连接到指定的服务器和端口
			m_Client.Connect(ipAdress, port);
			ClientLog($"客户端已启动，连接到 {ipAdress}::{port}", Color.green);
			OnClientStarted?.Invoke(); // 触发客户端启动委托
			m_ListenServerMsgsCoroutine = ListenServerMessages(); // 启动监听服务器消息的协程
			StartCoroutine(m_ListenServerMsgsCoroutine);
		}
		catch (SocketException)
		{
			ClientLog("Socket 异常：请先启动服务器", Color.red);
			CloseClient(); // 连接失败时关闭客户端
		}
	}

	#region 客户端与服务器的通信
	// 协程，等待并接收服务器消息
	private IEnumerator ListenServerMessages()
	{
		// 如果客户端未连接，直接退出
		if (!m_Client.Connected)
			yield break;

		// 获取客户端的网络流
		m_NetStream = m_Client.GetStream();

		// 持续异步读取服务器消息并在接收到消息时处理
		do
		{
			ClientLog("客户端正在监听服务器消息...", Color.white);
			// 异步读取服务器消息，并通过 MessageReceived 函数处理
			m_NetStream.BeginRead(m_Buffer, 0, m_Buffer.Length, MessageReceived, null);

			if (m_BytesReceived > 0)
			{
				OnMessageReceived(m_ReceivedMessage); // 处理接收到的消息
				m_BytesReceived = 0; // 重置已接收的字节数
			}

			yield return new WaitForSeconds(waitingMessagesFrequency); // 等待指定的时间

		} while (m_BytesReceived >= 0 && m_NetStream != null && m_Client != null);
		// 如果接收完毕，或客户端或网络流为 null，退出通信
	}

	// 处理接收到的消息
	protected virtual void OnMessageReceived(string receivedMessage)
	{
		ClientLog($"接收到的消息：<b>{receivedMessage}</b>", Color.green);
		switch (receivedMessage)
		{
			case "Close":
				CloseClient(); // 如果消息为 "Close"，关闭客户端连接
				break;
			default:
				ClientLog($"接收到消息 <b>{receivedMessage}</b>", Color.red);
				break;
		}
	}

	// 向服务器发送自定义消息
	protected virtual void SendMessageToServer(string messageToSend)
	{
		try
		{
			m_NetStream = m_Client.GetStream(); // 获取网络流
		}
		catch (Exception)
		{
			ClientLog("未连接到服务器，发生 Socket 异常", Color.red);
			CloseClient(); // 如果发生异常，关闭客户端
			return;
		}

		// 如果客户端未连接，返回错误
		if (!m_Client.Connected)
		{
			ClientLog("Socket 错误：请先建立与服务器的连接", Color.red);
			return;
		}

		// 将消息转换为字节并发送给服务器
		byte[] encodedMessage = Encoding.UTF8.GetBytes(messageToSend);

		// 同步写入消息
		m_NetStream.Write(encodedMessage, 0, encodedMessage.Length);
		ClientLog($"消息已发送到服务器：<b>{messageToSend}</b>", Color.yellow);

		// 如果消息是 "Close"，则关闭客户端
		if (messageToSend == "Close")
		{
			// 停止监听服务器消息
			StopCoroutine(m_ListenServerMsgsCoroutine);
			// 等待一定时间再关闭客户端，确保消息已经发送
			StartCoroutine(DelayedCloseClient(waitingMessagesFrequency + m_DelayedCloseTime));
		}
	}

	// 异步回调，处理 "BeginRead" 完成后的消息接收
	private void MessageReceived(IAsyncResult result)
	{
		if (result.IsCompleted && m_Client.Connected)
		{
			// 获取从服务器接收到的消息
			m_BytesReceived = m_NetStream.EndRead(result);
			m_ReceivedMessage = Encoding.UTF8.GetString(m_Buffer, 0, m_BytesReceived);
		}
	}
	#endregion

	#region 关闭客户端
	// 关闭客户端连接
	private void CloseClient()
	{
		ClientLog("客户端已关闭", Color.red);

		// 如果客户端仍然连接，关闭连接
		if (m_Client.Connected)
			m_Client.Close();

		// 重置客户端实例
		if (m_Client != null)
			m_Client = null;

		OnClientClosed?.Invoke(); // 触发客户端关闭委托
	}

	// 延迟关闭客户端
	private IEnumerator DelayedCloseClient(float delayedTime)
	{
		yield return new WaitForSeconds(delayedTime); // 等待指定的时间
		CloseClient(); // 关闭客户端
	}
	#endregion

	#region 客户端日志
	protected virtual void ClientLog(string msg, Color color)
	{
		Debug.Log($"<b>Client:</b> {msg}");
	}

	protected virtual void ClientLog(string msg)
	{
		Debug.Log($"<b>Client:</b> {msg}");
	}
	#endregion

}
