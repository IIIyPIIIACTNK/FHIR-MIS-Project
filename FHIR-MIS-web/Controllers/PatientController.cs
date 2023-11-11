using FHIR_MIS_web.Interfaces;
using FHIR_MIS_web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Hl7.Fhir.Model;
using System.Globalization;
using FHIR_MIS_web.Data;

namespace FHIR_MIS_web.Controllers
{
    public class PatientController : Controller
    {
        IServerPatientRepository _serverPatientRepository;
        public PatientController(IServerPatientRepository serverPatientRepository)
        {
            _serverPatientRepository = serverPatientRepository;
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
            PatientPull.Pull.Add(id, patient);
            var vm = new DetailPatientViewModel()
            {
                PatientId = id,
                Name = hasGivenName ? patient.Name.FirstOrDefault().GivenElement.FirstOrDefault().Value : "",
                Surname = patient.Name.FirstOrDefault().Family,
                Patronymic = hasGivenName ? patient.Name.FirstOrDefault().GivenElement.FirstOrDefault().Value : "",
                Adress = patient.Address.FirstOrDefault(),
                Birthdate = patient.BirthDate,
                Gender = patient.Gender.HasValue ? patient.Gender : AdministrativeGender.Unknown,
            };
            return View(vm);

        }

        public async Task<IActionResult> Delete(string id)
        {
            return _serverPatientRepository.Delete(id) ? RedirectToAction("Index") : RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            Patient patient = PatientPull.Pull[id];
            bool hasGivenName = patient.Name.FirstOrDefault().Given.Count() > 0;
            bool hasAddress = patient.Address.Count > 0;
            UpdatePatientViewModel vm = new UpdatePatientViewModel()
            {
                PatientId = id,
                Name = hasGivenName ? patient.Name.FirstOrDefault().GivenElement.FirstOrDefault().Value : "",
                Surname = patient.Name.FirstOrDefault().Family,
                Patronymic = hasGivenName ? patient.Name.FirstOrDefault().GivenElement.FirstOrDefault().Value : "",
                City = hasAddress ? patient.Address.FirstOrDefault().City : "",
                Street = hasAddress ? patient.Address.FirstOrDefault().Line.FirstOrDefault() : "",
                BirthDate = patient.BirthDate,
                Gender = patient.Gender.HasValue ? patient.Gender : AdministrativeGender.Unknown,
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdatePatientViewModel viewModel)
        {
            Patient pat = PatientPull.Pull[viewModel.PatientId];
            HumanName name = new HumanName
            {
                Family = viewModel.Surname,
                Given = new string[]
                {
                    viewModel.Name,
                    viewModel.Surname
                }
            };
            ContactPoint telephone = new ContactPoint()
            {
                Value = viewModel.Telephone,
                System = ContactPoint.ContactPointSystem.Phone

            };
            pat.Name.Clear();
            pat.Name.Add(name);
            pat.BirthDate = viewModel.BirthDate;
            pat.Gender = viewModel.Gender;
            pat.Telecom.Clear();
            pat.Telecom.Add(telephone);

            PatientPull.Pull.Clear();
            return _serverPatientRepository.Update(pat) ? RedirectToAction("Detail", new { id = viewModel.PatientId }) : RedirectToAction("Index","FireLy");
        }
    }
}
