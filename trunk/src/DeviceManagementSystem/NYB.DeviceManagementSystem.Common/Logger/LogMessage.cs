using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NYB.DeviceManagementSystem.Common.Logger
{
	public class LogMessage
	{
		public LogMessage(string userName, int functionID, string message)
		{
			this._userName = userName;
			this.FunctionID = functionID;
			this._message = message;
		}

		private string _userName;
		private string _message;

		public int FunctionID { get; set; }
		public string UserName { get { return _userName; } set { _userName = value; } }
		public string Message { get { return _message; } set { _message = value; } }
	}
}
