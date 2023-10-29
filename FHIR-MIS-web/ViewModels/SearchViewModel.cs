using FHIR_MIS_web.Data.Enums;
using FHIR_MIS_web.Interfaces;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Utility;
using System.Text;

namespace FHIR_MIS_web.ViewModels
{
    public class SearchViewModel : ISearchString
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FathersName { get; set; }
        public GenderEnum Gender { get; set; }
        public DateTime Birthday { get; set; }

        public string[] FormSearchStrings()
        {
            List<string> forms = new List<string>();
            if (!string.IsNullOrEmpty(LastName))
            {
                forms.Add($"family:exact={LastName}");
            }
            if (!string.IsNullOrEmpty($"given:exact={FirstName}"))
            {
                forms.Add(FirstName);
            }
            if (!string.IsNullOrEmpty(FathersName))
            {
                forms.Add(FathersName);
            }
            if (Gender != null) { }
            {
                forms.Add($"gender={Gender}");
            }
            if(Birthday!= null)
            {
                forms.Add($"birthdate={Birthday.Year}.{Birthday.Month}.{Birthday.Day}");
            }
            return forms.ToArray();
        }

        public SearchParams SeacrhParam()
        {
            var searchParams = new SearchParams();

            if (!string.IsNullOrEmpty(LastName))
            {
                searchParams.Where($"family:exact={LastName}");
            }
            if (!string.IsNullOrEmpty(FirstName))
            {
                searchParams.Where($"given:exact={FirstName}");
            }
            //searchParams.Where($"gender={Gender}");
            return searchParams;
                //new SearchParams()
                //.Where($"family:exact={LastName}")
                //.Where($"given:exact={FirstName} {FathersName}")
                //.Where($"gender={Gender}");
               
        }
    }
}
