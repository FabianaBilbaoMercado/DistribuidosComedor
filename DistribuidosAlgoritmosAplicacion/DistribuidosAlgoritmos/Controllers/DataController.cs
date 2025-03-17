using DistribuidosAlgoritmos.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DistribuidosAlgoritmos.Controllers
{
    public class DataController : Controller
    {


        private readonly DbLogsContext _context;

        public DataController(DbLogsContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("api/process-data")]
        public async Task<IActionResult> ProcessData([FromBody] List<EmployeeJson> jsonData)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var employeeJson in jsonData)
                {
                    // Check if employee exists by external_id and full_name
                    var existingEmployee = await _context.Employees
                        .FirstOrDefaultAsync(e => e.ExternalId == employeeJson.ID && e.FullName == employeeJson.Nombre);

                    int employeeId;
                    if (existingEmployee == null)
                    {
                        // Ensure department exists or create it
                        var department = await _context.Departments
                            .FirstOrDefaultAsync(d => d.Name == employeeJson.Departamento);
                        if (department == null)
                        {
                            department = new Department { Name = employeeJson.Departamento };
                            _context.Departments.Add(department);
                            await _context.SaveChangesAsync();
                        }

                        var newEmployee = new Employee
                        {
                            ExternalId = employeeJson.ID,
                            FullName = employeeJson.Nombre,
                            DepartmentId = department.Id
                        };
                        _context.Employees.Add(newEmployee);
                        await _context.SaveChangesAsync();
                        employeeId = newEmployee.Id;
                    }
                    else
                    {
                        employeeId = existingEmployee.Id;
                    }

                    // Check if any attendance record exists for this employee
                    var hasAttendance = await _context.AttendanceRecords
                        .AnyAsync(a => a.EmployeeId == employeeId);
                    if (!hasAttendance)
                    {
                        var attendanceRecords = employeeJson.Datos.Select(d => new AttendanceRecord
                        {
                            EmployeeId = employeeId,
                            RecordTime = DateTime.Parse(d.Fecha),
                            RecordType = d.Tipo
                        }).ToList();
                        _context.AttendanceRecords.AddRange(attendanceRecords);
                        await _context.SaveChangesAsync();
                    }
                }

                await transaction.CommitAsync();
                return Ok("Data processed successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Error processing data: {ex.Message}");
            }
        }

        // GET: DataController
        public ActionResult Index()
        {
            return View();
        }

        // GET: DataController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DataController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DataController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DataController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DataController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DataController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DataController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }


    public class EmployeeJson
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Departamento { get; set; }
        public List<DatoJson> Datos { get; set; }
    }

    public class DatoJson
    {
        public string Fecha { get; set; }
        public string Tipo { get; set; }
    }



}
