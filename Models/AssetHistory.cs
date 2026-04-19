using System;

namespace Web_JbcHrms.Models;

public class AssetHistory
{
    public DateTime Date { get; set; } = DateTime.Now;
    public required string Action { get; set; }
    public string? Notes { get; set; }
}