using UnityEngine;
using UnityEngine.UI;

public class CustomClient : Client
{
	[Header("UI 引用")]
	[SerializeField] private Button m_StartClientButton = null;   // 启动客户端的按钮
	[SerializeField] private Button m_SendToServerButton = null; // 发送消息到服务器的按钮
	[SerializeField] private InputField m_SendToServerInputField = null;  // 输入发送消息的文本框
	[SerializeField] private Button m_SendCloseButton = null;    // 关闭连接按钮
	[SerializeField] private ScrollRect m_ClientLoggerScrollRect = null; // 用于显示客户端日志的滚动视图

	private RectTransform m_ClientLoggerRectTransform = null;    // 客户端日志的RectTransform
	private Text m_ClientLoggerText = null;                       // 客户端日志的Text组件

	// 设置UI交互属性
	private void Awake()
	{
		// 启动客户端按钮点击时，调用基类的StartClient方法启动客户端
		m_StartClientButton.onClick.AddListener(base.StartClient);

		// 发送消息到服务器按钮
		m_SendToServerButton.interactable = false;  // 默认不可点击
		m_SendToServerButton.onClick.AddListener(SendMessageToServer);

		// 发送关闭连接的按钮
		m_SendCloseButton.interactable = false;  // 默认不可点击
		m_SendCloseButton.onClick.AddListener(SendCloseToServer);

		// 设置客户端启动和关闭时的UI交互行为
		OnClientStarted = () =>
		{
			// 客户端启动时，更新UI交互状态
			m_StartClientButton.interactable = false;  // 启动按钮不可点击
			m_SendToServerButton.interactable = true;  // 发送消息按钮可点击
			m_SendCloseButton.interactable = true;     // 关闭连接按钮可点击
		};

		OnClientClosed = () =>
		{
			// 客户端关闭时，更新UI交互状态
			m_StartClientButton.interactable = true;   // 启动按钮可点击
			m_SendToServerButton.interactable = false; // 发送消息按钮不可点击
			m_SendCloseButton.interactable = false;    // 关闭连接按钮不可点击
		};

		// 获取UI元素引用
		m_ClientLoggerRectTransform = m_ClientLoggerScrollRect.GetComponent<RectTransform>();
		m_ClientLoggerText = m_ClientLoggerScrollRect.content.gameObject.GetComponent<Text>();
	}

	// 发送消息到服务器
	private void SendMessageToServer()
	{
		string newMsg = m_SendToServerInputField.text;  // 获取输入框中的文本
		if (string.IsNullOrEmpty(newMsg))  // 如果消息为空
		{
			m_ClientLoggerText.text += $"\n- Enter message";  // 在日志中显示提示
			return;
		}
		base.SendMessageToServer(newMsg);  // 调用基类的方法发送消息
	}

	// 发送关闭连接消息到服务器
	private void SendCloseToServer()
	{
		base.SendMessageToServer("Close");  // 发送关闭消息到服务器
		// 设置UI交互状态
		m_SendCloseButton.interactable = false;  // 关闭按钮不可点击
	}

	// 自定义客户端日志
	#region 客户端日志
	protected override void ClientLog(string msg)
	{
		base.ClientLog(msg);  // 调用基类日志方法
		m_ClientLoggerText.text += $"\n- {msg}";  // 将日志信息添加到日志显示区域

		// 确保滚动条始终显示最后一条消息
		LayoutRebuilder.ForceRebuildLayoutImmediate(m_ClientLoggerRectTransform);
		m_ClientLoggerScrollRect.verticalNormalizedPosition = 0f;  // 将滚动条位置置于最底部
	}

	// 带颜色的自定义客户端日志
	protected override void ClientLog(string msg, Color color)
	{
		base.ClientLog(msg, color);  // 调用基类的带颜色日志方法
		m_ClientLoggerText.text += $"\n<color=#{ColorUtility.ToHtmlStringRGBA(color)}>- {msg}</color>";  // 以指定颜色显示日志

		// 确保滚动条始终显示最后一条消息
		LayoutRebuilder.ForceRebuildLayoutImmediate(m_ClientLoggerRectTransform);
		m_ClientLoggerScrollRect.verticalNormalizedPosition = 0f;  // 将滚动条位置置于最底部
	}
	#endregion
}
