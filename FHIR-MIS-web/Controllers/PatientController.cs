using FHIR_MIS_web.Interfaces;
using FHIR_MIS_web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Hl7.Fhir.Model;
using System.Globalization;

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
                BirthDate = viewModel.BirthDate.ToString("yyyy-MM-dd"),
                Telecom = new List<ContactPoint> { new ContactPoint()
                {
                    System = ContactPoint.ContactPointSystem.Phone,
                    Value = viewModel.Telephone
                } }
            });
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(string id)
        {
            Patient patient = _serverPatientRepository.GetById(id);
            bool hasGivenName = patient.Name.FirstOrDefault().Given.Count() > 0;
            var vm = new DetailPatientViewModel()
            {
                PatientId= id,
                Name = hasGivenName ? patient.Name.FirstOrDefault().GivenElement.FirstOrDefault().Value : "",
                Surname = patient.Name.FirstOrDefault().Family,
                Patronymic = hasGivenName ? patient.Name.FirstOrDefault().GivenElement.FirstOrDefault().Value : "",
                Adress = patient.Address.FirstOrDefault(),
                Birthdate= patient.BirthDate,
                Gender = patient.Gender.HasValue ? patient.Gender : AdministrativeGender.Unknown,
            };
            return View(vm);
            
        }

        public async Task<IActionResult> Delete(string id)
        {
            return _serverPatientRepository.Delete(id) ? RedirectToAction("Create") : RedirectToAction("Index");
        }
    }
}
