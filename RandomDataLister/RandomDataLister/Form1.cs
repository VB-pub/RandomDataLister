using RandomDataLister.Database;
using RandomDataLister.DataGenerator;
using RandomDataLister.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace RandomDataLister
{
    public partial class Form1 : Form
    {
        bool isWorking;
        Thread writeThr;
        BlockingCollection<RandomData> producedData = new BlockingCollection<RandomData>(boundedCapacity: 30);
        List<Thread> threads = new List<Thread>();
        Query query = new Query();
        public Form1()
        {
            InitializeComponent();
            listView1.View = View.Details;
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!isWorking)
            {
                int count = 0;
                if (int.TryParse(textBox1.Text, out count) && count >= 2 && count <= 15)
                {
                    listView1.Items.Clear();
                    button1.Text = "STOP";
                    button1.BackColor = Color.DarkRed;
                    isWorking = true;
                    label1.ForeColor = Color.Black;

                    query.CreateConn();
                    writeThr = new Thread(Write);
                    writeThr.Start();
                    threads.Clear();
                    GenerateRandomString();
                }
                else
                {
                    label1.ForeColor = Color.DarkRed;
                }
            }
            else
            {
                button1.Text = "START";
                button1.BackColor = Color.DarkGreen;
                isWorking = false;

                try
                {
                    query.CloseConn();
                    threads.ForEach(t => t.Abort());
                    writeThr.Abort();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
        private void GenerateRandomString()
        {
            try
            {
                int threadCount = int.Parse(textBox1.Text);
                if (threadCount >= 2 && threadCount <= 15)
                {
                    Generator generator = new Generator(ref producedData, threadCount);
                    for (int i = 1; i <= threadCount; i++)
                    {
                        int j = i;
                        Thread thr = new Thread(() => generator.Generate(j));
                        thr.Start();
                        threads.Add(thr);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Write()
        {
            while (isWorking)
            {
                if (producedData.Count == 0)
                {
                    Thread.Sleep(10);
                    continue;
                }

                RandomData rnd = producedData.Take();
                ListViewItem item = new ListViewItem(new[] { rnd.ThreadId.ToString(), rnd.Data });

                if (listView1.InvokeRequired)
                {
                    listView1.Invoke((MethodInvoker)delegate ()
                    {
                        listView1.Items.Add(item);
                        listView1.EnsureVisible(listView1.Items.Count - 1);

                        int count = listView1.Items.Count;
                        if (count > 20)
                            listView1.Items.Remove(listView1.Items[0]);
                    });
                }

                query.Write(rnd);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

