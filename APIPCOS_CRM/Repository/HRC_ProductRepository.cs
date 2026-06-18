using APIPCOS_CRM.Data;
using APIPCOS_CRM.Models;
using Microsoft.EntityFrameworkCore;

namespace APIPCOS_CRM.Repository
{
    public class HRC_ProductRepository
    {
        private readonly Bkmis11_Context _bkmis11;
        private readonly Bkmis13_Context _bkmis13;

        private static double? ScaleAndRound(double? value, double factor)
            => value.HasValue ? Math.Round(value.Value * factor, MidpointRounding.AwayFromZero) : null;

        // x100: C, Si, Mn, CEV | x1000: S, P, Cu, Ni, Cr, Mo | round only: V, Ti, Al, B, Ca
        private static readonly Dictionary<string, Func<HRC_Product, double?>> _chemicalMap =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["C"]   = p => ScaleAndRound(p.C,   100),
                ["Si"]  = p => ScaleAndRound(p.Si,  100),
                ["Mn"]  = p => ScaleAndRound(p.Mn,  100),
                ["CEV"] = p => ScaleAndRound(p.CEV, 100),
                ["S"]   = p => ScaleAndRound(p.S,   1000),
                ["P"]   = p => ScaleAndRound(p.P,   1000),
                ["Cu"]  = p => ScaleAndRound(p.Cu,  1000),
                ["Ni"]  = p => ScaleAndRound(p.Ni,  1000),
                ["Cr"]  = p => ScaleAndRound(p.Cr,  1000),
                ["Mo"]  = p => ScaleAndRound(p.Mo,  1000),
                ["V"]   = p => ScaleAndRound(p.V,   1),
                ["Ti"]  = p => ScaleAndRound(p.Ti,  1),
                ["Al"]  = p => ScaleAndRound(p.Al,  1),
                ["B"]   = p => ScaleAndRound(p.B,   1),
                ["CA"]  = p => ScaleAndRound(p.Ca,  1),
            };

        private const string DefaultConfig = "C;Si;Mn;S;P;Cu;Ni;Cr;Mo;V;Ti;Al;B;CA;CEV";

        public HRC_ProductRepository(Bkmis11_Context bkmis11, Bkmis13_Context bkmis13)
        {
            _bkmis11 = bkmis11;
            _bkmis13 = bkmis13;
        }

        public async Task<HRC_GetDataResult> GetDataAsync(HRC_ProductRequestDto request)
        {
            // 1. Lấy tất cả ProductName thuộc SO + Transporter
            var productNamesInSO = await GetProductNamesBySOAsync(request);

            // 2. Phân loại ListID: valid (có trong SO) vs alert (không có trong SO)
            var validIDs = request.ListID.Where(id => productNamesInSO.Contains(id)).ToList();
            var alertIDs = request.ListID.Where(id => !productNamesInSO.Contains(id)).ToList();

            // 3. Query PhieuXuatHang theo validIDs → dùng cho header + tính tổng Weight
            var phieuXuatList = await GetPhieuXuatHangByValidIDsAsync(request, validIDs);

            // 4. Query HRC_Product theo validIDs → dùng cho HPDQ_Data__c
            var productList = await GetHRCProductsAsync(validIDs);

            // 5. Build response
            var certificate = BuildResponse(request, phieuXuatList, productList);

            return new HRC_GetDataResult
            {
                Certificate = certificate,
                AlertIDs    = alertIDs
            };
        }

        // Lấy tất cả ProductName thuộc SO + filter Transporter LIKE
        private async Task<HashSet<string>> GetProductNamesBySOAsync(HRC_ProductRequestDto request)
        {
            var query = _bkmis13.PhieuXuatHang_HRCs
                .Where(p => p.SO == request.SO && p.ProductName != null);

            query = query.Where(p => EF.Functions.Like(p.Transporter, $"%{request.Transporter}%"));

            var names = await query
                .Select(p => p.ProductName!)
                .Distinct()
                .ToListAsync();

            return names.ToHashSet();
        }

        // Lấy PhieuXuatHang theo SO + validIDs + Transporter LIKE
        private async Task<List<PhieuXuatHang_HRC>> GetPhieuXuatHangByValidIDsAsync(
            HRC_ProductRequestDto request,
            List<string> validIDs)
        {
            if (!validIDs.Any())
                return new List<PhieuXuatHang_HRC>();

            var query = _bkmis13.PhieuXuatHang_HRCs
                .Where(p => p.SO == request.SO
                         && p.ProductName != null
                         && validIDs.Contains(p.ProductName));

            query = query.Where(p => EF.Functions.Like(p.Transporter, $"%{request.Transporter}%"));

            return await query.ToListAsync();
        }

        // Lấy HRC_Product theo validIDs
        private async Task<List<HRC_Product>> GetHRCProductsAsync(List<string> validIDs)
        {
            if (!validIDs.Any())
                return new List<HRC_Product>();

            return await _bkmis11.HRC_Products
                .Where(p => validIDs.Contains(p.ProductName))
                .ToListAsync();
        }

        private HRC_CertificateResponseDto BuildResponse(
            HRC_ProductRequestDto request,
            List<PhieuXuatHang_HRC> phieuXuatList,
            List<HRC_Product> productList)
        {
            var first      = phieuXuatList.FirstOrDefault();
            var configKeys = DefaultConfig.Split(';');

            // Build HPDQ_Data__c: mỗi HRC_Product → 1 Dictionary, chỉ trả đúng thành phần trong config
            var dataItems = productList.Select((p, index) =>
            {
                var obj = new Dictionary<string, object?>
                {
                    ["stt"]                  = index + 1,
                    ["coil_no"]              = p.ProductLotName,
                    ["thickness_mm"]         = p.Thick,
                    ["width_mm"]             = p.Width, 
                    ["length"]               = p.Length,
                    ["weight_kg"]            = p.Weight,
                    ["heat_no"]              = p.BilletLotname,
                    ["yield_strength_MPa"]   = p.Yeild,
                    ["tensile_strength_MPa"] = p.Tensile,
                    ["elongation_pct"]       = p.Elongation,
                    ["hardness_HRB"]         = p.HRB,
                    ["bending_test"]         = p.BendTest,
                };

                foreach (var key in configKeys)
                {
                    double? val = _chemicalMap.TryGetValue(key, out var getter) ? getter(p) : null;
                    obj[key] = val;
                }

                obj["remarks"] = "";
                return obj;
            }).ToList();

            return new HRC_CertificateResponseDto
            {
                HPDQ_Certificate_No__c    = $"0000-07{DateTime.Now:MMyy}/HOAPHAT",
                HPDQ_Issue_Date__c        = first?.IssueDate,
                HPDQ_Project__c           = first?.PartnerName,
                HPDQ_Grade__c             = first?.GradeCode,
                HPDQ_SAP_Customer_Code__c = request.CustomerCode,
                HPDQ_Standard__c          = first?.StandardCode,
                HPDQ_Contract__c          = first != null ? $"{first.SO} - {first.PurchaseOrderCode}" : null,
                HPDQ_Total_Weight__c      = phieuXuatList.Sum(p => p.Weight ?? 0),
                HPDQ_Total_Coils__c       = request.ListID.Count,
                HPDQ_Configuration__c     = DefaultConfig,
                HPDQ_Data__c              = dataItems
            };
        }
    }
}
