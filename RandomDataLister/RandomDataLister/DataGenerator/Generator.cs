using RandomDataLister.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RandomDataLister.DataGenerator
{
    class Generator
    {
        private string numStr = "0123456789", str = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private Random random;
        public BlockingCollection<RandomData> producedData;
        public Generator(ref BlockingCollection<RandomData> producedData, int threadCount = 15)
        {
            this.producedData = producedData;
            random = new Random();
        }
        public void Generate(int threadId)
        {
            while (true)
            {
                Produce(threadId, ref producedData);
            }
        }
        private void Produce(int threadId, ref BlockingCollection<RandomData> producedData)
        {
            producedData.Add(new RandomData { ThreadId = threadId, Data = RandomString(), Time = DateTime.Now });
            Thread.Sleep(random.Next(500, 2000));
        }
        private string RandomString()
        {
            int range = random.Next(5, 10);

            return new string(Enumerable.Repeat(numStr + str.ToLower() + str, range)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }



    }
}
