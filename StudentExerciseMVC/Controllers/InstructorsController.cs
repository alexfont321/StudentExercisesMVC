using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExerciseMVC.Models.ViewModels;
using StudentExercisesAPI.Data;

namespace StudentExerciseMVC.Controllers
{
    public class InstructorsController : Controller
    {

        private readonly IConfiguration _config;

        public InstructorsController(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        // GET: Instructors
        public async Task<ActionResult> Index()
        {
            using (IDbConnection conn = Connection)
            {
                IEnumerable<Instructor> instructors = await conn.QueryAsync<Instructor>(@"
                    select i.Id,
                        i.FirstName,
                        i.LastName,
                        i.SlackHandle,
                        i.Specialty,
                        i.CohortId
                    from Instructor i
                ");
                return View(instructors);
            }

        }

        // GET: Instructors/Details/5
        public async Task<ActionResult> Details(int id)
        {
            string sql = $@"
                 select i.Id,
                        i.FirstName,
                        i.LastName,
                        i.SlackHandle,
                        i.Specialty,
                        i.CohortId
                    from Instructor i
                    where i.Id = {id}
            ";
            using (IDbConnection conn = Connection)
            {
                Instructor inst = await conn.QueryFirstAsync<Instructor>(sql);
                return View(inst);
            }
        }

        // GET: Instructors/Create
        public ActionResult Create()
        {
            var model = new InstructorCreateViewModel(_config);

            return View(model);
        }

        // POST: Instructors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(InstructorCreateViewModel model)
        {
            string sql = $@"INSERT INTO Instructor 
            (FirstName, LastName, SlackHandle, Specialty, CohortId)
            VALUES
            (
                '{model.instructor.FirstName}'
                ,'{model.instructor.LastName}'
                ,'{model.instructor.SlackHandle}'
                ,'{model.instructor.Specialty}'
                ,{model.instructor.CohortId}
            );";

            using (IDbConnection conn = Connection)
            {
                var newId = await conn.ExecuteAsync(sql);
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Instructors/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            string sql = $@"
                 select i.Id,
                        i.FirstName,
                        i.LastName,
                        i.SlackHandle,
                        i.Specialty,
                        i.CohortId
                    from Instructor i
                    where i.Id = {id}
            ";

            using (IDbConnection conn = Connection)
            {
                Instructor inst = await conn.QueryFirstAsync<Instructor>(sql);
                return View(inst);
            }

        }

        // POST: Instructors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Instructor inst)
        {

            
            try
            {
                // TODO: Add update logic here
                string sql = $@"
                    UPDATE Instructor
                    SET FirstName = '{inst.FirstName}',
                        LastName = '{inst.LastName}',
                        SlackHandle = '{inst.SlackHandle}',
                        Specialty = '{inst.Specialty}'
                    WHERE Id = {id}";

                using (IDbConnection conn = Connection)
                {
                    int rowsAffected = await conn.ExecuteAsync(sql);
                    if (rowsAffected > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    return BadRequest();

                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Instructors/Delete/5
        public async Task<ActionResult> DeleteConfirm(int id)
        {
            string sql = $@"
                 select i.Id,
                        i.FirstName,
                        i.LastName,
                        i.SlackHandle,
                        i.Specialty,
                        i.CohortId
                    from Instructor i
                    where i.Id = {id}
            ";

            using (IDbConnection conn = Connection)
            {
                Instructor inst = await conn.QueryFirstAsync<Instructor>(sql);
                return View(inst);
            }
        }

        // POST: Instructors/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            string sql = $@"DELETE FROM Instructor WHERE Id = {id}";

            using (IDbConnection conn = Connection)
            {
                int rowsAffected = await conn.ExecuteAsync(sql);
                if (rowsAffected > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                throw new Exception("No rows affected");
            }
        }
    }
}