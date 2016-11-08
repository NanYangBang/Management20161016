using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NYB.DeviceManagementSystem.ViewModel
{
    public class WebProject
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

        public string Address { get; set; }
        public string Contact { get; set; }
        public string Phone { get; set; }

        public string Note
        {
            get { return _note; }
            set { _note = value; }
        }

        private string _adminLoginName = string.Empty;

        public string AdminLoginName
        {
            get { return _adminLoginName; }
            set { _adminLoginName = value; }
        }

        public string Password { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateUserID { get; set; }
    }
}
