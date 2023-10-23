using Microsoft.EntityFrameworkCore;
using Hl7.Fhir.Model;

namespace FHIR_MIS_webview
{
  public class ApplicationDBContext : DbContext
  {
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
    {
    }
    DbSet<Patient> Patinets;


  }
}
