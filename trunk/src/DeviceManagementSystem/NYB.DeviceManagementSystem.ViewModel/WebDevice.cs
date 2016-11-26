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

        [DisplayName("名称")]
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public string Note { get; set; }

        public string DeviceTypeID { get; set; }
        public string ManufacturerID { get; set; }
        public string SupplierID { get; set; }

        public Nullable<System.DateTime> ProductDate { get; set; }
        public Nullable<System.DateTime> MaintainDate { get; set; }

        public string DeviceTypeName { get; set; }
        public string ManufacturerName { get; set; }
        public string SupplierName { get; set; }
    }
}
