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
    
    public partial class Canvas
    {
        public int Id { get; set; }
        public string Canvas_Id { get; set; }
        public Nullable<int> Tag_Id { get; set; }
        public string Subtext_Title { get; set; }
        public Nullable<int> Subtext_Type { get; set; }
        public string Subtext { get; set; }
        public string Canvas_link { get; set; }
    
        public virtual Tag Tag { get; set; }
    }
}
