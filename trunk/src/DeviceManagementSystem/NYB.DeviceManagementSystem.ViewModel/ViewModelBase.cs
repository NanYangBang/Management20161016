using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NYB.DeviceManagementSystem.ViewModel
{
    public class ViewModelBase
    {
        public DateTime CreateDate { get; set; }

        private string _createUserID;
        
        /// <summary>
        /// 用于添加数据
        /// </summary>
        public string CreateUserID
        {
            get { return _createUserID; }
            set { _createUserID = value; }
        }

        private string _createUserName;

        /// <summary>
        /// 用于界面显示，用户姓名，不是登录名
        /// </summary>
        public string CreateUserName
        {
            get { return _createUserName; }
            set { _createUserName = value; }
        }


        private string _projectID = string.Empty;

        /// <summary>
        /// 项目所属
        /// </summary>
        public string ProjectID
        {
            get { return _projectID; }
            set { _projectID = value; }
        }
    }
}
