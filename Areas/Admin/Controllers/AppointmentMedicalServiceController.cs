using Application.Contracts;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClinicManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AppointmentMedicalServiceController : Controller
    {
        private readonly IAppointmentMedicalService _appointmentMedicalService;
        private readonly IMedicalService _medicalService; 
        private readonly IAppointment _appointment;
        public AppointmentMedicalServiceController(IAppointmentMedicalService appointmentMedicalService, 
            IMedicalService medicalService,
            IAppointment appointment)
        {
            _appointmentMedicalService = appointmentMedicalService;
            _medicalService = medicalService;
            _appointment = appointment;
        }
        public IActionResult List( )
        {
            var listAppointmentMedicalServices = _appointmentMedicalService.GetAll();
            return View( listAppointmentMedicalServices );
        }
        public IActionResult Edit(int? id)
        {
            ViewBag.listMedicalServices = new SelectList(_medicalService.GetAll(), "Id", "Name");
            ViewBag.listAppointments = new SelectList(_appointment.GetAll(), "Id", "Name");
             var appointmentMedicalService = new AppointmentServiceDTO();
            if (id != null)
            {
                 appointmentMedicalService = _appointmentMedicalService.GetById(Convert.ToInt32(id));
               
            }
            return View( appointmentMedicalService);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(AppointmentServiceDTO appointmentMedicalService)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.listMedicalServices = new SelectList(_medicalService.GetAll(), "Id", "Name");
                ViewBag.listAppointments = new SelectList(_appointment.GetAll(), "Id", "Name");
                return View("Edit", appointmentMedicalService);
            }
            if (appointmentMedicalService.Id == 0)
            {
                _appointmentMedicalService.Add(appointmentMedicalService);
            }
            else
            {
                _appointmentMedicalService.Update(appointmentMedicalService);
            }
            return RedirectToAction("List");
        }
        public IActionResult Delete(int id)
        {
           _appointmentMedicalService.Delete(id);
            return RedirectToAction("List");
        }
    }
}
