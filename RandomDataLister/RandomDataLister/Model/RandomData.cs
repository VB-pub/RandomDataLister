using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomDataLister.Model
{
    public class RandomData
    {
        public int Id { get; set; }
        public int ThreadId { get; set; }
        public DateTime Time { get; set; }
        public string Data { get; set; }

    }
}
