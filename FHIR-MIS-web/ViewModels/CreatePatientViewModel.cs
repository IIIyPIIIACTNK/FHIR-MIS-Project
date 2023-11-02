using FHIR_MIS_web.Data.Enums;

namespace FHIR_MIS_web.ViewModels
{
    public class CreatePatientViewModel
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public GenderEnum Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Telephone { get; set; }
    }
}
