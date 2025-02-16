using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;

namespace WGestures.App.Gui.Windows
{
    public partial class ErrorForm : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ErrorText
        {
            get { return tb_Detail.Text; }
            set { tb_Detail.Text = GetProductInfo() + value; }
        }

        public ErrorForm()
        {
            InitializeComponent();


            /*tb_Detail.MouseEnter += (sender, args) =>
            {
                tb_Detail.Focus();
                tb_Detail.SelectAll();
            };

            tb_mail.MouseEnter += (sender, args) =>
            {
                tb_mail.Focus();
                tb_mail.SelectAll();
            };*/
        }

        private void btn_close_Click(object sender, System.EventArgs e)
        {
            Close();
            Environment.Exit(1);
        }

        private static string GetProductInfo()
        {
            return "WGestures Version:" + 
                Application.ProductVersion + "\r\nOS:" + 
                Environment.OSVersion + "\r\nAppPath:" + 
                Application.ExecutablePath + "\r\n=======================\r\n";
        }

        private void copy_error_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(tb_Detail.Text);
            MessageBox.Show("复制成功!","信息");
              
        }
    }
}
