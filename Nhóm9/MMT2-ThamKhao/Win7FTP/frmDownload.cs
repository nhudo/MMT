using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.IO;
using WinFTP.Library;

namespace WinFTP
{
    public partial class frmDownload : Form
    {
        #region Members
        //String Variables we will need to use throughout the File.
        string FileName, SaveFilePath, CurrentDirectory;

        //FTPClient used to Download File and setup File Download Events
        FTPclient FtpClient;
        #endregion
        
        #region Constructor
        /// <summary>
        /// Hảm dựng frmDownload để thực thi chương trình
        /// author: Gấu béo
        /// date: 26/10/2018 
        /// title: Edit Func Send file
        /// </summary>
        /// <param name="Filename">Name of the File to Download</param>
        /// <param name="Current_Directory">Current Directory of the FTPClient; where file will be downloaded from.</param>
        /// <param name="SavePath">Path where the File will be saved.</param>
        /// <param name="Ftpclient">FTPClient from frmMain that will be refrenced here to FtpClient variable.</param>
        
        public frmDownload(string Filename, string Current_Directory, string SavePath, FTPclient Ftpclient)
        {
            //Init Form
            InitializeComponent();

            //Setup Variables
            //Tên file muốn tải về
            FileName = Filename;
            //Đường dẫn để lưu file
            SaveFilePath = SavePath;
            //Thiết lập thư mục hiện tại để tập tin tải về tại đó
            CurrentDirectory = Current_Directory;
            lblDownloadFrom.Text = Ftpclient.Hostname + Current_Directory + FileName;   //ex: ftp://ftp.somesite.com/current_dir/File.exe
            //label tiêu đề
            lblSavePath.Text = SaveFilePath;
            //Thiết lập FTPClient
            FtpClient = Ftpclient;

            //Show Form
            this.Show();

            //Setup our Download Client and Start Downloading
            FtpClient.CurrentDirectory = Current_Directory;
            //Sự kiện OnDownloadProgressChanged
            FtpClient.OnDownloadProgressChanged += new FTPclient.DownloadProgressChangedHandler(FtpClient_OnDownloadProgressChanged);
            //Sự kiện  OnDownloadCompleted
            FtpClient.OnDownloadCompleted += new FTPclient.DownloadCompletedHandler(FtpClient_OnDownloadCompleted);
            //Bắt đầu tải về
            FtpClient.Download(FileName, SavePath, true);
        }
        #endregion

        #region Events
        #region Download Client Events
        bool Happened = false;      //Supposedly, The OnDownloadCompleted repeats.  Well each time, it repeats one more time than
                                    //it previously repeated. Nothing bad about this, except that tyhe ShowCompleteDownloadDialog 
                                    //gets called as well. We don't want that.  So I have this variable to keep track of everything.

        //Event fires when the Download has completed.
        void FtpClient_OnDownloadCompleted(object sender, DownloadCompletedArgs e)
        {
            if (e.DownloadCompleted)
            {
                if (!Happened)
                { 
                    //Display the appropriate information to the User regarding the Download.
                    this.Text = "Download Completed!";
                    lblDownloadStatus.Text = "Downloaded File Successfully!";
                    progressBar1.Value = progressBar1.Maximum;
                    btnCancel.Text = "Exit";
                }
                Happened = true;
            }
            else
            {
                lblDownloadStatus.Text = "Download Status: " + e.DownloadStatus;
                this.Text = "Download Error";
                btnCancel.Text = "Exit";
                MessageBox.Show("Error: " + e.DownloadStatus);
            }
            Happened = true;
        }

        //Event Fires whenever the Download Progress in changed.
        void FtpClient_OnDownloadProgressChanged(object sender, DownloadProgressChangedArgs e)
        {
            //Set Value for Progressbar
            progressBar1.Maximum = Convert.ToInt32(e.TotleBytes);
            progressBar1.Value = Convert.ToInt32(e.BytesDownloaded);

            // Calculate the download progress in percentages
            Int64 PercentProgress = Convert.ToInt64((progressBar1.Value * 100) / e.TotleBytes);

            //Display Information to the User on Form and on Labels
            this.Text = PercentProgress.ToString() + "% Downloading " + FileName;
            lblDownloadStatus.Text = "Download Status: Downloaded " + GetFileSize(e.BytesDownloaded) + " out of " + GetFileSize(e.TotleBytes) + " (" + PercentProgress.ToString() + "%)";
        }
        #endregion

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            if (btnCancel.Text != "Exit")
                FtpClient.CancelDownload();  //This means that the Text is "Cancel" and the User wants to Cancel Download.
                                             //Remember that we are changing text to Exit when Download Finishes.
            this.Close();
        }

        private void frmDownload_FormClosing(object sender, FormClosingEventArgs e)
        {
            Happened = false;
        }
        #endregion

        #region Functions
        /// <summary>
        /// Code Below Converts Bytes to KB, MB, GB, or just Bytes.  Makes the App more look :)
        /// Obtained from: http://www.freevbcode.com/ShowCode.Asp?ID=1971
        /// </summary>
        /// <param name="byteCount">Bytes that need to be converted</param>
        /// <returns>Converts the Bytes into its Appropriate form (KB, MB, GB, or just Bytes) and returns them in the form of: ex: 22 KB</returns>
        private string GetFileSize(double byteCount)
        {
            string size = "0 Bytes";
            if (byteCount >= 1073741824.0)
                size = String.Format("{0:##.##}", byteCount / 1073741824.0) + " GB";
            else if (byteCount >= 1048576.0)
                size = String.Format("{0:##.##}", byteCount / 1048576.0) + " MB";
            else if (byteCount >= 1024.0)
                size = String.Format("{0:##.##}", byteCount / 1024.0) + " KB";
            else if (byteCount > 0 && byteCount < 1024.0)
                size = byteCount.ToString() + " Bytes";

            return size;
        }
        #endregion
    }
}
