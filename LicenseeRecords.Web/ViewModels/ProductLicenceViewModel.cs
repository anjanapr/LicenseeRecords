using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using LicenseeRecords.Web.Models;

namespace LicenseeRecords.Web.ViewModels;

public class ProductLicenceViewModel
{
    public int AccountId { get; set; }
    [Required]
    public string AccountName { get; set; }
    public Status AccountStatus { get; set; }
    public required ProductLicence ProductLicence { get; set; }
    public List<Product> Products { get; set; } = new List<Product>();
}