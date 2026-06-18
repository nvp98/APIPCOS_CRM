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

        // PhieuXuatHang_HRC.GradeCode
        public string? HPDQ_Grade__c { get; set; }

        // HRC_ProductRequestDto.CustomerCode
        public string? HPDQ_SAP_Customer_Code__c { get; set; }

        // PhieuXuatHang_HRC.StandardCode
        public string? HPDQ_Standard__c { get; set; }

        // PhieuXuatHang_HRC.SO + "-" + PurchaseOrderCode
        public string? HPDQ_Contract__c { get; set; }

        // Sum of PhieuXuatHang_HRC.Weight
        public double HPDQ_Total_Weight__c { get; set; }

        // HRC_ProductRequestDto.ListID.Count
        public int HPDQ_Total_Coils__c { get; set; }

        // Default cố định
        public string HPDQ_Configuration__c { get; set; } = "C;Si;Mn;S;P;Cu;Ni;Cr;Mo;V;Ti;Al;B;CA;CEV";

        public object? HPDQ_Data__c { get; set; }
    }
}
