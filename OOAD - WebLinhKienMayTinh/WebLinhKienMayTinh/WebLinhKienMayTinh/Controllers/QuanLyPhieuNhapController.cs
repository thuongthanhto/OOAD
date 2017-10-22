﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLinhKienMayTinh.Models;

namespace WebLinhKienMayTinh.Controllers
{
    public class QuanLyPhieuNhapController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        
        /// <summary>
        /// Coder: Châu Ngọc Thái Sơn
        /// Trả về trang nhập hàng
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NhapHang()
        {
            ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.ListSanPham = db.SanPhams.OrderBy(n=>n.TenSP);
            return View();
        }

        /// <summary>
        /// Coder: Châu Ngọc Thái Sơn
        /// Xử lý nhập hàng
        /// </summary>
        /// <param name="model"></param>
        /// <param name="lstModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NhapHang(PhieuNhap model, IEnumerable<ChiTietPhieuNhap> lstModel)
        {
            ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.ListSanPham = db.SanPhams;
            // sau khi đã kiểm tra các dữ liệu đầu vào
            model.DaXoa = false; // gán đã xóa: false
            db.PhieuNhaps.Add(model);
            db.SaveChanges();
            SanPham sp;
            foreach (var item in lstModel)
            {
                //cập nhật số lượng tồn của từng sp 
                sp = db.SanPhams.Single(n => n.MaSP == item.MaSP);
                sp.SoLuongTon += item.SoLuongNhap;

                item.MaPN = model.MaPN; //gán mã phiếu nhập cho tất cả các chi tiết phiếu nhập
            }
            db.ChiTietPhieuNhaps.AddRange(lstModel);
            db.SaveChanges();
            ViewBag.ThongBao = "Đã nhập hàng thành công";

            return View();
        }

        /// <summary>
        /// Coder: Trần Huy Thịnh
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DSSPHetHang()
        {
            //danh sách sản phẩm gần hết hàng
            var lstSP = db.SanPhams.Where(n => n.SoLuongTon <= 5 && n.DaXoa == false);
            return View(lstSP);
        }

        /// <summary>
        /// Coder: Trần Huy Thịnh
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpGet]
        public ActionResult NhapHangDon(int? id)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            //Tương tự như trang chỉnh sửa sản phẩm nhưng ta chỉ hiển thị những thông tin cần thiết.
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            return View(sp);
        }

        // Xử lý nhập hàng từng sản phẩm
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult NhapHangDon(PhieuNhap pn, ChiTietPhieuNhap ctpn, HttpPostedFileBase HinhAnh)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", pn.MaNCC);
            if (HinhAnh != null)
            {
                //Kiểm tra nội dung hình ảnh
                if (HinhAnh.ContentLength > 0)
                {
                    //Lấy tên hình ảnh
                    var fileName = Path.GetFileName(HinhAnh.FileName);
                    //Lấy hình ảnh chuyển vào thư mục hình ảnh 
                    var path = Path.Combine(Server.MapPath("~/Content/images/sanpham"), fileName);
                    //Nếu thư mục chứa hình ảnh đó rồi thì xuất ra thông báo
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    HinhAnh.SaveAs(path);
                }
            }
            //Sau khi các bạn đã kiểm tra tất cả dữ liệu đầu vào
            //Gán đã xóa: False
            pn.NgayNhap = DateTime.Now;
            pn.DaXoa = false;
            db.PhieuNhaps.Add(pn);
            db.SaveChanges();
            //Lấy mã phiếu nhập gán cho lstChiTietPhieuNhap
            ctpn.MaPN = pn.MaPN;
            //Xử lý cập nhật số lượng tồn
            SanPham sp = db.SanPhams.Single(n => n.MaSP == ctpn.MaSP);
            sp.SoLuongTon += ctpn.SoLuongNhap;
            sp.DonGia = (ctpn.DonGiaNhap + (ctpn.DonGiaNhap * 5) / 100);
            db.ChiTietPhieuNhaps.Add(ctpn);

            db.Set<SanPham>().AddOrUpdate(sp);
            db.SaveChanges();
            return RedirectToAction("DSSPHetHang");
        }

        //Giải phóng biến cho vùng nhớ
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                    db.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}