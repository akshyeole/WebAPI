using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Dapper;
using EmployeeWebApi.Models;

namespace EmployeeWebApi.Controllers
{
    public class employeeController : ApiController
    {
        static string cnnstr = ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString;
        public HttpResponseMessage Get()
        {

            List<Employee> employeelist = new List<Employee>();
            using (IDbConnection cnn = new SqlConnection(cnnstr))
            {
                employeelist = cnn.Query<Employee>("getAllEmployees").ToList();
            }
            return Request.CreateResponse(HttpStatusCode.OK, employeelist);
        }
        [Route("api/Employee/getAllDepartmentsByName")]
        [HttpGet]
        public HttpResponseMessage getAllDepartmentsByName()
        {

            List <string>departmentnamelist = new List<string>();
            using (IDbConnection cnn = new SqlConnection(cnnstr))
            {
                departmentnamelist = cnn.Query<string>("getAllDepartmentsByName").ToList();
            }
            return Request.CreateResponse(HttpStatusCode.OK, departmentnamelist);
        }
        public HttpResponseMessage Post(Employee employee)
        {
            try
            {
                using (IDbConnection cnn = new SqlConnection(cnnstr))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@name", employee.name);           
                    parameters.Add("@dateofjoining", employee.dateofjoining);
                    parameters.Add("@department", employee.department);
                    parameters.Add("@photofilename", employee.photofilename);
                    cnn.Execute("addEmployee", parameters, commandType: CommandType.StoredProcedure);
                }
                return Request.CreateErrorResponse(HttpStatusCode.Created, "employee " + employee.name + " Added");
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Put(Employee employee, int id)
        {
            try
            {
                using (IDbConnection cnn = new SqlConnection(cnnstr))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@name", employee.name);
                    parameters.Add("id", id);
                    parameters.Add("@department", employee.department);
                    parameters.Add("@dateofjoining", employee.dateofjoining);
                    parameters.Add("@photofilename", employee.photofilename);
                    cnn.Execute("updateEmployee", parameters, commandType: CommandType.StoredProcedure);
                }
                return Request.CreateErrorResponse(HttpStatusCode.Created, "Employee Updated ");
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (IDbConnection cnn = new SqlConnection(cnnstr))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("id", id);
                    cnn.Execute("deleteEmployee", parameters, commandType: CommandType.StoredProcedure);
                }
                return Request.CreateErrorResponse(HttpStatusCode.Created, "Employee Deleted ");
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        [Route("api/Employee/SaveFile")]
        public string SaveFile()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                var postedFile = httpRequest.Files[0];
                string filename =postedFile.FileName;
                var physicalpath = HttpContext.Current.Server.MapPath("~/photos/"+filename);
                postedFile.SaveAs(physicalpath);
                return filename+" Uploaded";
            }
             catch (Exception ex)
            {

                return "Not Uploaded"+ex.ToString();
            }
           
        }
    }
}
