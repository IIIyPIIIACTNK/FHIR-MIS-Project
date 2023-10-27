using Microsoft.AspNetCore.Mvc;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System.ComponentModel.DataAnnotations;
using FHIR_MIS_web.ViewModels;
using FHIR_MIS_web.Interfaces;

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
            IEnumerable<FireLyPatientViewModel> patinetsToView = GetPatient();
            return View(patinetsToView);
        }

        public IActionResult SearchResult(SearchViewModel searchViewModel)
        {
            IEnumerable<FireLyPatientViewModel> patientsToView = GetPatient(
                searchViewModel.FormSearchStrings()
            );
            return View(patientsToView);
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
                        try
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
                            });
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            continue;
                        }
                        
                    }
                    if (patients.Count >= maxPatient) break;
                }
                if (patients.Count >= maxPatient) break;
                fhirClient.Continue(patientBundle);
            }

            return fireLyPatientViewModels;
        }
    }
}
