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
            IEnumerable<FireLyPatientViewModel> toView = ViewModelMapper.
                PatientToFireLyVM(
                PatientWrapper.GetPatients(fhirClient)
                );
            return View(toView);
        }

        public IActionResult SearchResult(SearchViewModel searchViewModel)
        {
            IEnumerable<FireLyPatientViewModel> toView = ViewModelMapper
                .PatientToFireLyVM(
                PatientWrapper.GetPatients(fhirClient)
                );
            return View(toView);
        }

        public IActionResult Search()
        {
            return View();
        }

    }
}
