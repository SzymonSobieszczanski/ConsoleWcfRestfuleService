using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using PumoxTest.Enums;
using PumoxTest.Models;

namespace PumoxTest
{
    [ServiceContract]
    public interface IService
    {

        [OperationContract,
         WebInvoke(UriTemplate = "/company/create", Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped,
             RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        long Create(string Name, int EstablishmentYear, List<Employee> Employees);


        [OperationContract,
         WebInvoke(UriTemplate = "/company/search", Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped,
             RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [return: MessageParameter(Name = "Results")]
        List<Company> Search(string Keyword, DateTime? EmployeeDateOfBirthFrom, DateTime? EmployyDateOfBirthTo, EmployeeJobTitlesEnum EmployeeJobTitles);

        [OperationContract,
         WebInvoke(UriTemplate = "/company/update/{id}", Method = "PUT", BodyStyle = WebMessageBodyStyle.Wrapped,
             RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void Update(string id, string name, int EstablishmentYear, List<Employee> Employees);


        [OperationContract,
         WebInvoke(UriTemplate = "/company/delete/{id}", Method = "DELETE", BodyStyle = WebMessageBodyStyle.Wrapped,
             RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]

        void Delete(string id);
    }
}
