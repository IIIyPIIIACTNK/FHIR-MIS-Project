using FHIR_MIS_web.Data;
using FHIR_MIS_web.Interfaces;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Microsoft.Extensions.Options;
using Serilog;

namespace FHIR_MIS_web.Repositories
{
    public class ServerPatientRepository : IServerPatientRepository
    {
        FhirClient _client;
        public ServerPatientRepository(IOptions<FhirServerSettings> server)
        {
            _client = new FhirClient(server.Value.Url);
        }
        public bool Create(Patient patient)
        {
            if(patient == null)
            {
                Log.Error("Patient in create func was null");
                return false;
            }
            _client.CreateAsync<Patient>(patient);
            return true;
        }

        public bool Delete(Patient patient)
        {
            if (patient == null)
            {
                Log.Error("Patient in create func was null");
                return false;
            }
            _client.DeleteAsync(patient);
            return true;
        }

        public Patient GetById(string id)
        {
            Bundle bundle = _client.SearchByIdAsync<Patient>(id).Result;
            Patient patient = (Patient)bundle.Entry.FirstOrDefault().Resource;
            return patient;
        }

        public bool Update(Patient patient)
        {
            if (patient == null)
            {
                Log.Error("Patient in create func was null");
                return false;
            }
            _client.UpdateAsync(patient);
            return true;
        }
    }
}
