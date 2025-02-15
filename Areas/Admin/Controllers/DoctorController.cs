using Application.Contracts;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DoctorController : Controller
    {
        private readonly IDoctor _doctor;
        public DoctorController( IDoctor doctor)
        {
          _doctor = doctor;
        }
        public IActionResult List()
        {
            var listDoctors = _doctor.GetAll();
            return View( listDoctors);
        }
        public IActionResult Edit(int? id)
        {
            var doctor = new DoctorDTO();
            if (id != null)
            {
                doctor = _doctor.GetById(Convert.ToInt32(id));
            }
            return View(doctor);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(DoctorDTO doctor)
        {
            if (!ModelState.IsValid)
            { 
                return View("Edit", doctor);
            }
            if ( doctor.Id == 0)
            {
                _doctor.Add(doctor);
            }
            else
            {
                _doctor.Update(doctor);
            }
            return RedirectToAction("List");
        }
        public IActionResult Delete(int id)
        {
            _doctor.Delete(id);
            return RedirectToAction("List");
        }
    }
}
