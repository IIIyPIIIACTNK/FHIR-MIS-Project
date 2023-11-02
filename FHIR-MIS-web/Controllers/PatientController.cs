using FHIR_MIS_web.Interfaces;
using FHIR_MIS_web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Hl7.Fhir.Model;
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
            IEnumerable<string> GivenName = new List<string>()
            {
                viewModel.Name,
                viewModel.Patronymic
            };
            _serverPatientRepository.Create(new Patient()
            {
                //Имя
                Name = new List<HumanName>()
                {
                    new HumanName()
                    {
                        Family = viewModel.Surname,
                        Given = GivenName,
                    }
                },
                Gender = viewModel.FhirGender,
                BirthDate = viewModel.BirthDate.Date.ToShortDateString(),
                Telecom = new List<ContactPoint> { new ContactPoint()
                {
                    System = ContactPoint.ContactPointSystem.Phone,
                    Value = viewModel.Telephone
                } }
            });
            return RedirectToAction("Index");
        }
    }
}
