using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebLinhKienMayTinh.Models
{
    [MetadataType(typeof(ThanhVienMetadata))]
    public partial class ThanhVien
    {
        internal sealed class ThanhVienMetadata
        {
            public int MaThanhVien { get; set; }

            [DisplayName("Tài khoản")]
            [Required(ErrorMessage = "{0} không được bỏ trống")]
            [RegularExpression(@"^\w+[\w.-_\S]$", ErrorMessage = "Tài khoản phải bắt đầu bằng chữ số hoặc chữ cái")]
            [StringLength(50, MinimumLength = 8, ErrorMessage = "Tài khoản phải chứa ít nhất 8 ký tự")]
            public string TaiKhoan { get; set; }

            [DisplayName("Mật khẩu")]
            [Required(ErrorMessage = "{0} không được bỏ trống", AllowEmptyStrings = false)]
            [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", ErrorMessage = "Mật khẩu ít nhất 8 ký tự, mật khẩu phải chứa ít nhất 1 ký tự in hoa và 1 chữ thường.")]
            //[StringLength(50, MinimumLength = 8, ErrorMessage = "Mật khẩu ít nhất 8 ký tự")]
            public string MatKhau { get; set; }

            [DisplayName("Nhập lại mật khẩu")]
            [Compare("MatKhau", ErrorMessage = "Mật khẩu nhập không trùng khớp")]
            [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
            public string NhapLaiMatKhau { get; set; }

            [DisplayName("Họ tên")]
            [Required(ErrorMessage = "{0} không được bỏ trống")]
            public string HoTen { get; set; }

            [DisplayName("Địa chỉ")]
            [Required(ErrorMessage = "{0} không được bỏ trống")]
            [RegularExpression(@"^[\w,-_.\s]+$", ErrorMessage = "Địa chỉ không hợp lệ")]
            public string DiaChi { get; set; }

            [DisplayName("Email")]
            [Required(ErrorMessage = "{0} không được bỏ trống")]
            [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$", ErrorMessage = "Email không hợp lệ")]
            public string Email { get; set; }

            [DisplayName("Số điện thoại")]
            [DataType(DataType.PhoneNumber)]
            [Required(ErrorMessage = "{0} không được bỏ trống")]
            [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
            public string SoDienThoai { get; set; }

            [DisplayName("Câu hỏi")]
            public string CauHoi { get; set; }

            [DisplayName("Câu trả lời")]
            [Required(ErrorMessage = "{0} không được bỏ trống")]
            public string CauTraLoi { get; set; }
            public Nullable<int> MaLoaiTV { get; set; }
        }
    }
}