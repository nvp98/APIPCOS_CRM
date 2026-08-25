namespace APIPCOS_CRM.Models
{
    public class HRC_CertificateResponseDto
    {
        // Default
        public string Name { get; set; }

        // null - chờ logic điền
        public string? HPDQ_Certificate_No__c { get; set; }

        // PhieuXuatHang_HRC.IssueDate
        public string? HPDQ_Issue_Date__c { get; set; }

        // PhieuXuatHang_HRC.PartnerName
        public string? HPDQ_Project__c { get; set; }

        // Default
        public string HPDQ_Product__c { get; set; } = "THÉP CUỘN CÁN NÓNG/HOT ROLLED COIL";

        // Distinct PhieuXuatHang_HRC.GradeCode (mác thành phẩm) của phieuXuatList
        public List<string> HPDQ_Grade__c { get; set; } = new();

        // HRC_ProductRequestDto.CustomerCode
        public string? HPDQ_SAP_Customer_Code__c { get; set; }

        // PhieuXuatHang_HRC.StandardCode gom theo từng mác thép.
        // Cùng số phần tử & cùng thứ tự với HPDQ_Grade__c: HPDQ_Standard__c[i] là tiêu chuẩn của HPDQ_Grade__c[i].
        // Mác không có tiêu chuẩn -> chuỗi rỗng (giữ đúng index).
        public List<string> HPDQ_Standard__c { get; set; } = new();

        // Cặp mác thép <-> tiêu chuẩn cho FE render bảng (mỗi mác kèm list tiêu chuẩn riêng).
        // Cùng nguồn & cùng thứ tự với HPDQ_Grade__c / HPDQ_Standard__c, chỉ khác cách đóng gói.
        public List<HRC_GradeStandardDto> HPDQ_Grade_Standards { get; set; } = new();

        // PhieuXuatHang_HRC.SO + "-" + PurchaseOrderCode
        public string? HPDQ_Contract__c { get; set; }

        // Sum of PhieuXuatHang_HRC.Weight
        public double HPDQ_Total_Weight__c { get; set; }

        // HRC_ProductRequestDto.ListID.Count
        public int HPDQ_Total_Coils__c { get; set; }

        // Default cố định
        public string HPDQ_Configuration__c { get; set; } = "C;Si;Mn;S;P;Cu;Ni;Cr;Mo;V;Ti;Al;B;CA;CEV";
        public List<string>? HPDQ_SO { get; set; }
        public string? HPDQ_Transport { get; set; }

        public object? HPDQ_Data__c { get; set; }
        public string? EndUser { get; set; } = null!;
    }
}
