using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFTP
{
    public partial class frmRename : Form
    {
        #region Members
        string Path, Extension, FileName;
        frmMain ForMain;
        #endregion

        #region Contructor
        /// <summary>
        /// Renames a File on the Server
        /// </summary>
        /// <param name="fileName">Name of the File to Rename</param>
        /// <param name="CurrentDir">Directory that contains the file we want to rename</param>
        /// <param name="MainForm">Pass instance of the MainForm.  MainForm because we have Messages and the FtpClient on MainForm will show the Message passed by Rename.  There is probably another work around. But in a hurry I chose to do this.</param>
        /// <param name="extension">Extension of the File to rename</param>
        public frmRename(string fileName, string CurrentDir, frmMain MainForm, string extension)
        {
            //Set Data for variab;es
            Path = CurrentDir;
            Extension = extension;
            FileName = fileName;
            InitializeComponent();
            ForMain = MainForm;

            //Set Information for User
            this.Text = "Rename " + FileName;
            lblFileName.Text = "Filename: " + FileName;
            lblLocation.Text = "Location: " + Path;
        }
        #endregion

        #region Events
        private void Form1_Resize(object sender, EventArgs e)
        {
            //Below code will never fire....But I have it for Development purposes..
            //in case someone decides to make it resizable
            Rectangle panelRect = ClientRectangle;
            panelRect.Inflate(-30, -30);
            pnlNonTransparent.Bounds = panelRect;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdRename_Click(object sender, EventArgs e)
        {
            try
            {
                ForMain.FtpClient.CurrentDirectory = Path;
                ForMain.FtpClient.FtpRename(FileName, txtRenameTo.Text);
                //(this.Parent as frmMain).FtpClient.FtpRename(FileName, txtRenameTo.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.Close();
        }
        #endregion
    }
}
