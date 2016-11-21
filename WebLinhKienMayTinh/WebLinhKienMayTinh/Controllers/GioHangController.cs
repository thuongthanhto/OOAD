using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLinhKienMayTinh.Models;

namespace WebLinhKienMayTinh.Controllers
{
    public class GioHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        
        // Xem giỏ hàng
        public ActionResult XemGioHang()
        {
            // Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }

        // Lấy ra danh sách các sản phẩm trong giỏ hàng được lưu vào session
        public List<ItemGioHang> LayGioHang()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                // Nếu session giỏ hàng chưa tồn tại thì khởi tạo giỏ hàng
                lstGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }

        public ActionResult ThemGioHang(int MaSP, string strURL)
        {
            // Kiểm tra sản phẩm có tồn tại trong CSDL hay không
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                // Không tìm thấy sản phẩm
                Response.StatusCode = 404;
                return null;
            }
            // Lấy giỏ hàng từ session
            List<ItemGioHang> lstGioHang = LayGioHang();
            // Trường hợp nếu sản phẩm đã tồn tại trong giỏ hàng
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck != null)
            {
                // Kiểm tra số lượng tồn trước khi cho khách hàng mua hàng
                if (sp.SoLuongTon - spCheck.SoLuong < spCheck.SoLuong)
                {
                    // Hết sản phẩm có thể mua
                    return View("ThongBao"); 
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong*spCheck.DonGia;
                return Redirect(strURL);
            }
            
            ItemGioHang itemGH = new ItemGioHang(MaSP);
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                return View("ThongBao");
            }
            
            lstGioHang.Add(itemGH);
            return Redirect(strURL);
        }
        
        // Tính tổng số lượng
        public double TinhTongSoLuong()
        {
            // Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.SoLuong);
        }

        // Tính tổng tiền
        public decimal TinhTongTien()
        {
            // Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.ThanhTien);
        }
        // Partial view Giỏ hàng
        public ActionResult GioHangPartial()
        {
            if (TinhTongSoLuong() == 0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }
        public ActionResult SuaGioHang(int MaSP)
        {
            // Kiểm tra session giỏ hàng tồn tại chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index","Home");
            }
            // Kiểm tra sản phẩm có tồn tại trong CSDL hay không
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                // Báo không tìm thấy sản phẩm
                Response.StatusCode = 404;
                return null;
            }
            // Lấy list giỏ hàng từ session
            List < ItemGioHang > lstGioHang = LayGioHang();
            // Kiểm tra xem sản phẩm đó có tồn tại trong giỏ hàng hay không
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index","Home");
            }
            // Lấy list giỏ hàng gởi đến giao diện
            ViewBag.GioHang = lstGioHang;

            // Nếu tồn tại rồi 
            return View(spCheck);
        }

        //Xử lý khi nhấp nút cập nhật bên view sửa giỏ hàng
        [HttpPost]
        public ActionResult CapNhatGioHang(ItemGioHang itemGH)
        {
            // Kiểm tra số lượng tồn
            SanPham spCheck = db.SanPhams.Single(n => n.MaSP == itemGH.MaSP);
            if (spCheck.SoLuongTon < itemGH.SoLuong)
            {
                return View("ThongBao");
            }
            // Cập nhật số lượng trong session giỏ hàng
            // Bước 1: Lấy list giỏ hàng từ session giỏ hàng
            List<ItemGioHang> lstGH = LayGioHang();
            // Bước 2: Lấy sản phẩm cần cập nhật từ trong list giỏ hàng ra
            ItemGioHang itemGHUpdate = lstGH.Find(n => n.MaSP == itemGH.MaSP);
            // Bước 3: Tiến hành cập nhật lại số lượng cũng như thành tiền
            itemGHUpdate.SoLuong = itemGH.SoLuong;
            itemGHUpdate.ThanhTien = itemGHUpdate.SoLuong*itemGHUpdate.DonGia;
            return RedirectToAction("XemGioHang");
        }
        public ActionResult XoaGioHang(int MaSP)
        {
            // Kiểm tra session giỏ hàng tồn tại chưa
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // Kiểm tra sản phẩm có tồn tại trong CSDL hay không 
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                // Không tìm thấy sản phẩm trong csdl
                Response.StatusCode = 404;
                return null;
            }
            // Lấy list giỏ hàng từ session 
            List<ItemGioHang> lstGioHang = LayGioHang();
            // Kiểm tra xem sản phẩm đó có tồn tại trong giỏ hàng hay không
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // Xóa item trong giỏ hàng
            lstGioHang.Remove(spCheck);
            return RedirectToAction("XemGioHang");
            return View();
        }
        public ActionResult DatHang(KhachHang kh)
        {
            // Kiểm tra session giỏ hàng tồn tại chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index","Home");
            }
            KhachHang kHang = new KhachHang();
            if (Session["TaiKhoan"] == null)
            {
                // Thêm khách hàng vào bảng khách hàng đối với khác hàng vãng lai (kh chưa có tài khoản)
                kHang = kh;
                db.KhachHangs.Add(kHang);
                db.SaveChanges();
            }
            else
            {
                // Đối với khách hàng là thành viên
                ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
                kHang.TenKH = tv.HoTen;
                kHang.DiaChi = tv.DiaChi;
                kHang.Email = tv.Email;
                kHang.SoDienThoai = tv.SoDienThoai;
                kHang.MaThanhVien = tv.MaLoaiTV;
                db.KhachHangs.Add(kHang);
            }

            // Thêm đơn hàng
            DonDatHang ddh = new DonDatHang();
            ddh.MaKH = kHang.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            ddh.UuDai = 0;
            ddh.DaXoa = 0;
            ddh.DaHuy = 0;
            db.DonDatHangs.Add(ddh);
            db.SaveChanges();

            // Thêm chi tiết đơn đặt hagnf
            List<ItemGioHang> lstGH = LayGioHang();
            foreach (var item in lstGH)
            {
                ChiTietDonDatHang ctdh = new ChiTietDonDatHang();
                ctdh.MaDDH = ddh.MaDDH;
                ctdh.MaSP = item.MaSP;
                ctdh.TenSP = item.TenSP;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = item.DonGia;
                db.ChiTietDonDatHangs.Add(ctdh);
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang");
        }
    }
}