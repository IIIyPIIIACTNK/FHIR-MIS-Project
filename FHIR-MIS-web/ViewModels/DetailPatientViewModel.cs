using Hl7.Fhir.Model;
using System.Security.Principal;

namespace FHIR_MIS_web.ViewModels
{
    public class DetailPatientViewModel
    {
        public string Name { get; set; }
        public string? Surname { get; set; }
        public string Patronymic { get; set; }
        public AdministrativeGender? Gender { get; set; }
        public string Birthdate { get; set; }
        public Address? Adress { get; set; }
    }
}
