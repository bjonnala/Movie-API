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
    
    public partial class ActorMovy
    {
        public int ActorMovies_ID { get; set; }
        public int Actor_ID { get; set; }
        public int Movies_ID { get; set; }
    
        public virtual Actor Actor { get; set; }
        public virtual Movy Movy { get; set; }
    }
}
