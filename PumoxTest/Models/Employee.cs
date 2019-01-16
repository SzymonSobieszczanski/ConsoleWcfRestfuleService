using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using PumoxTest.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace PumoxTest.Models
{
    [DataContract]
    public class Employee
    {
        
        [Key]
        [Required]
        public long Id { get; set; }
        [Required]
        public long CompanyId { get; set; }

        [Required]
        [DataMember(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [DataMember(Name = "LastName")]
        public string LastName { get; set; }

     
        [DataMember(Name = "DateOfBirth")]
        public DateTime DateOfBirth { get; set; }

       
        [DataMember(Name = "JobTitle")]
        [JsonConverter(typeof(EmployeeJobTitlesEnum))]
        public string  JobTitle { get; set; }

        public Company company { get; set; }
    }
}
