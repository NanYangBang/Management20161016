using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NYB.DeviceManagementSystem.ViewModel
{
    public class WebDeviceType : ViewModelBase
    {
        public string ID { get; set; }

        [DisplayName("设备名称")]
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [DisplayName("备注")]
        public string Note { get; set; }
    }
}
