using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace charp_dotnet_form {
    public partial class Form1 : Form {
        public Form1()
        {
            InitializeComponent();
        }
        public static string MakeSpace(string str, int len)
        {
            string rst = string.Empty;
            int sum = len - str.Length;
            if (sum < 1) return rst;
            for (int i = 0; i < sum; i++)
            {
                rst += " ";
            }
            return rst;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ///显示Environment.SpecialFolder
            string[] enumArr = Enum.GetNames(typeof(Environment.SpecialFolder));
            Array enumValueArr = Enum.GetValues(typeof(Environment.SpecialFolder));
            for (int i = 0, j = 1; i < enumArr.Length; i++, j++)
            {
               this.textBox1.Text += j.ToString() + ":" + MakeSpace(j.ToString(), 10) + enumArr[i] +  MakeSpace(enumArr[i], 30) + Environment.GetFolderPath((Environment.SpecialFolder)enumValueArr.GetValue(i)) + "\r\n";
                
            }
        }
    }
    
}
