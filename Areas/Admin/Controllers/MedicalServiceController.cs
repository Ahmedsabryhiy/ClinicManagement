using Application.Contracts;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MedicalServiceController : Controller
    {
        private readonly IMedicalService _medicalService;
        public MedicalServiceController( IMedicalService medicalService)
        {
            _medicalService = medicalService;
        }
        public IActionResult List()
        {
            var listMedicalServices = _medicalService.GetAll();
            return View(listMedicalServices );
        }
        public IActionResult Edit(int? id)
        {
            var medicalService = new MedicalServiceDTO();
            if (id != null)
            {
                medicalService = _medicalService.GetById(Convert.ToInt32(id));
            }
            return View(medicalService);
        }
        [HttpPost]
        [ValidateAntiForgeryToken ]
        public IActionResult Save(MedicalServiceDTO medicalService)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", medicalService);
            }
            if (medicalService.Id == 0)
            {
                _medicalService.Add(medicalService);
            }
            else
            {
                _medicalService.Update(medicalService);
            }
            return RedirectToAction("List");
        }
        public IActionResult Delete(int id)
        {
            _medicalService.Delete(id);
            return RedirectToAction("List");
        }
    }
}
