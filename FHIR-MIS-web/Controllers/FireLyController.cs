using Microsoft.AspNetCore.Mvc;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System.ComponentModel.DataAnnotations;
using FHIR_MIS_web.ViewModels;

namespace FHIR_MIS_web.Controllers
{
    public class FireLyController : Controller
    {
        private readonly static string _fireLyUrl = "http://server.fire.ly";

        public FireLyController()
        {

        }

        FhirClient fhirClient = new FhirClient(_fireLyUrl);
        public IActionResult Index()
        {
            IEnumerable<FireLyPatientViewModel> patinetsToView = GetPatient(new string[] {"name=test"}, 10);
            int i = 0;
            return View(patinetsToView);
        }

        public IActionResult SearchResult(FireLyPatientViewModel patientViewModel)
        {
            IEnumerable<FireLyPatientViewModel> patinetsToView = GetPatient(new string[]
            {
                $"name={patientViewModel.FullName}",
                $"birthday={patientViewModel.Birthday}"
            });
            return View(patientViewModel);
        }

        public IActionResult Search()
        {
            return View();
        }

        private List<FireLyPatientViewModel> GetPatient(
            string[] criteria = null,
            int maxPatient = 20
            )
        {
            List<Patient> patients = new List<Patient>();
            List<FireLyPatientViewModel> fireLyPatientViewModels= new List<FireLyPatientViewModel>();

            Bundle patientBundle;

            if (criteria == null || criteria.Length == 0)
            {
                patientBundle = fhirClient.Search<Patient>();
            }
            else
            {
                patientBundle = fhirClient.Search<Patient>(criteria);
            }

            while (patientBundle != null)
            {
                foreach (var entry in patientBundle.Entry)
                {
                    if (entry.Resource != null)
                    {
                        Patient pat = (Patient)entry.Resource;
                        patients.Add(pat);
                        fireLyPatientViewModels.Add(new FireLyPatientViewModel()
                        {
                            FirstName = pat.Name.FirstOrDefault().Given.FirstOrDefault(),
                            LastName = pat.Name.FirstOrDefault().Family,
                            Gender = pat.Gender.ToString(),
                            Birthday = pat.BirthDate,
                            Address = pat.Address.FirstOrDefault()
                        }) ;
                    }
                    if (patients.Count >= maxPatient) break;
                }
                if (patients.Count >= maxPatient) break;
            }

            return fireLyPatientViewModels;
        }
    }
}
