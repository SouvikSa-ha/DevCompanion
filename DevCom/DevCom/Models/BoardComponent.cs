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
    
    public partial class BoardComponent
    {
        public int Id { get; set; }
        public string Content_Id { get; set; }
        public int Board_Id { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
    
        public virtual Visual_Boards Visual_Boards { get; set; }
    }
}
