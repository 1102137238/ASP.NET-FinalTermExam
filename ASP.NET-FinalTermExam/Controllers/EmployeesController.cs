using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ASP.NET_FinalTermExam.Models;

namespace ASP.NET_FinalTermExam.Controllers
{
    public class EmployeesController : Controller
    {
        // GET: Order
        public ActionResult Index(Models.Employees employees)
        {
            Models.EmployeesService empService = new Models.EmployeesService();
            ViewBag.empInfo = empService.GetEmployeesTitleArray();

            ViewBag.empResult = empService.GetEmployees(employees);

            return View(new Employees());
        }


        [HttpPost()]
        public JsonResult DeleteEmployee(string EmployeeID)
        {

            try
            {
                Models.EmployeesService empService = new Models.EmployeesService();
                empService.DeleteEmployeeById(EmployeeID);

                return this.Json(true);
            }
            catch (Exception)
            {
                return this.Json(false);
            }
        }

    }
}