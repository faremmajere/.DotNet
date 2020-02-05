using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Isen.Dotnet.Library.Model
{
    public class Role : BaseEntity
    { 
        public string Name {get;set;}
        public ICollection<RolePersonne> RolePersonnes {get;set;}
        public override string ToString() =>
            $"{Id} : {Name}" ;

    }
}
