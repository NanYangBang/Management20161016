//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace NYB.DeviceManagementSystem.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Project
    {
        public Project()
        {
            this.Device = new HashSet<Device>();
            this.DeviceType = new HashSet<DeviceType>();
            this.MaintainRecord = new HashSet<MaintainRecord>();
            this.Manufacturer = new HashSet<Manufacturer>();
            this.RepairRecord = new HashSet<RepairRecord>();
            this.Supplier = new HashSet<Supplier>();
            this.User = new HashSet<User>();
        }
    
        public string ID { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUserID { get; set; }
    
        public virtual ICollection<Device> Device { get; set; }
        public virtual ICollection<DeviceType> DeviceType { get; set; }
        public virtual ICollection<MaintainRecord> MaintainRecord { get; set; }
        public virtual ICollection<Manufacturer> Manufacturer { get; set; }
        public virtual ICollection<RepairRecord> RepairRecord { get; set; }
        public virtual ICollection<Supplier> Supplier { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
