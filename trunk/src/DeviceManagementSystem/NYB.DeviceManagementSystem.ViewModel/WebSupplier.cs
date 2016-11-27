using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NYB.DeviceManagementSystem.ViewModel
{
    public class WebSupplier : ViewModelBase
    {
        public string ID { get; set; }

        [DisplayName("名称")]
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public string Address { get; set; }
        public string Contact { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
    }
}
