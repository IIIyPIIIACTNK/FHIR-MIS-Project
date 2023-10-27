using FHIR_MIS_web.Data.Enums;
using FHIR_MIS_web.Interfaces;
using System.Text;

namespace FHIR_MIS_web.ViewModels
{
    public class SearchViewModel : ISearchString
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FathersName { get; set; }
        public GenderEnum Gender { get; set; }
        public DateOnly Birthday { get; set; }

        public string[] FormSearchStrings()
        {
            Console.WriteLine($"name={LastName} {FirstName} {FathersName}" +
                $"birthday={Birthday}" +
                $"gender={Gender}");
            return new string[]
            {
                $"name={LastName} {FirstName} {FathersName}",
                $"birthday={Birthday}",
            };
        }
    }
}
