namespace APIPCOS_CRM.Models
{
    // Cặp mác thép <-> tiêu chuẩn, dựng từ PhieuXuatHang_HRC (GradeCode + StandardCode).
    // Dành cho FE render bảng; luồng đẩy Salesforce vẫn dùng HPDQ_Grade__c / HPDQ_Standard__c.
    public class HRC_GradeStandardDto
    {
        // PhieuXuatHang_HRC.GradeCode (đã Trim)
        public string Grade { get; set; } = "";

        // Các PhieuXuatHang_HRC.StandardCode distinct của riêng mác này.
        // Rỗng nếu mác không có tiêu chuẩn nào.
        public List<string> Standards { get; set; } = new();
    }
}
