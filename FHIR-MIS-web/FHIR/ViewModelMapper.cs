using FHIR_MIS_web.ViewModels;
using Hl7.Fhir.Model;

namespace FHIR_MIS_web.FHIR
{
    public static class ViewModelMapper
    {
        public static List<FireLyPatientViewModel> PatientToFireLyVM(List<Patient> patients)
        {
            List<FireLyPatientViewModel> patinetsToView = new List<FireLyPatientViewModel>();
            foreach (var pat in patients)
            {
                patinetsToView.Add(new FireLyPatientViewModel()
                {
                    Id = pat.Id,
                    FirstName = pat.Name.FirstOrDefault().Given.FirstOrDefault(),
                    LastName = pat.Name.FirstOrDefault().Family,
                    Gender = pat.Gender.ToString(),
                    Birthday = pat.BirthDate,
                    Address = pat.Address.FirstOrDefault()
                });
            }
            return patinetsToView;
        }
    }
}
