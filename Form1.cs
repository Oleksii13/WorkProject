using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace WorkProject
{
    public partial class Form1 : Form
    {
        static string lastLine;
        private Timer tm = new Timer();
        static string path = @"C:\Users\User\Documents\File.txt";
        public Form1()
        {
            InitializeComponent();
        }
        //Solved thread error
        delegate void SetTextCallback(string text);
        private void SetText(string text)
        {
            if (this.textBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textBox1.Text = text;
            }
        }
        ///
            //Start button
        private void button1_Click(object sender, EventArgs e)
        {
            tm.Tick -= new EventHandler(timer1_Tick);
            tm.Tick += new EventHandler(timer1_Tick);
            tm.Interval = 2000;
            tm.Enabled = true;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime aDate = DateTime.Now;
            if (!File.Exists(path))
            {
                using (var txtFile = File.AppendText(path))
                {
                    txtFile.WriteLine($"<Line#1. " + aDate.ToString("yyyy/MM/dd hh:mm:ss tt"));
                    txtFile.Close();
                }
            }
            else if (File.Exists(path))
            {
                var lineCount = File.ReadAllLines(path).Length + 1;
                using (var txtFile = File.AppendText(path))
                {
                    txtFile.WriteLine("<Line#{0}. " + aDate.ToString("yyyy/MM/dd hh:mm:ss tt"), lineCount);
                    txtFile.Close();
                }
            }
            lastLine = File.ReadLines(path).Last();
            SetText(lastLine.ToString());
        }
        ///
            //Stop button
        private void button2_Click(object sender, EventArgs e)
        {
            tm.Enabled = false;
            FileSystemWatcher watcher = new FileSystemWatcher();
                watcher.Path = @"C:\Users\User\Documents\";
                watcher.NotifyFilter = NotifyFilters.LastAccess
                                       | NotifyFilters.LastWrite;
                watcher.Filter = "File.txt";
                watcher.Changed -= OnChanged;
                watcher.Changed += OnChanged;
                watcher.EnableRaisingEvents = true;
        }
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            lastLine = File.ReadLines(path).Last().ToString();
            SetText(lastLine.ToString());
        }
    }
}