using Hl7.Fhir.Model;

namespace FHIR_MIS_web.Interfaces
{
    public interface IServerPatientRepository
    {
        public bool Create(Patient patient);
        public bool Update(Patient patient);
        public Patient GetById(string id);
        public bool Delete(Patient patient);
    }
}
