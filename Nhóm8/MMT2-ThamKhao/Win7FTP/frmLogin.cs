using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinFTP.Library;

namespace WinFTP
{
    public partial class frmLogin : Form
    {
        #region Members
        public static FTPclient objFtp;
        private frmMain Main;
        #endregion

        #region Contructor
        public frmLogin()
        {
            //Init Form
            InitializeComponent();
        }
        #endregion

        #region Events

        private void Form1_Resize(object sender, EventArgs e)
        {

            Rectangle panelRect = ClientRectangle;
            panelRect.Inflate(-30, -30);
            pnlNonTransparent.Bounds = panelRect;
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            try
            {
                //Gán FTP
                FTPclient objFtp = new FTPclient(txtHostName.Text, txtUserName.Text, txtPassword.Text);
                objFtp.CurrentDirectory = "/";
                Main = new frmMain();
           
                //Gán FTP Client trong form chính
                Main.SetFtpClient(objFtp);

                //Hiển thị form chính và ẩn form hiện tại
                Main.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                //Hiển thị lỗi
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
