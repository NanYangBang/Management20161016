using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace NYB.DeviceManagementSystem.Common
{
	internal static class ErrorCodeExtension
	{
		static Dictionary<ErrorCode, DescriptionAttribute> _descriptionMap = new Dictionary<ErrorCode, DescriptionAttribute>();

		public static DescriptionAttribute GetErrorCodeDescription(ErrorCode errorCode)
		{
			if (_descriptionMap.ContainsKey(errorCode)) {
				return _descriptionMap[errorCode];
			}

			FieldInfo provider = errorCode.GetType().GetField(errorCode.ToString());
			DescriptionAttribute[] attributes = (DescriptionAttribute[])provider.GetCustomAttributes(typeof(DescriptionAttribute), false);

			DescriptionAttribute errorCodeDescription = attributes[0];

			_descriptionMap[errorCode] = errorCodeDescription;

			return errorCodeDescription;
		}
	}
}
