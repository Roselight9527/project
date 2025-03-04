using UnityEngine;
using UnityEngine.UI;

public class CustomServer : Server
{
	[Header("UI 引用")]
	[SerializeField] private Button m_StartServerButton = null;    // 启动服务器的按钮
	[SerializeField] private Button m_SendToClientButton = null;  // 发送消息到客户端的按钮
	[SerializeField] private InputField m_SendToClientInputField = null; // 输入发送消息的文本框
	[SerializeField] private Button m_CloseServerButton = null;   // 关闭服务器按钮
	[SerializeField] private ScrollRect m_ServerLoggerScrollRect = null;  // 用于显示服务器日志的滚动视图

	private bool serverConnected = false;   // 服务器是否已连接
	private bool clientConnected = false;   // 客户端是否已连接
	private RectTransform m_ServerLoggerRectTransform = null;   // 服务器日志的RectTransform
	private Text m_ServerLoggerText = null;                      // 服务器日志的Text组件

	// 设置UI交互属性
	protected virtual void Awake()
	{
		// 启动服务器
		m_StartServerButton.interactable = true;  // 启用按钮，让用户可以启动服务器
		m_StartServerButton.onClick.AddListener(StartServer);

		// 发送消息到客户端
		m_SendToClientButton.interactable = false;  // 默认不可点击，直到客户端连接
		m_SendToClientButton.onClick.AddListener(SendMessageToClient);

		// 关闭服务器
		m_CloseServerButton.interactable = false;  // 默认不可点击，直到服务器启动
		m_CloseServerButton.onClick.AddListener(CloseServer);

		// 设置服务器委托
		OnClientConnected = () => { clientConnected = true; };  // 客户端连接时，更新状态
		OnClientDisconnected = () => { clientConnected = false; };  // 客户端断开连接时，更新状态
		OnServerClosed = () => { serverConnected = false; };  // 服务器关闭时，更新状态
		OnServerStarted = () => { serverConnected = true; };  // 服务器启动时，更新状态

		// 获取UI元素引用
		m_ServerLoggerRectTransform = m_ServerLoggerScrollRect.GetComponent<RectTransform>();
		m_ServerLoggerText = m_ServerLoggerScrollRect.content.gameObject.GetComponent<Text>();
	}

	// 更新UI交互属性
	protected override void Update()
	{
		base.Update();

		// 在Update中设置交互状态，因为这些方法可能会从非主线程调用
		m_StartServerButton.interactable = !serverConnected;  // 如果服务器没有启动，则启动按钮可用
		m_CloseServerButton.interactable = serverConnected;  // 如果服务器已启动，则关闭按钮可用
		m_SendToClientButton.interactable = clientConnected; // 如果客户端已连接，则发送按钮可用
	}

	// 获取输入框中的文本并发送到客户端
	private void SendMessageToClient()
	{
		string newMsg = m_SendToClientInputField.text;  // 获取输入框中的消息
		if (string.IsNullOrEmpty(newMsg))  // 如果消息为空
		{
			m_ServerLoggerText.text += $"\n- Enter message";  // 在日志中显示提示
			return;
		}
		base.SendMessageToClient(newMsg);  // 调用基类方法发送消息
	}

	// 自定义服务器日志
	#region 服务器日志
	// 带文本颜色的日志
	protected override void ServerLog(string msg)
	{
		base.ServerLog(msg);  // 调用基类日志方法
		m_ServerLoggerText.text += $"\n- {msg}";  // 将日志消息添加到显示区域

		// 确保滚动条始终显示最后一条消息
		LayoutRebuilder.ForceRebuildLayoutImmediate(m_ServerLoggerRectTransform);
		m_ServerLoggerScrollRect.verticalNormalizedPosition = 0f;  // 将滚动条位置置于最底部
	}

	// 带颜色的服务器日志
	protected override void ServerLog(string msg, Color color)
	{
		base.ServerLog(msg, color);  // 调用基类的带颜色日志方法
		m_ServerLoggerText.text += $"\n<color=#{ColorUtility.ToHtmlStringRGBA(color)}>- {msg}</color>";  // 以指定颜色显示日志

		// 确保滚动条始终显示最后一条消息
		LayoutRebuilder.ForceRebuildLayoutImmediate(m_ServerLoggerRectTransform);
		m_ServerLoggerScrollRect.verticalNormalizedPosition = 0f;  // 将滚动条位置置于最底部
	}
	#endregion
}
