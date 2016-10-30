using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NYB.DeviceManagementSystem.Common
{
	/// <summary>
	/// 描述特性
	/// </summary>
	public class DescriptionAttribute : Attribute
	{
		public string Description { get; set; }

		public DescriptionAttribute(string description)
		{
			this.Description = description;
		}
	}
}
