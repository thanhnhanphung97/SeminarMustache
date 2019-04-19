using Demo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult LoadData()
        {
            var filePath = AppDomain.CurrentDomain.BaseDirectory + @"App_Data/dssv.json";
            using (StreamReader stream = new StreamReader(filePath))
            {
                string data = stream.ReadToEnd();
                List<SinhVien> listsinhvien = JsonConvert.DeserializeObject<List<SinhVien>>(data);
                List<string> date = new List<string>();
                foreach (var item in listsinhvien)
                {
                    string time = item.NgaySinh.ToString("yyyy-MM-dd");
                    date.Add(time);
                }
                return Json(new { data = listsinhvien, date = date }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult LoadDetail(int Id)
        {
            var filePath = AppDomain.CurrentDomain.BaseDirectory + @"App_Data/dssv.json";
            using (StreamReader stream = new StreamReader(filePath))
            {
                string data = stream.ReadToEnd();
                List<SinhVien> listsinhvien = JsonConvert.DeserializeObject<List<SinhVien>>(data);
                SinhVien sv = listsinhvien.SingleOrDefault(x => x.Id == Id);
                string date = sv.NgaySinh.ToString("yyyy-MM-dd");
                return Json(new { data = sv, date = date }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CreateEdit(string strSV)
        {
            var filePath = AppDomain.CurrentDomain.BaseDirectory + @"App_Data/dssv.json";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var sinhvien = serializer.Deserialize<SinhVien>(strSV);
            if (sinhvien.Id == 0)
            {
                try
                {
                    var jsondata = System.IO.File.ReadAllText(filePath);
                    var listsinhvien = JsonConvert.DeserializeObject<List<SinhVien>>(jsondata);
                    sinhvien.Id = listsinhvien.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
                    listsinhvien.Add(sinhvien);
                    jsondata = JsonConvert.SerializeObject(listsinhvien);
                    System.IO.File.WriteAllText(filePath, jsondata);
                    return Json(new { mes = "success" });
                }
                catch (Exception)
                {
                    return Json(new { mes = "fail" });
                }
            }
            if(sinhvien.Id != 0)
            {
                var jsondata = System.IO.File.ReadAllText(filePath);
                var listsinhvien = JsonConvert.DeserializeObject<List<SinhVien>>(jsondata);
                foreach (var item in listsinhvien)
                {
                    if (item.Id == sinhvien.Id)
                    {
                        item.HoTen = sinhvien.HoTen;
                        item.TenLop = sinhvien.TenLop;
                        item.HocBong = sinhvien.HocBong;
                        item.NgaySinh = sinhvien.NgaySinh;
                        item.GioiTinh = sinhvien.GioiTinh;
                    }
                }
                jsondata = JsonConvert.SerializeObject(listsinhvien);
                System.IO.File.WriteAllText(filePath, jsondata);
                return Json(new { mes = "success" });
            }
            else return Json(new { mes = "fail" });
        }
        [HttpPost]
        public JsonResult DeleteData(int Id)
        {
            var filePath = AppDomain.CurrentDomain.BaseDirectory + @"App_Data/dssv.json";
            var jsondata = System.IO.File.ReadAllText(filePath);
            var listsinhvien = JsonConvert.DeserializeObject<List<SinhVien>>(jsondata);
            var newList = new List<SinhVien>();
            foreach (var item in listsinhvien)
            {
                if (item.Id != Id) newList.Add(item);
            }
            jsondata = JsonConvert.SerializeObject(newList);
            System.IO.File.WriteAllText(filePath, jsondata);
            return Json(new { mes = "success" });
        }
    }
}