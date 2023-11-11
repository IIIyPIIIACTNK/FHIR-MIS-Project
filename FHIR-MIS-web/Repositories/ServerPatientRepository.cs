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
        FhirClientSettings settings = new FhirClientSettings()
        {
            PreferredFormat = ResourceFormat.Json
        };
        public ServerPatientRepository(IOptions<FhirServerSettings> server)
        {
            _client = new FhirClient(server.Value.Url, settings);
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

        public bool Delete(string id)
        {
            if (id == null)
            {
                Log.Error("Input id in delete func was null");
                return false;
            }
            try
            {
                Patient pat = (Patient)_client.SearchByIdAsync<Patient>(id).Result.Entry.FirstOrDefault().Resource;
                _client.DeleteAsync(pat);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to delete patient. Error: {ex.Message}");
                return false;
            }
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
                Log.Error("Patient in update func was null");
                return false;
            }
            try
            {
                _client.UpdateAsync(patient);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"Error  in update patient func: {ex.Message} {ex.InnerException}");
                return false;
            }
        }
    }
}
