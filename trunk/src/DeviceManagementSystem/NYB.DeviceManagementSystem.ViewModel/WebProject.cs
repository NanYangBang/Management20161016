using System;
using System.Collections.Generic;
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

    }
}
