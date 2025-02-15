using Application.Contracts;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClinicManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AppointmentController : Controller
    {
        private readonly IAppointment _appointment;
        private readonly IPatient _patient;
        private readonly IDoctor _doctor;
        public AppointmentController( IAppointment appointment, IPatient patient, IDoctor doctor)
        {
            _appointment = appointment;
            _patient = patient;
            _doctor = doctor;
        }
        public IActionResult List ()
        {
            var listAppointments = _appointment.GetAll();
            return View( listAppointments );
        }
        public IActionResult Edit(int? id)
        {
            // Get list of patients with ALL necessary properties (Id and DisplayName)
            ViewBag.PatientList = new SelectList(_patient.GetAll(), "Id", "Name");

            // Similarly for doctors
            ViewBag.DoctorList = new SelectList(_doctor.GetAll(), "Id", "Name");

            var appointment = new AppointmentDTO();
            if (id != null)
            {
                appointment = _appointment.GetById(Convert.ToInt32(id));
            }
            return View(appointment);
        }
        public IActionResult Save ( AppointmentDTO appointment)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", appointment);
            }
            if ( appointment.Id == 0)
            {
                _appointment.Add(appointment);
            }
            else
            {
                _appointment.Update(appointment);
            }
            return RedirectToAction("List");
        }
        public IActionResult Delete (int id)
        {
           _appointment.Delete(id);
            return RedirectToAction("List");
        }
    }
}
