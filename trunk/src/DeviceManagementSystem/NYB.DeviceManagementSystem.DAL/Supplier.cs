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
    
    public partial class Supplier
    {
        public Supplier()
        {
            this.Device = new HashSet<Device>();
        }
    
        public string ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string ProjectID { get; set; }
        public bool IsValid { get; set; }
        public string Note { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUserID { get; set; }
    
        public virtual Project Project { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Device> Device { get; set; }
    }
}
