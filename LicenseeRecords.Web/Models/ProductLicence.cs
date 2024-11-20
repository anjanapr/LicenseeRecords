using System.Text.Json.Serialization;

namespace LicenseeRecords.Web.Models;

public class ProductLicence
{
    public int LicenceId { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Status LicenceStatus { get; set; }
    public DateTime? LicenceFromDate { get; set; }
    public DateTime? LicenceToDate { get; set; }
    public Product Product { get; set; }
}