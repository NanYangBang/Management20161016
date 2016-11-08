using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NYB.DeviceManagementSystem.ViewModel
{
    public class WebProject : ViewModelBase
    {
        private string _id;

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }


        private string _name = string.Empty;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _note = string.Empty;

        public string Note
        {
            get { return _note; }
            set { _note = value; }
        }

        private string _logoName = string.Empty;

        /// <summary>
        /// 登录名
        /// </summary>
        [Required(ErrorMessage = "登录名必填")]
        [Display(Name = "登录名")]
        public string LogoName
        {
            get { return _logoName; }
            set { _logoName = value; }
        }

        private string _pwd = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码必填")]
        [Display(Name = "密码")]
        public string Pwd
        {
            get { return _pwd; }
            set { _pwd = value; }
        }


    }
}
