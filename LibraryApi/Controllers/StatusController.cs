﻿
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class StatusController : Controller
    {

        ISystemTime systemTime;
        IConfiguration config;

        public StatusController(ISystemTime systemTime, IConfiguration config)
        {
            this.systemTime = systemTime;
            this.config = config;
        }




        // GET /status -> 200 Ok

        [HttpGet("/status")]
        public ActionResult GetTheStatus()
        {
            var response = new GetStatusResponse
            {
                Message = "Everything is Golden!",// + config.GetValue<string>("appName"),
                CheckedBy = "Joe Schmidtly",
                WhenLastChecked = systemTime.GetCurrent()
            };
            return Ok(response);
            // one last thing!
        }

        // GET /employees/93/salary
        [HttpGet("employees/{employeeId:int:min(0)}/salary")]
        public ActionResult GetEmployeeSalary(int employeeId)
        {
            return Ok($"The Employee {employeeId} has a salary of $72,000.00");
        }

        // GET /employees?dept=DEV
        [HttpGet("employees")]
        public ActionResult GetEmployees([FromQuery]string dept = "All")
        {
            return Ok($"Returning employees for department {dept}");
        }

        // "Kinds" of resources (Resource Archetypes)
        // 1. Document - a single thingy.  GET /employee/52
        // 2. Collection - a plural thingy GET /employees?dept=DEV
        // 3. Store
        // 4. Controller

        [HttpGet("whoami")]
        public ActionResult WhoAmi(
            [FromHeader(Name ="User-Agent")]
            string userAgent)
        {
            return Ok($"I see you are running {userAgent}");
        }

        [HttpPost("employees")]
        public ActionResult HireEmployee([FromBody] EmployeeCreateRequest employeeToHire, 
            [FromHeader(Name ="Content-Type")] string ellis)
        {
            return Ok($"Hiring {employeeToHire.LastName} as a {employeeToHire.Department} \n {ellis}");

        }

    }

    public class GetStatusResponse
    {
        public string Message { get; set; }
        public string CheckedBy { get; set; }
        public DateTime WhenLastChecked { get; set; }
    }


    public class EmployeeCreateRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
    }

}
