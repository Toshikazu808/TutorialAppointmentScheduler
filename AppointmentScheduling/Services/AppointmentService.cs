using AppointmentScheduling.Models;
using AppointmentScheduling.Models.ViewModels;
using AppointmentScheduling.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduling.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _db;
        public AppointmentService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<int> AddUpdate(AppointmentVM model)
        {
            var startDate = DateTime.Parse(model.StartDate);
            var endDate = DateTime.Parse(model.StartDate).AddMinutes(Convert.ToDouble(model.Duration));

            if (model != null && model.Id > 0)
            {
                // Update
                return 1;
            }
            else
            {
                // Create
                Appointment appointment = new Appointment()
                {
                    Title = model.Title,
                    Description = model.Description,
                    StartDate = startDate,
                    EndDate = endDate,
                    Duration = model.Duration,
                    DoctorId = model.DoctorId,
                    PatientId = model.PatientId,
                    IsDoctorApproved = false,
                    AdminId = model.AdminId
                };

                _db.Appointments.Add(appointment);
                await _db.SaveChangesAsync();
                return 2;
            }
        }

        public List<DoctorVM> GetDoctorList()
        {
            /* 
             * In our database we can see that our users tabel is called AspNetUsers.  We have to retrieve a list of users from this table and then join that to the AspNetRoles table using AspNetUserRoles as an intermediate table, and get only the users whos role is doctor.
               How do we acces the AspNetUsers, AspNetRoles, and AspNetUserRoles tables?
               Because they are included inside Identity, we can access them directly.
               The name of the tables inside our db will be everything following AspNet 

               Here we will be using query syntax.  This is more descriptive than method syntax when you are joining multiple tables.
               
             */

            // This is known as projection, because we are not retrieving everything, but only certain columns.
            var doctors = (from user in _db.Users
                           join userRoles in _db.UserRoles on user.Id equals userRoles.UserId // Here we are joining tables using userRoles
                           join roles in _db.Roles.Where(x => x.Name == Helper.doctor) on userRoles.RoleId equals roles.Id
                           select new DoctorVM
                           {
                               Id = user.Id,
                               Name = user.Name
                           }
                           ).ToList();
            return doctors;
        }

        public List<PatientVM> GetPatientList()
        {
            var patients = (from user in _db.Users
                           join userRoles in _db.UserRoles on user.Id equals userRoles.UserId // Here we are joining tables using userRoles
                           join roles in _db.Roles.Where(x => x.Name == Helper.patient) on userRoles.RoleId equals roles.Id
                           select new PatientVM
                           {
                               Id = user.Id,
                               Name = user.Name
                           }
                           ).ToList();
            return patients;
        }
    }
}
