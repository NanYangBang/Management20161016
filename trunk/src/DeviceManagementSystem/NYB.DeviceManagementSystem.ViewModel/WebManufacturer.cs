﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NYB.DeviceManagementSystem.ViewModel
{
    public class WebManufacturer : ViewModelBase
    {
        public string ID { get; set; }

        [DisplayName("名称")]
        [Required(ErrorMessage="名称是必填项")]
        [MaxLength(30)]
        public string Name { get; set; }

        [DisplayName("地址")]
        public string Address { get; set; }

        [DisplayName("联系人")]
        [Required(ErrorMessage="联系人是必填项")]
        public string Contact { get; set; }

        [DisplayName("手机")]
        public string Mobile { get; set; }

        [DisplayName("电话")]
        public string Phone { get; set; }

        [DisplayName("备注")]
        public string Note { get; set; }
    }
}
