using System;

namespace Web_JbcHrms.Models;

public class Asset
{
    public required string AssetTag { get; set; }
    public required string Type { get; set; }
    public string? SerialNumber { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
    public int? AssignedEmployeeID { get; set; }
    public string? AssignedTeamName { get; set; }
    public string? ResponsiblePerson { get; set; }
    public string? Model { get; set; }
    public string? Config { get; set; }
    public bool IsDeleted { get; set; }
    public List<AssetHistory> History { get; set; } = new();
}