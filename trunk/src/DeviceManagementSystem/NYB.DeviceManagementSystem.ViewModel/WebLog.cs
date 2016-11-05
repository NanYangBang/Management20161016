using NYB.DeviceManagementSystem.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NYB.DeviceManagementSystem.ViewModel
{
    public class WebLog
    {
        private string _loginName;

        public string LoginName
        {
            get { return _loginName; }
            set { _loginName = value; }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _content;

        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        private OperatTypeEnum _operatType;

        public OperatTypeEnum OperatType
        {
            get { return _operatType; }
            set { _operatType = value; }
        }

        public BusinessModelEnum BusinessModel { get; set; }

        public DateTime OperatDate { get; set; }
    }
}
