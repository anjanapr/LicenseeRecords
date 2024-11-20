using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LicenseeRecords.Web.Models;

public class Account
{
    public int AccountId { get; set; }
    [Required]
    public string AccountName { get; set; } = string.Empty;
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Status AccountStatus { get; set; }
    public ICollection<ProductLicence> ProductLicence { get; set; } = new List<ProductLicence>();
}