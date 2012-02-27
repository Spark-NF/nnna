namespace NNNA
{
	class ConsoleMessage
	{
		private string _message;
		public string Message
		{
			get { return _message; }
			set { _message = value; }
		}

		public ConsoleMessage(string message)
		{ _message = message; }
	}
}
