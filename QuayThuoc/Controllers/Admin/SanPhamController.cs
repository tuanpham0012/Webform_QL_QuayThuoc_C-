using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuayThuoc.Models;

namespace QuayThuoc.Controllers.Admin
{
    public class SanPhamController : Controller
    {
        QLQuayThuocDBEntities db = new QLQuayThuocDBEntities();
        // GET: SanPham
        [HttpGet]
        public JsonResult Show(int pageIndex, String TimKiem, int Id_LoaiSp)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var pageSize = 9;
                var data = db.SanPhams.Join(db.LoaiSPs, sp => sp.Id_LoaiSP,
                                                        lsp => lsp.Id_LoaiSP,
                                                        (sp, lsp) => new
                                                        {
                                                            Id = sp.Id_SanPham,
                                                            MaSp = sp.MaSP,
                                                            TenSp = sp.TenSP,
                                                            Id_LoaiSp = sp.Id_LoaiSP,
                                                            TenLoaiSp = lsp.TenLoaiSP,
                                                            ThongTin = sp.ThongTin,
                                                            DonVi = sp.DonVi,
                                                            HinhAnh = sp.HinhAnh,
                                                            DaXoa = sp.DaXoa,
                                                        }).Where(sp=> sp.DaXoa == false).OrderByDescending(s => s.Id).ToList();
                if (TimKiem.Length > 0)
                {
                    data = data.Where(sp => sp.TenSp.Contains(TimKiem)).ToList();
                }
                if (Id_LoaiSp != -1)
                {
                    data = data.Where(sp => sp.Id_LoaiSp == Id_LoaiSp).ToList();
                }
                var result = data.Skip((pageIndex - 1) * pageSize).Take(pageSize);

                var pageTotal = (data.Count % pageSize == 0) ? data.Count / pageSize : (data.Count / pageSize) + 1;
                return Json(new { code = 200, data = result, soTrang = pageTotal, msg = "Lấy Danh Sách Sản Phẩm Thành Công" }, JsonRequestBehavior.AllowGet);
            }catch(Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy Danh Sách Sản Phẩm Thất Bại"+ex.Message }, JsonRequestBehavior.AllowGet);
            }
            
        }

        [HttpGet]
        public JsonResult Detail(int id)
        {
            try
            {
                var ds = db.SanPhams.Where(sp=> sp.Id_SanPham == id).Select(sp=> new
                          {
                              Id = sp.Id_SanPham,
                              Ten = sp.TenSP,
                              Ma = sp.MaSP,
                              LoaiSp = sp.Id_LoaiSP,
                              NhaCC = sp.Id_NhaCC,
                              ThongTin = sp.ThongTin,
                              DonVi = sp.DonVi,
                              HinhAnh = sp.HinhAnh,
                          }).Take(1).ToList();
                
                return Json(new { code = 200, data = ds[0], msg = "Lấy thông tin thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult UploadFiles(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Content/User_asset/images/"),
                                               Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            return Json(new { code = 200, msg = "Cập nhật thành công!" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Update(int Id, String MaSp, String TenSp, int Loaisp,int NhaCC, String ThongTin, String DonVi, String HinhAnh)
        {
            try
            {
                var ds = db.SanPhams.SingleOrDefault(s => s.Id_SanPham == Id);
                ds.MaSP = MaSp;
                ds.TenSP = TenSp;
                ds.Id_LoaiSP = Loaisp;
                ds.Id_NhaCC = NhaCC;
                ds.ThongTin = ThongTin;
                ds.DonVi = DonVi;
                ds.HinhAnh = HinhAnh;
                db.SaveChanges();

                return Json(new { code = 200, msg = "Cập nhật thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Cập nhật thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Create(String MaSp, String TenSp, int Loaisp, String ThongTin, String DonVi, String HinhAnh)
        {
            try
            {
                var ds = new SanPham();
                ds.MaSP = MaSp;
                ds.TenSP = TenSp;
                ds.Id_LoaiSP = Loaisp;
                ds.ThongTin = ThongTin;
                ds.DonVi = DonVi;
                ds.HinhAnh = HinhAnh;
                ds.DaXoa = false;
                db.SanPhams.Add(ds);
                db.SaveChanges();

                return Json(new { code = 200, msg = "Thêm mới thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm mới thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Delete(int Id)
        {
            try
            {
                var ds = db.SanPhams.SingleOrDefault(sp => sp.Id_SanPham == Id);
                ds.DaXoa = true;
                db.SaveChanges();

                return Json(new { code = 200, msg = "Xóa sản phẩm thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa sản phẩm thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}