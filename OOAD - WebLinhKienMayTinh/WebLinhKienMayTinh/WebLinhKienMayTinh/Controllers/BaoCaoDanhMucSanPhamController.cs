using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using PagedList;
using WebLinhKienMayTinh.Models;

namespace WebLinhKienMayTinh.Controllers
{
    public class BaoCaoDanhMucSanPhamController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: BaoCaoDanhMucSanPham
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var model = db.SanPhams.OrderBy(x => x.MaSP).ToPagedList(page, pageSize);
            return View(model);
        }

        public ActionResult ExportExel()
        {
            GridView gv = new GridView();
            var list = db.SanPhams.Select(p => new
            {
                MaSP = p.MaSP,
                TenSP = p.TenSP,
                LoaiSanPham = p.LoaiSanPham.TenLoai,
                SoLuongTon = p.SoLuongTon,
                DonGia = p.DonGia,
                NhaCungCap = p.NhaCungCap.TenNCC,
                NhaSanXuat = p.NhaSanXuat.TenNSX,
            }).ToList();
            gv.DataSource = list;

            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=ProductReport.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("Index");
        }
    }
}