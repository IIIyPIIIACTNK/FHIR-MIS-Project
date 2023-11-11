using Microsoft.AspNetCore.Mvc;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System.ComponentModel.DataAnnotations;
using FHIR_MIS_web.ViewModels;
using FHIR_MIS_web.Interfaces;
using Serilog;
using FHIR_MIS_web.Data;
using FHIR_MIS_web.FHIR;
using System.IO;
using Microsoft.Extensions.Options;

namespace FHIR_MIS_web.Controllers
{
    public class FireLyController : Controller
    {
        FhirClient fhirClient;
        public FireLyController(IOptions<FhirServerSettings> server)
        {
            fhirClient = new FhirClient(server.Value.Url);
        }
        
        public async Task<IActionResult> Index()
        {
            PatientPull.Pull.Clear();
            IEnumerable<FireLyPatientViewModel> toView = ViewModelMapper.
                PatientToFireLyVM(
                await PatientWrapper.GetPatients(fhirClient)
                );
            return View(toView);
        }

        public async Task<IActionResult> SearchResult(SearchViewModel searchViewModel)
        {
            IEnumerable<FireLyPatientViewModel> toView = ViewModelMapper
                .PatientToFireLyVM(
                await PatientWrapper.GetPatients(fhirClient)
                );
            return View(toView);
        }

        public IActionResult Search()
        {
            return View();
        }

    }
}
