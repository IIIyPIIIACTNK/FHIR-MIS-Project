using FHIR_MIS_web.Data.Enums;
using Hl7.Fhir.Model;

namespace FHIR_MIS_web.ViewModels
{
    public class UpdatePatientViewModel
    {
        public string PatientId { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public AdministrativeGender? Gender { get; set; }
        public string BirthDate { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Telephone { get; set; }
    }
}
