using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NNNA
{
	class Console
	{
		public static List<ConsoleMessage> Messages = new List<ConsoleMessage>();

		public static List<ConsoleMessage> GetLast(int count = 10)
		{
			List<ConsoleMessage> ret = new List<ConsoleMessage>();
			int max = Messages.Count - count > 0 ? Messages.Count - count : 0;
			for (int i = Messages.Count - 1; i >= max; i--)
			{ ret.Add(Messages[i]); }
			return ret;
		}
	}
}
