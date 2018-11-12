using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using StudentExercisesAPI.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExerciseMVC.Models.ViewModels
{
    public class StudentCreateViewModel
    {
        private readonly IConfiguration _config;

        public List<SelectListItem> Cohorts { get; set; }
        public Student student { get; set; }

        public StudentCreateViewModel() { }

        public StudentCreateViewModel(IConfiguration config)
        {

            using (IDbConnection conn = new SqlConnection(config.GetConnectionString("DefaultConnection")))
            {
                Cohorts = conn.Query<Cohort>(@"
                    SELECT Id, Name FROM Cohort;
                ")
                .AsEnumerable()
                .Select(li => new SelectListItem
                {
                    Text = li.Name,
                    Value = li.Id.ToString()
                }).ToList();
                ;
            }

            Cohorts.Insert(0, new SelectListItem
            {
                Text = "Choose cohort...",
                Value = "0"
            });
        }
    }
}
