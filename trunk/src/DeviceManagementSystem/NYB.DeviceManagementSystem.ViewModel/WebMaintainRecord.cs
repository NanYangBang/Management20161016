using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NYB.DeviceManagementSystem.ViewModel
{
    public class WebMaintainRecord : ViewModelBase
    {
        public WebMaintainRecord()
        {
            Attachments = new List<WebAttachment>();
        }

        public string ID { get; set; }

        public string DeviceID { get; set; }

        [DisplayName("设备名称")]
        public string DeviceName { get; set; }

        [DisplayName("保养人")]
        [StringLength(30, ErrorMessage = "最大长度为30")]
        [Required]
        public string Operator { get; set; }

        [DisplayName("保养日期")]
        [Required]
        public DateTime MaintainDate { get; set; }

        [DisplayName("备注")]
        [StringLength(2000, ErrorMessage = "最大长度为2000")]
        public string Note { get; set; }

        public List<WebAttachment> Attachments { get; set; }
    }
}
