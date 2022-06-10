using QuayThuoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuayThuoc.Controllers.Admin
{
    public class DoanhThuController : Controller
    {
        QLQuayThuocDBEntities db = new QLQuayThuocDBEntities();
        [HttpGet]
        public JsonResult LayDoanhThu(int pageIndex, DateTime TimTheoNgay, DateTime TimTheoThang, Boolean Ngay, Boolean Thang)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                var DBDoanhThu = db.DoanhThus.OrderByDescending(dt => dt.Ngay).ToList();
                if (Ngay)
                {
                    DBDoanhThu = DBDoanhThu.Where(dt => dt.Ngay == TimTheoNgay).ToList();
                }
                if (Thang)
                {
                    DateTime month = TimTheoThang.AddMonths(1);
                    DBDoanhThu = DBDoanhThu.Where(hd => hd.Ngay < month && hd.Ngay > TimTheoThang).ToList();
                }
                var pageSize = 15;
                var soTrang = (DBDoanhThu.Count() % pageSize == 0) ? DBDoanhThu.Count() / pageSize : (DBDoanhThu.Count() / pageSize) + 1;

                DBDoanhThu = DBDoanhThu.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return Json(new { code = 200, data = DBDoanhThu, soTrang = soTrang, msg = "Lấy dữ liệu thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy dữ liệu thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}