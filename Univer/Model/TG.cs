//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Univer.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TG
    {
        public int Id { get; set; }
        public Nullable<int> Group_Id { get; set; }
        public Nullable<int> Teacher_Id { get; set; }
        public Nullable<int> Subject_Id { get; set; }
    
        public virtual Group Group { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
