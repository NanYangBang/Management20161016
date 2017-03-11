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

        [DisplayName("名称")]
        [Required]
        [StringLength(30, ErrorMessage = "最大长度为30")]
        public string Name { get; set; }

        [DisplayName("备注")]
        [StringLength(100, ErrorMessage = "最大长度为100")]
        public string Note { get; set; }
    }
}
