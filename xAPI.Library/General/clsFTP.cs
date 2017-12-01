using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Configuration;
using xAPI.Library.Base;
using xAPI.Library.General;
namespace xAPI.Library.General
{
    public static class clsFTP
    {
        private static void FTPDeleteFileBackup(String pathDownload)
        {
            try
            {
                if (File.Exists(pathDownload))
                    File.Delete(pathDownload);
            }
            catch (Exception)
            {
            }
        }

        private static Boolean FTPBackupFile(string pathFtp, string fileName, string pathDownload, string user, string password)
        {
            Boolean isCorrect = false;
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(pathFtp + fileName);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(user, password);
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = true;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                byte[] buffer = new byte[2048];
                FileStream newFile = new FileStream(pathDownload, FileMode.Create);
                int readCount = responseStream.Read(buffer, 0, buffer.Length);
                while (readCount > 0)
                {
                    newFile.Write(buffer, 0, readCount);
                    readCount = responseStream.Read(buffer, 0, buffer.Length);
                }
                newFile.Close();
                responseStream.Close();
                response.Close();
                if (!response.StatusDescription.Contains("226"))
                {
                    FTPDeleteFileBackup(pathDownload);
                    isCorrect = false;
                }
                else isCorrect = true;
            }
            catch (Exception ex)
            {
                isCorrect = false;
            }

            return isCorrect;
        }

        private static Boolean FTPSendFile(string pathFtp, string fileName, string pathDownload, string user, string password)
        {
            Boolean isCorrect = false;
            FileInfo fileInf = new FileInfo(pathDownload);
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(pathFtp + fileName);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(user, password);
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;
            request.ContentLength = fileName.Length;
            Stream responseStream = request.GetRequestStream();
            byte[] buffer = new byte[2048];
            FileStream fs = fileInf.OpenRead();
            try
            {
                int readCount = fs.Read(buffer, 0, buffer.Length);
                while (readCount > 0)
                {
                    responseStream.Write(buffer, 0, readCount);
                    readCount = fs.Read(buffer, 0, buffer.Length);
                }
            }
            catch (Exception ex)
            {
                isCorrect = false;
            }
            finally
            {
                fs.Close();
                responseStream.Close();
                var response = (FtpWebResponse)request.GetResponse();
                response.Close();
                if (!response.StatusDescription.Contains("226"))
                {
                    FTPDeleteFileBackup(pathDownload);
                    isCorrect = false;
                }
                else isCorrect = true;
            }
            return isCorrect;
        }

        public static void FTPUpload(string fileName, string pathUpload,Int32 FtpServer = 1)
        {
            string pathFtp = "";
            string pathDownload = "";// System.Web.Configuration.WebConfigurationManager.AppSettings["FTPPathBackup"].ToString() + fileName;
            string user = "";
            string password = "";
            Boolean exists = false;
            Boolean isCorrect = false;
            FileInfo info = null;
            try
            {
                pathDownload = System.Web.Configuration.WebConfigurationManager.AppSettings["FTPPathBackup"].ToString() + fileName;
                if (FtpServer == 1)
                {
                    pathFtp = System.Web.Configuration.WebConfigurationManager.AppSettings["FTPServer"].ToString();
                    user = System.Web.Configuration.WebConfigurationManager.AppSettings["FTPUser"].ToString();
                    password = System.Web.Configuration.WebConfigurationManager.AppSettings["FTPPassword"].ToString();
                }
                else
                    if (FtpServer == 2)
                    {
                        pathFtp = System.Web.Configuration.WebConfigurationManager.AppSettings["FTPServer2"].ToString();
                        user = System.Web.Configuration.WebConfigurationManager.AppSettings["FTPUser2"].ToString();
                        password = System.Web.Configuration.WebConfigurationManager.AppSettings["FTPPassword2"].ToString();
                    }
                    else
                        if (FtpServer == 3)
                        {
                            pathFtp = System.Web.Configuration.WebConfigurationManager.AppSettings["FTPServer3"].ToString();
                            user = System.Web.Configuration.WebConfigurationManager.AppSettings["FTPUser3"].ToString();
                            password = System.Web.Configuration.WebConfigurationManager.AppSettings["FTPPassword3"].ToString();
                        }
                        else
                            if (FtpServer == 4)
                            {
                                pathFtp = System.Web.Configuration.WebConfigurationManager.AppSettings["FTPServer3"].ToString();
                                user = System.Web.Configuration.WebConfigurationManager.AppSettings["FTPUser4"].ToString();
                                password = System.Web.Configuration.WebConfigurationManager.AppSettings["FTPPassword4"].ToString();
                            }

                info = new FileInfo(pathDownload);
            }
            catch (Exception)
            {
                
               
            }
            

           // string password = "Lu1sP@ch3c0"; //FTPPassword FTPPassword2
           
          
            try
            {
               // exists = FTPBackupFile(pathFtp, fileName, pathDownload, user, password);
                if (exists == true)
                {
                    // FTP Send File
                    isCorrect = FTPSendFile(pathFtp, fileName, pathUpload, user, password);
                    if (isCorrect)
                    {
                        FTPDeleteFileBackup(pathDownload);
                    }
                    else
                    { // FTP Restore File
                        FTPSendFile(pathFtp, fileName, pathDownload, user, password);
                        FTPDeleteFileBackup(pathDownload);
                    }
                }
                else
                {
                    // FTP Send File
                    FTPSendFile(pathFtp, fileName, pathUpload, user, password);
                }
            }
            catch (Exception ex)
            {
                if (exists)
                {
                    // FTP Restore File
                    FTPSendFile(pathFtp, fileName, pathDownload, user, password);
                    FTPDeleteFileBackup(pathDownload);
                }

            }
        }  
    }
}
