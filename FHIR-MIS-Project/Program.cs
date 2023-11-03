using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace FHIR_MIS_Project
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FhirClient _fhirClient = new FhirClient("http://localhost:8080/fhir",
                new FhirClientSettings()
                {
                    PreferredFormat = ResourceFormat.Json,
                    ReturnPreference = ReturnPreference.Representation
                }) ;
            
            _fhirClient.Create<Patient>(new Patient()
            {
                Name = new List<HumanName>(){
                    new HumanName(){
                      Family = "Super"
                    }
                }
            }) ;
        }
    }
}