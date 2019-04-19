using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.Models
{
    public class SinhVien
    {
        public int Id { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string TenLop { get; set; }
        public string GioiTinh { get; set; }
        public int HocBong { get; set; }
    }
}