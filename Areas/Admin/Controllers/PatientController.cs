using Application.Contracts;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PatientController : Controller
    {
        private readonly   IPatient _patient;
        public PatientController( IPatient patient)
        {
            _patient = patient;
        }
        public IActionResult List()
        {
            var listPatients = _patient.GetAll();
            return View(listPatients);
        }
        public IActionResult Edit( int? id)
        {
            var patient = new PatientDTO();
            if (id != null )
            {
                 patient = _patient.GetById( Convert.ToInt32(id));
               
            }

            return View(patient);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save( PatientDTO patient)
        {
          if (!ModelState.IsValid)
            {
                return View("Edit" ,patient );
            }
           if (patient.Id == null || patient.Id == 0)
            {
                _patient.Add(patient);
            }
            else
            {
                _patient.Update(patient);
            }
            return RedirectToAction("List");
           
        }
 
    }
}
