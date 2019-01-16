using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using PumoxTest.Common;
using PumoxTest.Enums;
using PumoxTest.Models;

namespace PumoxTest
{
   
    public class Service : IService
    {

        WebOperationContext ctx = WebOperationContext.Current;

        public long Create(string Name, int EstablishmentYear, List<Employee> Employees)
        {
            if (!IsAuthenticated())
            {
                throw new WebFaultException<string>(
                    string.Format($"Request not authorized."),
                    HttpStatusCode.Unauthorized);
            }
          
            if (string.IsNullOrEmpty(Name))
            {
                throw new WebFaultException<string>(
                    string.Format("Name is required."),
                    HttpStatusCode.Forbidden);
            }
            else if (EstablishmentYear <= 0)
            {
                throw new WebFaultException<string>(
                    string.Format("EstablishmentYear is required."),
                    HttpStatusCode.Forbidden);
            }

            var company = new Company()
            {
                Name = Name,
                EstablishmentYear = EstablishmentYear,
                Employees = Employees
            };

            try
            {
                using (var context = new Context())
                {
                    context.Companies.Add(company);
                    context.SaveChanges();
                    ctx.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                    return company.Id;
                }

            }
            catch (Exception e)
            {

                throw new WebFaultException<string>(
                    string.Format($"Internal server error! Error={e.Message}"),
                    HttpStatusCode.InternalServerError);
            }


        }

        public void Delete(string id)
        {
            if (!IsAuthenticated())
            {
                throw new WebFaultException<string>(
                    string.Format($"Request not authorized."),
                    HttpStatusCode.Unauthorized);
            }
            try
            {
                long Identifier = Parser.ParseToLong(id);
                using (var context = new Context())
                {
                    var entity = context.Companies.FirstOrDefault(p => p.Id == Identifier);
                    context.Companies.Remove(entity);
                    context.SaveChanges();
                }

                ctx.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                throw new WebFaultException<string>(
                string.Format($"Internal server error! Error={e.Message}"),
                HttpStatusCode.InternalServerError);

            }
        }

        [return: MessageParameter(Name = "Results")]
        public List<Company> Search(string Keyword, DateTime? EmployeeDateOfBirthFrom, DateTime? EmployyDateOfBirthTo, EmployeeJobTitlesEnum EmployeeJobTitles)
        {
            
            try
            {
                using (var context = new Context())
                {
                    var result1 = context.Companies.Where(company => company.Name.Contains(Keyword) ||
                                                                     company.Employees.All(
                                                                         c=> ((Keyword == string.Empty) || c.FirstName.Contains(Keyword)) 
                                                                         || ((EmployeeDateOfBirthFrom == null) || c.DateOfBirth >= EmployeeDateOfBirthFrom) 
                                                                             || ((EmployyDateOfBirthTo == null) || c.DateOfBirth <= EmployyDateOfBirthTo) 
                                                                             ||  ((EmployeeJobTitles.ToString() == string.Empty) || c.JobTitle == EmployeeJobTitles.ToString())))
                        .Include(company => company.Employees)
                        .ToList();



                    //var result = context.Companies.Join(context.Employees, c => c.Id, e => e.CompanyId, (c, e) => c)
                    //    .Where(c => c.Name.Contains(Keyword)
                    //                || c.Employees.All(emp => emp.FirstName.Contains(Keyword)
                    //                                          || emp.LastName.Contains(Keyword)
                    //                                          || emp.JobTitle == EmployeeJobTitles.ToString()
                    //                                          || emp.DateOfBirth > EmployeeDateOfBirthFrom
                    //                                          || emp.DateOfBirth < EmployyDateOfBirthTo)).ToList();
                    return result1;
                }
            }
            catch (Exception e)
            {
                throw new WebFaultException<string>(
                    string.Format($"Internal server error! Error={e.Message}"),
                    HttpStatusCode.InternalServerError);
            }
        }

        public void Update(string id, string name, int EstablishmentYear, List<Employee> Employees)
        {
            if (!IsAuthenticated())
            {
                throw new WebFaultException<string>(
                    string.Format($"Request not authorized."),
                    HttpStatusCode.Unauthorized);
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new WebFaultException<string>(
                    string.Format("Name is required."),
                    HttpStatusCode.Forbidden);
            }
            else if (EstablishmentYear < 0)
            {
                throw new WebFaultException<string>(
                    string.Format("EstablishmentYear is required."),
                    HttpStatusCode.Forbidden);
            }

            try
            {
                long Identifier = Parser.ParseToLong(id);
                Context context = new Context();
                var entity = context.Companies.FirstOrDefault(p => p.Id == Identifier);
                entity.Name = name;
                entity.EstablishmentYear = EstablishmentYear;
                entity.Employees = Employees;
                context.SaveChanges();

            }
            catch (Exception e)
            {
                throw new WebFaultException<string>(
                    string.Format($"Internal server error! Error={e.Message}"),
                    HttpStatusCode.InternalServerError);
            }
        }

        private bool IsAuthenticated()
        {
            var temp = ctx.IncomingRequest.Headers["Authorization"];
            if (temp != null && temp.StartsWith("Basic"))
            {
                string encodedUsernamePassword = temp.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
                int seperatorIndex = usernamePassword.IndexOf(':');

               var username = usernamePassword.Substring(0, seperatorIndex);
               var password = usernamePassword.Substring(seperatorIndex + 1);
                if (username == ConfigurationManager.AppSettings["login"] &&
                    password == ConfigurationManager.AppSettings["password"])
                    return true;
                else
                    return false;
            }
            else
            {
                throw new WebFaultException<string>(
                    string.Format($"Request not authorized."),
                    HttpStatusCode.Unauthorized);
            }
        }

    }
}
