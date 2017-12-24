using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyChefDemo.Entities
{
    public class Payments
    {
        public Payments()
        {

        }
        public int CNumber { get; set; }
        public int ID { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public int CCV { get; set; }
        public string CardName { get; set; }

        public virtual Address Address { get; set; }


    }
}
