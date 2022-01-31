using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAccounting.AboutOrderMenu
{
    public class AboutOrder
    {
        public int ID { get; set; }
        public string Phone { get; set; }
        public int Price { get; set; }
        public int Prepayment { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public DateTime DateOfOrderGet { get; set; }
        public DateTime DateOfOrderFinished { get; set; }
        public DateTime LayoutRaedy { get; set; }
        public bool Refusal { get; set; }
        public bool Ready { get; set; }
        public bool Issued { get; set; }
        public bool ToTheWorkShop { get; set; }

        public Image Picture { get; set; }
    }
}
