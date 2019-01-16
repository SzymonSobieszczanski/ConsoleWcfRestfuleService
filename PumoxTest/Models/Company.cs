using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace PumoxTest.Models
{
   [DataContract]
    public class Company
    {
        [Key]
        [Required]
  
        public long Id { get; set; }
        [Required]
        [DataMember(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [DataMember(Name = "EstablishmentYear")]
        public int EstablishmentYear { get; set; }
        [DataMember]
        public ICollection<Employee> Employees { get; set; }
    }
}
