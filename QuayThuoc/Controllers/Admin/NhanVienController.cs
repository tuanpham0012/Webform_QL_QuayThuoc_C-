using QuayThuoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuayThuoc.Controllers.Admin
{
    public class NhanVienController : Controller
    {
        QLQuayThuocDBEntities db = new QLQuayThuocDBEntities();
        DungChung dc = new DungChung();
        [HttpGet]
        public JsonResult Show(int pageIndex, String TimKiem, int Id_ChucVu)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var pageSize = 9;
                var DBNhanVien = db.NhanViens.Join(db.ChucVus, nv => nv.Id_ChucVu,
                                                        cv => cv.Id_ChucVu,
                                                        (nv, cv) => new
                                                        {
                                                            Id = nv.Id_NhanVien,
                                                            TenNv = nv.TenNV,
                                                            Email = nv.Email,
                                                            MaNv = nv.MaNV,
                                                            GioiTinh = nv.GioiTinh,
                                                            NgaySinh = nv.NgaySinh,
                                                            Id_ChucVu = nv.Id_ChucVu,
                                                            ChucVu = cv.TenChucVu,
                                                            DaXoa = nv.DaXoa,
                                                        }).Where(nv => nv.DaXoa == false && nv.Id_ChucVu != 1).OrderByDescending(s => s.Id).ToList();
                if (TimKiem.Length > 0)
                {
                    DBNhanVien = DBNhanVien.Where(nv => nv.TenNv.Contains(TimKiem)).ToList();
                }
                if (Id_ChucVu != -1)
                {
                    DBNhanVien = DBNhanVien.Where(nv => nv.Id_ChucVu == Id_ChucVu).ToList();
                }
                if(Id_ChucVu != 5)
                {
                    DBNhanVien = DBNhanVien.Where(nv => nv.Id_ChucVu != 5).ToList();
                }
                DBNhanVien = DBNhanVien.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                var pageTotal = (DBNhanVien.Count % pageSize == 0) ? DBNhanVien.Count / pageSize : (DBNhanVien.Count / pageSize) + 1;
                return Json(new { code = 200, data = DBNhanVien, soTrang = pageTotal, msg = "Lấy Danh Sách Thành Công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy Danh Sách Thất Bại" + ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult Detail(int id)
        {
            try
            {
                var ds = db.NhanViens.Where(nv => nv.Id_NhanVien == id).Select(nv => new
                {
                    Id = nv.Id_NhanVien,
                    MaNv = nv.MaNV,
                    TenNv = nv.TenNV,
                    Email = nv.Email,
                    DiaChi = nv.DiaChi,
                    LienHe = nv.LienHe,
                    GioiTinh = nv.GioiTinh,
                    NgaySinh = nv.NgaySinh,
                    CMND = nv.CMND,
                    Id_ChucVu = nv.Id_ChucVu,
                    NgayTao = nv.created_at,
                    NgaySua = nv.update_at,
                    DangNhapCuoi = nv.last_login,
                }).FirstOrDefault();

                return Json(new { code = 200, data = ds, msg = "Lấy thông tin thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult Update(int Id, String TenNv,String Email, String DiaChi, String LienHe, String GioiTinh, DateTime NgaySinh, String CMND, int Id_ChucVu)
        {
            try
            {
                var ds = db.NhanViens.Find(Id);
                if(ds != null)
                {
                    DateTime day = DateTime.Now;
                    ds.TenNV = TenNv;
                    ds.Email = Email;
                    ds.DiaChi = DiaChi;
                    ds.LienHe = LienHe;
                    ds.GioiTinh = GioiTinh;
                    ds.NgaySinh = NgaySinh;
                    ds.CMND = CMND;
                    if(Id_ChucVu != -1)
                    {
                        ds.Id_ChucVu = Id_ChucVu;
                    }
                    
                    ds.update_at = day;
                    db.SaveChanges();
                }
                
                return Json(new { code = 200, msg = "Cập nhật thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Cập nhật thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Create(String TenNv, String Email, String Password, String DiaChi, String LienHe, String GioiTinh, DateTime NgaySinh, String CMND, int Id_ChucVu, Boolean user)
        {
            try
            {
                var check = db.NhanViens.ToList();
                foreach(var item in check)
                {
                    if (item.Email.Equals(Email))
                    {
                        return Json(new { code = 202, msg = "Email đã được sử dụng!" }, JsonRequestBehavior.AllowGet);
                    }
                    if (item.CMND.Equals(CMND))
                    {
                        return Json(new { code = 202, msg = "CMND đã được sử dụng!" }, JsonRequestBehavior.AllowGet);
                    }
                    if (item.LienHe.Equals(LienHe))
                    {
                        return Json(new { code = 202, msg = "Số điện thoại đã được sử dụng!" }, JsonRequestBehavior.AllowGet);
                    }
                }

                var ds = new NhanVien();
                DateTime day = DateTime.Now;

                var nv = db.NhanViens.OrderByDescending(n => n.Id_NhanVien).FirstOrDefault();
                var stt = 1;
                if(nv != null)
                {
                    stt = nv.Id_NhanVien;
                }

                var Ma = "";
                if (user)
                {
                    Ma = dc.CreateMaHD("User", stt);
                }
                else
                {
                    Ma = dc.CreateMaHD("NV", stt);
                }
                ds.MaNV = Ma;
                ds.TenNV = TenNv;
                ds.Email = Email;
                ds.DiaChi = DiaChi;
                ds.Password = dc.EncodePassword(Password);
                ds.LienHe = LienHe;
                ds.GioiTinh = GioiTinh;
                ds.NgaySinh = NgaySinh;
                ds.CMND = CMND;
                ds.Id_ChucVu = Id_ChucVu;
                ds.created_at = day;
                ds.update_at = day;
                ds.DaXoa = false;
                db.NhanViens.Add(ds);
                db.SaveChanges();
                if (user)
                {
                    return Json(new { code = 200, msg = "Đăng kí thành công!" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { code = 200, msg = "Thêm nhân viên thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { msg = "Thêm nhân viên thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Delete(int Id)
        {
            try
            {
                var ds = db.NhanViens.Find(Id);
                if(ds.Id_ChucVu <= 2)
                {
                    return Json(new { code = 202, msg = "Không thể xóa người dùng này!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ds.DaXoa = true;
                    db.SaveChanges();

                    return Json(new { code = 200, msg = "Xóa người dùng thành công!" }, JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa người dùng thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ChangePassword(int Id, String Email, String PasswordConfirm, String Password)
        {
            try
            {
                var UserDB = db.NhanViens.Find(Id);
                if(UserDB != null)
                {
                    if (!UserDB.Email.Equals(Email))
                    {
                        return Json(new { code = 500, msg = "Email không tồn tại!"}, JsonRequestBehavior.AllowGet);
                    }
                    if (UserDB.Email.Equals(Email) && !UserDB.Password.Equals(dc.EncodePassword(PasswordConfirm)))
                    {
                        return Json(new { code = 500, msg = "Sai mật khẩu!" }, JsonRequestBehavior.AllowGet);
                    }
                    if(UserDB.Email.Equals(Email) && UserDB.Password.Equals(dc.EncodePassword(PasswordConfirm)))
                    {
                        UserDB.Password = dc.EncodePassword(Password);
                        db.SaveChanges();
                        return Json(new { code = 200, msg = "Đổi mật khẩu thành công!" }, JsonRequestBehavior.AllowGet);
                    }
                    
                }
                return Json(new { code = 500, msg = "Lỗi đổi mật khẩu!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lỗi đổi mật khẩu!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}