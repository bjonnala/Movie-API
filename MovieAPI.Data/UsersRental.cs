//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MovieAPI.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class UsersRental
    {
        public int UserMovieRental_ID { get; set; }
        public int Users_ID { get; set; }
        public int Movies_ID { get; set; }
        public Nullable<System.DateTime> RentalDateFrom { get; set; }
        public Nullable<System.DateTime> RentalDateTo { get; set; }
        public Nullable<int> UsersTransaction_ID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    
        public virtual Movy Movy { get; set; }
        public virtual User User { get; set; }
        public virtual UsersTransaction UsersTransaction { get; set; }
    }
}
