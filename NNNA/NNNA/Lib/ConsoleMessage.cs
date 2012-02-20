namespace NNNA
{
	class ConsoleMessage
	{
		private string m_message;
		public string Message
		{
			get { return m_message; }
			set { m_message = value; }
		}

		public ConsoleMessage(string message)
		{ m_message = message; }
	}
}
