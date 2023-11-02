using FHIR_MIS_web.Interfaces;
using FHIR_MIS_web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FHIR_MIS_web.Controllers
{
    public class PatientController : Controller
    {
        IServerPatientRepository _serverPatientRepository;
        public PatientController(IServerPatientRepository serverPatientRepository)
        {
            _serverPatientRepository= serverPatientRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreatePatientViewModel viewModel) 
        {
            return RedirectToAction("Index");
        }
    }
}
