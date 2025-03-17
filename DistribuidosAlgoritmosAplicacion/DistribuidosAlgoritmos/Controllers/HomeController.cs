using DistribuidosAlgoritmos.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DistribuidosAlgoritmos.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly DbLogsContext _context;


        public HomeController(ILogger<HomeController> logger, DbLogsContext context)
        {
            _logger = logger;
            _context = context;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            // obtener dado una persona su asistencia
            var employee = _context.Employees.FirstOrDefault();
            var attendanceRecords = _context.AttendanceRecords.Where(a => a.EmployeeId == employee.Id).OrderBy(q => q.CreatedAt).ToList();
            return View(attendanceRecords);
            //return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
