using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NYB.DeviceManagementSystem.ViewModel
{
    public class User
    {
        private string _userName = string.Empty;

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        private string _pwd = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码必填")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Pwd
        {
            get { return _pwd; }
            set { _pwd = value; }
        }

        private string _logoName = string.Empty;

        /// <summary>
        /// 登录名
        /// </summary>
        [Required(ErrorMessage = "用户名必填")]
        [Display(Name = "用户名")]
        public string LogoName
        {
            get { return _logoName; }
            set { _logoName = value; }
        }

        private string _address = string.Empty;
        /// <summary>
        /// 地址
        /// </summary>
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        private string _telPhone = string.Empty;

        /// <summary>
        /// 联系电话
        /// </summary>
        public string TelPhone
        {
            get { return _telPhone; }
            set { _telPhone = value; }
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
        private string _email = string.Empty;
        /// <summary>
        /// 电子邮件
        /// </summary>
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

    }
}
