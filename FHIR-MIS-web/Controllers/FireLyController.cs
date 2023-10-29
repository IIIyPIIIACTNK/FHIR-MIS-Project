using Microsoft.AspNetCore.Mvc;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System.ComponentModel.DataAnnotations;
using FHIR_MIS_web.ViewModels;
using FHIR_MIS_web.Interfaces;
using Serilog;
using FHIR_MIS_web.Data;

namespace FHIR_MIS_web.Controllers
{
    public class FireLyController : Controller
    {

        public FireLyController()
        {

        }

        FhirClient fhirClient = new FhirClient(FHIRServers.Servers["FireLy"]);
        public IActionResult Index()
        {
            IEnumerable<FireLyPatientViewModel> patinetsToView = GetPatient(new SearchParams());
            return View(patinetsToView);
        }

        public IActionResult SearchResult(SearchViewModel searchViewModel)
        {
            IEnumerable<FireLyPatientViewModel> patientsToView = GetPatient(
                searchViewModel.SeacrhParam()
            ); ;
            
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
            //var q = new SearchParams().Where("name:exact= ");

            if (criteria == null || criteria.Length == 0)
            {
                patientBundle = fhirClient.Search<Patient>();
            }
            else
            {
                patientBundle = fhirClient.Search<Patient>(criteria);
                //patientBundle = fhirClient.Search<Patient>(q);
            }

            while (patientBundle != null)
            {
                if (patientBundle.Entry.Count == 0) break;
                foreach (var entry in patientBundle.Entry)
                {
                    if (entry.Resource != null)
                    {
                        try
                        {
                            // явное приведение ресусрса entry к типу patient
                            // баг - при другом типе данных в поиске, попадаем в бесконечный цикл
                            Patient pat = (Patient)entry.Resource;
                            patients.Add(pat);
                            //проверить добавляются ли одинаковые пациенты
                            //если да то почему (возможно fhirClient.Continue())
                            fireLyPatientViewModels.Add(new FireLyPatientViewModel()
                            {
                                Id= pat.Id,
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
            fhirClient.Dispose();
            return fireLyPatientViewModels;
        }
        
        private List<FireLyPatientViewModel> GetPatient(
            SearchParams criteria = null,
            int maxPatient = 20
            )
        {
            List<FireLyPatientViewModel> fireLyPatientViewModels= new List<FireLyPatientViewModel>();

            Bundle patientBundle;

            if (criteria == null)
            {
                patientBundle = fhirClient.Search<Patient>();
            }
            else
            {
                Log.Information("Search params {@params}", criteria);
                patientBundle = fhirClient.Search<Patient>(criteria);
            }

            while (patientBundle != null)
            {
                
                if (patientBundle.Entry.Count == 0) {
                    Log.Information("Search bundle has no elements");
                    break;
                } 
                foreach (var entry in patientBundle.Entry)
                {
                    if (entry.Resource.TypeName == FHIRDefinedType.OperationOutcome.ToString())
                    {
                        var operation = (OperationOutcome)entry.Resource;
                        foreach (var item in operation.Issue)
                        {
                            Log.Error("Fhir operation outcome {code} {severity} {diagnostic}",
                                item.Code.Value, 
                                item.Severity.Value, 
                                item.Diagnostics
                            );
                            foreach (var detail in item.Details)
                            {
                                Log.Error("Error detail {detail}",
                                    detail.Value);
                            }
                        }
                        break;
                    }
                    if (entry.Resource != null)
                    {
                        try
                        {
                            // явное приведение ресусрса entry к типу patient
                            // баг - при другом типе данных в поиске, попадаем в бесконечный цикл
                            Patient pat = (Patient)entry.Resource;
                            //проверить добавляются ли одинаковые пациенты
                            //если да то почему (возможно fhirClient.Continue())

                            fireLyPatientViewModels.Add(new FireLyPatientViewModel()
                            {
                                Id= pat.Id,
                                FirstName = pat.Name.FirstOrDefault().Given.FirstOrDefault(),
                                LastName = pat.Name.FirstOrDefault().Family,
                                Gender = pat.Gender.ToString(),
                                Birthday = pat.BirthDate,
                                Address = pat.Address.FirstOrDefault()
                            });
                        }
                        catch (InvalidCastException ex)
                        {
                            Log.Error("Error {@Exception}", ex);
                            continue;
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Error {@Exception}", ex);
                            continue;
                        }
                    }
                    if (fireLyPatientViewModels.Count >= maxPatient) break;
                }
                if (fireLyPatientViewModels.Count >= maxPatient) break;
                
                fhirClient.Continue(patientBundle);
            }
            fhirClient.Dispose();
            return fireLyPatientViewModels;
        }
    }
}
