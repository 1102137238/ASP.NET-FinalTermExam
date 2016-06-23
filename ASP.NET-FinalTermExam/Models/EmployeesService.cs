using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;
using System.Web.Mvc;

namespace ASP.NET_FinalTermExam.Models
{
    public class EmployeesService
    {
        private string GetDBConnectionString()
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }

        public List<SelectListItem> GetEmployeesTitleArray()
        {
            DataTable result = new DataTable();
            string sql = @"SELECT Title FROM HR.Employees GROUP BY Title";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(result);
                conn.Close();
            }
            return this.MapEmployeeList(result);

        }

        public List<SelectListItem> MapEmployeeList(DataTable EmployeeData)
        {
            List<SelectListItem> result = new List<SelectListItem>();

            result.Add(new SelectListItem()
            {
                Text = "",
                Value = null
            });

            foreach (DataRow row in EmployeeData.Rows)
            {
                result.Add(new SelectListItem()
                {
                    Text = row["Title"].ToString(),
                    Value = row["Title"].ToString()
                });
            }

            return result;

        }


        public List<Employees> GetEmployees(Models.Employees employees)
        {
            DataTable result = new DataTable();
            string sql = @"SELECT EmployeeID,CONCAT(FirstName,LastName) as FullName,Title,HireDate,Gender,BirthDate
                FROM HR.Employees 
                WHERE (EmployeeID LIKE @EmployeeID OR @EmployeeID = '') AND 
                           (CONCAT(FirstName,LastName) LIKE '%'+@FirstName+'%' OR @FirstName = '') AND 
                            (Title LIKE @Title OR @Title = '') AND 
                            (HireDate >= @HireDate AND HireDate < HireDate OR @HireDate = '')";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@EmployeeID", employees.EmployeeID == null ? string.Empty : employees.EmployeeID));
                cmd.Parameters.Add(new SqlParameter("@FirstName", employees.FirstName == null ? string.Empty : employees.FirstName));
                cmd.Parameters.Add(new SqlParameter("@Title", employees.Title == null ? string.Empty : employees.Title));
                cmd.Parameters.Add(new SqlParameter("@HireDate", employees.HireDate == null ? string.Empty : employees.HireDate));

                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(result);
                conn.Close();
            }

            return this.MapList(result);
        }

        
        private List<Employees> MapList(DataTable data)
        {
            List<Employees> result = new List<Employees>();


            foreach (DataRow row in data.Rows)
            {
                result.Add(new Employees()
                {
                    EmployeeID = row["EmployeeID"].ToString(),
                    FullName = row["FullName"].ToString(),
                    Title = row["Title"].ToString(),
                    HireDate = row["HireDate"].ToString(),
                    Gender = row["Gender"].ToString(),
                    BirthDate = row["BirthDate"].ToString()
                });
            }
            return result;
        }


        public void DeleteEmployeeById(string EmployeeID)
        {
            try
            {
                string sql = "Delete FROM HR.Employees Where EmployeeID=@EmployeeID";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(sql, conn);
                    command.Parameters.Add(new SqlParameter("@EmployeeID", EmployeeID));
                    command.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}