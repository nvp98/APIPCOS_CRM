using System.ComponentModel.DataAnnotations;

namespace APIPCOS_CRM.Data
{
    public class HRC_Product
    {
        public string? ProductOrder { get; set; }
    public string ProductName { get; set; } = null!;
    public string? IDLayCoTinh { get; set; }
    public string? BilletLotname { get; set; }
    public bool IsSampled { get; set; }
    public string? SampleName { get; set; }
    public double? Tensile { get; set; }
    public double? Yeild { get; set; }
    public double? Elongation { get; set; }
    public double? HRB { get; set; }
    public double? ImpactEnergy { get; set; }
    public string? SAPCode { get; set; }
    public string? SAPDescription { get; set; }
    public string? ShiftName { get; set; }
    public DateOnly ProductionDate { get; set; }
    public string? WorkshopName { get; set; }
    public string? ProductLotName { get; set; }
    public double? Length { get; set; }
    public double? Weight { get; set; }
    public string? IsReturn { get; set; }
    public string? Nhom { get; set; }
    public string? Loai { get; set; }
    public string? InputProductName { get; set; }
    public string? ID_GOC { get; set; }
    public string? BilletGradeCode { get; set; }
    public double? C { get; set; }
    public double? Si { get; set; }
    public double? Mn { get; set; }
    public double? S { get; set; }
    public double? P { get; set; }
    public double? Cu { get; set; }
    public double? Ni { get; set; }
    public double? Cr { get; set; }
    public double? Mo { get; set; }
    public double? CEV { get; set; }
    public double? V { get; set; }
    public double? Ti { get; set; }
    public double? Al { get; set; }
    public double? B { get; set; }
    public double? Ca { get; set; }
    public double? O { get; set; }
    public double? N { get; set; }
    public double? H { get; set; }
    public double? Nb { get; set; }
    public double? Al_sol { get; set; }
    public double? Al_ins { get; set; }

    public string? ClassifyName { get; set; }
    public string? ChemicalDetail { get; set; }
    }
}
