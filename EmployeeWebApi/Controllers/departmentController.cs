using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dapper;
using EmployeeWebApi.Models;

namespace EmployeeWebApi.Controllers
{
    public class departmentController : ApiController
    {
        static string cnnstr = ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString;
        public HttpResponseMessage Get()
        {
            
            List<Department> departmentlist=new List<Department>();
            using (IDbConnection cnn=new SqlConnection(cnnstr))
            {
                departmentlist=cnn.Query<Department>("getAllDepartments").ToList();
            }
            return Request.CreateResponse(HttpStatusCode.OK,departmentlist);
        }
        public HttpResponseMessage Post(Department department)
        {
            try
            {
                using (IDbConnection cnn=new SqlConnection(cnnstr))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@name",department.Name);
                    cnn.Execute("addDepartment",parameters,commandType:CommandType.StoredProcedure);
                }
                return Request.CreateErrorResponse(HttpStatusCode.Created,"Department "+department.Name+" Added");
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,ex);
            }
        }
        public HttpResponseMessage Put(Department department,int id)
        {
            try
            {
                using (IDbConnection cnn = new SqlConnection(cnnstr))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@name", department.Name);
                    parameters.Add("id", id);
                    cnn.Execute("updateDepartment", parameters, commandType: CommandType.StoredProcedure);
                }
                return Request.CreateErrorResponse(HttpStatusCode.Created, "Department Name Updated to " + department.Name);
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
                    cnn.Execute("deleteDepartment", parameters, commandType: CommandType.StoredProcedure);
                }
                return Request.CreateErrorResponse(HttpStatusCode.Created, "Department Deleted ");
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
