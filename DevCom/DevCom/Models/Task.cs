//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DevCom.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Task
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Task()
        {
            this.Update_Histories = new HashSet<Update_Histories>();
        }
    
        public int Task_Id { get; set; }
        public int Uid { get; set; }
        public string Short_Description { get; set; }
        public System.DateTime Creation_Time { get; set; }
        public Nullable<System.DateTime> Target_Time { get; set; }
        public int Status { get; set; }
        public string Icon { get; set; }
    
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Update_Histories> Update_Histories { get; set; }
    }
}