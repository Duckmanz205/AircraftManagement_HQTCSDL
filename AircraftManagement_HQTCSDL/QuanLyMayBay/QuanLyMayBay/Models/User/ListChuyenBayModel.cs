using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyMayBay.Models
{
    public class ListChuyenBayModel
    {
        public List<ChuyenBayModel> listCB {  get; set; }
        public ListChuyenBayModel()
        {
            
        }
        public List<ChuyenBayModel> SortMin()
        {
            return listCB.OrderBy(x => x.Gia).ToList();
        }
        public List<ChuyenBayModel> SortMax()
        {
            return listCB.OrderByDescending(x => x.Gia).ToList();
        }
        public List<ChuyenBayModel> SortEarly()
        {
            return listCB.OrderBy(x => x.GioCatCanh).ToList();
        }
        public List<ChuyenBayModel> SortTimeSpan()
        {
            return listCB.OrderBy(x => x.TongThoiGianBay).ToList();
        }

    }
}