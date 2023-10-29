using Hl7.Fhir.Model;
using System.ComponentModel;

namespace FHIR_MIS_web.ViewModels
{
    public class FireLyPatientViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string FathersName { get; set; } = "";
        public string Gender { get; set; } = "";
        public string Birthday { get; set; } = "01.01.1900";
        public Address Address { get; set; } = new Address() { 
            City="город",
            Line=new string[] {"улица"},
        };

        public string FullName()
        {
            return $"{LastName} {FirstName} {FathersName}";
        }

        public string FullAddress()
        {
            if (Address != null)
                return $"{Address.City} {Address.Line.FirstOrDefault()}";
            else
                return "Нет адреса";
        }
    }
}
