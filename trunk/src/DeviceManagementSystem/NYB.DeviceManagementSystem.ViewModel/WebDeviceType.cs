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
        public WebDeviceType()
        {
            this.MaintainItems = new List<WebMaintainItem>();
        }

        public string ID { get; set; }

        [DisplayName("名称")]
        [Required]
        [StringLength(50, ErrorMessage = "最大长度为50")]
        public string Name { get; set; }

        [DisplayName("备注")]
        [StringLength(100, ErrorMessage = "最大长度为100")]
        public string Note { get; set; }

        public List<WebMaintainItem> MaintainItems { get; set; }
    }
}
