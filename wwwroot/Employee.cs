//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RestExercise
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employee
    {
        public int employee_id { get; set; }
        public string last_name { get; set; }
        public string first_name { get; set; }
        public string branch { get; set; }
    
        public virtual Store Store { get; set; }
    }
}
