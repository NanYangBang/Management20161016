using NYB.DeviceManagementSystem.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NYB.DeviceManagementSystem.ViewModel
{
    public class WebDevice : ViewModelBase
    {
        public string ID { get; set; }

        [DisplayName("编号")]
        [Required]
        [StringLength(30, ErrorMessage = "最大长度为30")]
        public string Num { get; set; }

        [DisplayName("名称")]
        [Required]
        [StringLength(50, ErrorMessage = "最大长度为50")]
        public string Name { get; set; }

        [DisplayName("备注")]
        [StringLength(1000, ErrorMessage = "最大长度为1000")]
        public string Note { get; set; }

        [DisplayName("设备类型")]
        public string DeviceTypeID { get; set; }
        [DisplayName("生产厂商")]
        public string ManufacturerID { get; set; }

        [DisplayName("供应商")]
        public string SupplierID { get; set; }

        [DisplayName("生产日期")]
        public Nullable<System.DateTime> ProductDate { get; set; }

        [DisplayName("保养日期")]
        public Nullable<System.DateTime> MaintainDate { get; set; }

        [DisplayName("设备类型名称")]
        public string DeviceTypeName { get; set; }

        [DisplayName("生产厂商名称")]
        public string ManufacturerName { get; set; }

        [DisplayName("供应商名称")]
        public string SupplierName { get; set; }

        [DisplayName("设备状态")]
        public DeviceStateEnum DeviceState { get; set; }

    }
}
