using FHIR_MIS_web.ViewModels;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Serilog;

namespace FHIR_MIS_web.FHIR
{
    public static class PatientWrapper
    {
        public static List<Patient> GetPatients(
            FhirClient fhirClient,
            SearchParams criteria = null,
            int maxPatients = 20)
        {
            List<Patient> patients = new List<Patient>();
            Bundle patientBundle;

            if (criteria == null)
            {
                patientBundle = fhirClient.Search<Patient>();
            }
            else
            {
                Log.Information("Search params {@params}", criteria);
                patientBundle = fhirClient.Search<Patient>(criteria);
            }

            while (patientBundle != null)
            {

                if (patientBundle.Entry.Count == 0)
                {
                    Log.Information("Search bundle has no elements");
                    break;
                }
                foreach (var entry in patientBundle.Entry)
                {
                    if (entry.Resource.TypeName == FHIRDefinedType.OperationOutcome.ToString())
                    {
                        var operation = (OperationOutcome)entry.Resource;
                        foreach (var item in operation.Issue)
                        {
                            Log.Error("Fhir operation outcome {code} {severity} {diagnostic}",
                                item.Code.Value,
                                item.Severity.Value,
                                item.Diagnostics
                            );
                            foreach (var detail in item.Details)
                            {
                                Log.Error("Error detail {detail}",
                                    detail.Value);
                            }
                        }
                        break;
                    }
                    if (entry.Resource != null)
                    {
                        try
                        {
                            // явное приведение ресусрса entry к типу patient
                            // баг - при другом типе данных в поиске, попадаем в бесконечный цикл
                            Patient pat = (Patient)entry.Resource;
                            //проверить добавляются ли одинаковые пациенты
                            //если да то почему (возможно fhirClient.Continue())

                            patients.Add(pat);
                        }
                        catch (InvalidCastException ex)
                        {
                            Log.Error("Error {@Exception}", ex);
                            continue;
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Error {@Exception}", ex);
                            continue;
                        }
                    }
                    if (patients.Count >= maxPatients) break;
                }
                if (patients.Count >= maxPatients) break;

                fhirClient.Continue(patientBundle);
            }
            fhirClient.Dispose();
            return patients;
        }
    }
}
