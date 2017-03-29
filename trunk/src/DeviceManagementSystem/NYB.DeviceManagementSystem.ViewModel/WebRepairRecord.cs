using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NYB.DeviceManagementSystem.ViewModel
{
    public class WebRepairRecord : ViewModelBase
    {

        public WebRepairRecord()
        {
            this.Attachments = new List<WebAttachment>();
        }
        public string ID { get; set; }

        public string DeviceID { get; set; }

        [DisplayName("设备名称")]
        public string DeviceName { get; set; }

        [DisplayName("维修人")]
        [Required]
        [StringLength(30, ErrorMessage = "最大长度为30")]
        public string Operator { get; set; }

        [DisplayName("维修日期")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime RepairDate { get; set; }

        [DisplayName("备注")]
        [StringLength(2000, ErrorMessage = "最大长度为2000")]

        public string Note { get; set; }
        [DisplayName("故障现象分析")]
        [StringLength(2000, ErrorMessage = "最大长度为2000")]

        public string Describe { get; set; }
        [DisplayName("处理方案")]
        [StringLength(2000, ErrorMessage = "最大长度为2000")]
        public string Solution { get; set; }

        public List<WebAttachment> Attachments { get; set; }
    }
}
