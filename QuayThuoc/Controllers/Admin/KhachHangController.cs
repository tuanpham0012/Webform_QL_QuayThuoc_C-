using QuayThuoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuayThuoc.Controllers.Admin
{
    
    public class KhachHangController : Controller
    {
        QLQuayThuocDBEntities db = new QLQuayThuocDBEntities();
        [HttpGet]
        public JsonResult Show(int pageIndex, String TimKiem, int Id_LoaiKh)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var pageSize = 9;
                var DBKhachHang = db.KhachHangs.Join(db.LoaiKHs, kh => kh.Id_LoaiKH,
                                                        lsp => lsp.Id_LoaiKH,
                                                        (kh, lkh) => new
                                                        {
                                                            Id = kh.Id_KhachHang,
                                                            TenKh = kh.TenKhachHang,
                                                            DiaChi = kh.DiaChi,
                                                            LienHe = kh.LienHe,
                                                            Id_LoaiKh = kh.Id_LoaiKH,
                                                            LoaiKh = lkh.TenLoaiKH,
                                                            DaXoa = kh.DaXoa,
                                                        }).Where(kh => kh.DaXoa == false).OrderByDescending(s => s.Id).ToList();
                if (TimKiem.Length > 0)
                {
                    DBKhachHang = DBKhachHang.Where(sp => sp.TenKh.Contains(TimKiem)).ToList();
                }
                if (Id_LoaiKh != -1)
                {
                    DBKhachHang = DBKhachHang.Where(sp => sp.Id_LoaiKh == Id_LoaiKh).ToList();
                }
                DBKhachHang = DBKhachHang.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                var pageTotal = (DBKhachHang.Count % pageSize == 0) ? DBKhachHang.Count / pageSize : (DBKhachHang.Count / pageSize) + 1;
                return Json(new { code = 200, data = DBKhachHang, soTrang = pageTotal, msg = "Lấy Danh Sách Thành Công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy Danh Sách Thất Bại" + ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult Detail(int id)
        {
            try
            {
                var ds = db.KhachHangs.Where(sp => sp.Id_KhachHang == id).Select(sp => new
                {
                    Id = sp.Id_KhachHang,
                    Ten = sp.TenKhachHang,
                    DiaChi = sp.DiaChi,
                    LienHe = sp.LienHe,
                    Id_LoaiKh = sp.Id_KhachHang,
                }).FirstOrDefault();

                return Json(new { code = 200, data = ds, msg = "Lấy thông tin thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult Update(int Id, String TenKh, String DiaChi, String LienHe, int Id_LoaiKh)
        {
            try
            {
                var ds = db.KhachHangs.Find(Id);
                ds.TenKhachHang = TenKh;
                ds.DiaChi = DiaChi;
                ds.LienHe = LienHe;
                ds.Id_LoaiKH = Id_LoaiKh;
                db.SaveChanges();

                return Json(new { code = 200, msg = "Cập nhật thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Cập nhật thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Delete(int Id)
        {
            try
            {
                var ds = db.KhachHangs.Find(Id);
                ds.DaXoa = true;
                db.SaveChanges();

                return Json(new { code = 200, msg = "Xóa khách hàng thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa khách hàng thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}