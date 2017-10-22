using EncryptPassword.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebLinhKienMayTinh.Models;

namespace WebLinhKienMayTinh.Controllers
{
    public class HomeController : Controller
    {
        private QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        /// <summary>
        /// Coder: Tô Thành Thương
        /// Trả về view trang Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            // Lần lượt tạo các viewbag để lấy list sản phẩm từ cơ sở dữ liệu
            // List Laptop mới
            var listLaptopMoi = db.SanPhams.Where(n => n.MaLoaiSP == 1 && n.Moi == true && n.DaXoa == false);
            ViewBag.listLapTopMoi = listLaptopMoi;
            // List PC mới
            var listPCMoi = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == true && n.DaXoa == false);
            ViewBag.listPCMoi = listPCMoi;
            // List Màn hình mới
            var listManHinhMoi = db.SanPhams.Where(n => n.MaLoaiSP == 3 && n.Moi == true && n.DaXoa == false);
            ViewBag.listManHinhMoi = listManHinhMoi;
            // List Phụ kiện mới
            var listPhuKienMoi = db.SanPhams.Where(n => (n.MaLoaiSP == 4 || n.MaLoaiSP == 5 || n.MaLoaiSP == 6) && n.Moi == true && n.DaXoa == false);
            ViewBag.listPhuKienMoi = listPhuKienMoi;
            return View();
        }

        /// <summary>
        /// Coder: Châu Ngọc Thái Sơn
        /// </summary>
        /// <returns></returns>
        public ActionResult MenuPartial()
        {
            // Truy vấn lấy về 1 list các sản phẩm là laptop, PC và màn hình LCD
            var lstSanPham1 = db.SanPhams.Where(n => n.MaLoaiSP == 1 || n.MaLoaiSP == 2 || n.MaLoaiSP == 3);
            ViewBag.lstSanPham1 = lstSanPham1;
            // Truy vấn lấy về 1 list các sản phẩm khác 3 loại trên
            var lstSanPham2 = db.SanPhams.Where(n => n.MaLoaiSP == 4 || n.MaLoaiSP == 5 || n.MaLoaiSP == 6);
            ViewBag.lstSanPham2 = lstSanPham2;
            return PartialView();
        }

        /// <summary>
        /// Coder: Trần Huy Thịnh
        /// Trả về trang đăng ký
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        /// <summary>
        /// Coder: Trần Huy Thịnh và Tô Thành Thương
        /// Xử lý dữ liệu gởi lên từ trang đăng ký
        /// </summary>
        /// <param name="tv"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DangKy(ThanhVien tv)
        {
            var tv1 = db.ThanhViens.FirstOrDefault(n => n.TaiKhoan == tv.TaiKhoan);
            if (tv1 == null)
            {
                tv.MaLoaiTV = 3;
                tv.DaXoa = false;
                // Mã hóa password
                tv.MatKhau = PasswordHelper.ComputeHash(tv.MatKhau, "MD5", GetBytes("Website"));
                // Thêm khách hàng vào csdl
                db.ThanhViens.Add(tv);
                db.SaveChanges();
                ViewBag.ThongBao = "Thêm thành công";
            }
            else
            {
                ViewBag.ThongBao = "Tên tài khoản đã tồn tại";
            }

            return View();
        }

        /// <summary>
        /// Coder: Tô Thành Thương
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// Coder: Tô Thành Thương
        /// Phân quyền cho người dùng
        /// </summary>
        /// <param name="TaiKhoan"></param>
        /// <param name="Quyen"></param>
        public void PhanQuyen(string TaiKhoan, string Quyen)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1,
                                          TaiKhoan, //user
                                          DateTime.Now, //Thời gian bắt đầu
                                          DateTime.Now.AddHours(1800000), //Thời gian kết thúc
                                          false, //Ghi nhớ?
                                          Quyen, // "DangKy,QuanLyDonHang,QuanLySanPham"
                                          FormsAuthentication.FormsCookiePath);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
            Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Coder: Tô Thành Thương
        /// Hàm xử lý đăng nhập
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            string taikhoan = f["txtTenDangNhap"].ToString();
            string matkhau = f["txtMatKhau"].ToString();
            //Truy vấn kiểm tra đăng nhập lấy thông tin thành viên
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == taikhoan);
            if (tv != null)
            {
                //if (PasswordHelper.VerifyHash(matkhau, "MD5", tv.MatKhau))
                //{
                if (tv.MatKhau == matkhau)
                {
                    //Lấy ra list quyền của thành viên tương ứng với loại thành viên
                    var lstQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == tv.MaLoaiTV);
                    //Duyệt list quyền
                    string Quyen = "";
                    if (lstQuyen.Count() != 0)
                    {
                        foreach (var item in lstQuyen)
                        {
                            Quyen += item.Quyen.MaQuyen + ",";
                        }
                        Quyen = Quyen.Substring(0, Quyen.Length - 1); //Cắt dấu ","
                        PhanQuyen(tv.TaiKhoan.ToString(), Quyen);
                    }
                    Session["TaiKhoan"] = tv;
                    return RedirectToAction("Index");
                }
            }
            return Content("Tài khoản hoặc mật khẩu không đúng!");
        }

        /// <summary>
        /// Coder: Tô Thành Thương
        /// Hàm xử lý đăng xuất
        /// </summary>
        /// <returns></returns>
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Coder: Tô Thành Thương
        /// Tạo 1 trang kế thừa từ AdminLayout
        /// </summary>
        /// <returns></returns>
        public ActionResult QuanTri()
        {
            return View();
        }

        /// <summary>
        /// Coder: Tô Thành Thương
        /// </summary>
        /// <returns></returns>
        public ActionResult MenuQuanTri()
        {
            // Lấy ra danh sách các quyền ứng với tài khoản
            if (Session["TaiKhoan"] != null)
            {
                ThanhVien tv = (ThanhVien)Session["TaiKhoan"];
                List<String> lstQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == tv.MaLoaiTV).OrderByDescending(n=>n.MaQuyen).Select(n => n.MaQuyen).ToList();
                ViewBag.lstQuyen = lstQuyen;
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
            return PartialView();
        }

        public ActionResult PermissionError()
        {
            return View();
        }
    }
}