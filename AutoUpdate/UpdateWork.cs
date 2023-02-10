using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Ionic.Zip;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.Serialization.Json;

namespace MAutoUpdate
{
    public class UpdateWork
    {
        public delegate void UpdateProgess(double data);
        public UpdateProgess OnUpdateProgess;

        public event EventHandler UpdateCompleted;

        public delegate void UpdateStateChanged(string data);
        public UpdateStateChanged OnUpdateStateChanged;

        public delegate void UpdateVersionChanged(string data);
        public UpdateVersionChanged OnUpdateVersionChanged;

        string mainName;
        //临时目录（WIN7以及以上在C盘只有对于temp目录有操作权限）
        string tempPath = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), $@"MAutoUpdate\temp\");
        string bakPath = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), @"MAutoUpdate\bak\");

        LocalInfo localInfo;

        public List<RemoteInfo> UpdateVerList { get; set; }
        public string programName { get; set; }
        public string subKey { get; set; }
        /// <summary>
        /// 初始化配置目录信息
        /// </summary>
        public UpdateWork(string _programName, string localAddress, string isClickUpdate)
        {
            localInfo = new LocalInfo(localAddress);
            Process cur = Process.GetCurrentProcess();
            mainName = Path.GetFileName(cur.MainModule.FileName);
            programName = _programName;
            //创建备份目录信息
            DirectoryInfo bakinfo = new DirectoryInfo(bakPath);
            if (bakinfo.Exists == false)
            {
                bakinfo.Create();
            }
            //创建临时目录信息
            DirectoryInfo tempinfo = new DirectoryInfo(tempPath);
            if (tempinfo.Exists == false)
            {
                tempinfo.Create();
            }
            localInfo.LoadJson();
            if (localInfo.LocalVersion == null)
                return;
            UpdateVerList = GetServer(localInfo.ServerUpdateUrl);
            CheckVer(isClickUpdate);
        }

        public bool Do()
        {
            KillProcessExist();
            //Thread.Sleep(400);
            //更新之前先备份
            //Bak();
            //Thread.Sleep(400);
            //备份结束开始下载东西
            DownLoad();//下载更新包文件信息

            return true;
        }

        public void IgnoreThisVersion()
        {
            var item = UpdateVerList[UpdateVerList.Count - 1];
            localInfo.LocalIgnoreVersion = item.ReleaseVersion;
            localInfo.SaveJson();
        }

        /// <summary>
        /// 获取更新的服务器端的数据信息
        /// </summary>
        /// <param name="url">自动更新的URL信息</param>
        /// <returns></returns>
        private List<RemoteInfo> GetServer(string url)
        {
            ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;

            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<RemoteInfo>));
            List<RemoteInfo> remoteInfos = null;
            try
            {
                var temp = WebRequest.Create(url);
                using (var stream = temp.GetResponse().GetResponseStream())
                    remoteInfos = jsonSerializer.ReadObject(stream) as List<RemoteInfo>;
            }
            catch
            {
            }

            if (remoteInfos == null)
                remoteInfos = new List<RemoteInfo>();

            return remoteInfos;

        }
        private static bool RemoteCertificateValidate(
              object sender, X509Certificate cert,
               X509Chain chain, SslPolicyErrors error)
        {
            System.Console.WriteLine("Warning, trust any certificate");
            return true;
        }

        /// <summary>
        /// 下载方法
        /// </summary>
        private void DownLoad()
        {
            var percentPerVersion = 80.0 / UpdateVerList.Count;

            int index = 0;
            foreach (var item in UpdateVerList)
            {
                OnUpdateStateChanged?.Invoke($"正在下载 v{item.ReleaseVersion} ...");
                OnUpdateVersionChanged?.Invoke(item.GetVersionDesc());
                var Myrq = (HttpWebRequest)HttpWebRequest.Create(item.ReleaseUrl);
                var myrp = (HttpWebResponse)Myrq.GetResponse();
                double totalBytes = myrp.ContentLength * 1.0, totalDownloadedByte = 0;
                Stream st = myrp.GetResponseStream();
                var so = new FileStream(tempPath + item.ReleaseVersion + ".zip", FileMode.Create);
                byte[] by = new byte[10240];
                int osize = st.Read(by, 0, by.Length);
                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte;
                    so.Write(by, 0, osize);
                    osize = st.Read(by, 0, by.Length);

                    OnUpdateProgess.Invoke(index * percentPerVersion + percentPerVersion * (totalDownloadedByte / totalBytes));
                }
                so.Close();
                st.Close();
                index++;
            }


            //3、开始更新
            Update();
            Thread.Sleep(400);

            Start();
            Thread.Sleep(400);

            UpdateCompleted?.Invoke(this, EventArgs.Empty);

            //using (WebClient web = new WebClient())
            //{
            //    web.DownloadProgressChanged += Web_DownloadProgressChanged;
            //    web.DownloadFileCompleted += Web_DownloadFileCompleted;
            //    foreach (var item in UpdateVerList)
            //    {
            //        try
            //        {
            //            //LogTool.AddLog("更新程序：下载更新包文件" + item.ReleaseVersion);
            //            web.DownloadFileAsync(new Uri(item.ReleaseUrl), tempPath + item.ReleaseVersion + ".zip");
            //        }
            //        catch (Exception ex)
            //        {
            //            //LogTool.AddLog("更新程序：更新包文件" + item.ReleaseVersion + "下载失败,本次停止更新，异常信息：" + ex.Message);
            //            throw ex;
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 备份当前的程序目录信息
        /// </summary>
        private UpdateWork Bak()
        {
            try
            {

                //LogTool.AddLog("更新程序：准备执行备份操作");

                DirectoryInfo di = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                foreach (var item in di.GetFiles())
                {
                    if (item.Name != mainName)//当前文件不需要备份
                    {
                        File.Copy(item.FullName, bakPath + item.Name, true);
                    }
                }
                //文件夹复制
                foreach (var item in di.GetDirectories())
                {
                    if (item.Name != "bak" && item.Name != "temp")
                    {
                        CopyDirectory(item.FullName, bakPath);
                    }
                }
                //LogTool.AddLog("更新程序：备份操作执行完成,开始关闭应用程序");
                OnUpdateProgess?.Invoke(20);
                return this;
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }

        private UpdateWork Update()
        {
            int index = 1;
            var percent = 18.0 / UpdateVerList.Count;
            foreach (var item in UpdateVerList)
            {
                OnUpdateStateChanged?.Invoke($"正在升级 v{item.ReleaseVersion} ...");
                OnUpdateVersionChanged?.Invoke($"正在升级 请耐心等待...");
                try
                {
                    //如果是覆盖安装的话，先删除原先的所有程序
                    if (item.UpdateMode == "Cover")
                    {
                        DelLocal();
                    }
                    string path = tempPath + item.ReleaseVersion + ".zip";
                    using (ZipFile zip = new ZipFile(path, Encoding.GetEncoding("GBK")))
                    {
                        //LogTool.AddLog("更新程序：解压" + item.ReleaseVersion + ".zip");
                        zip.ExtractAll(AppDomain.CurrentDomain.BaseDirectory, ExtractExistingFileAction.OverwriteSilently);
                        //LogTool.AddLog("更新程序：" + item.ReleaseVersion + ".zip" + "解压完成");
                        ExecuteINI();//执行注册表等更新以及删除文件
                    }

                }
                catch (Exception ex)
                {
                    //LogTool.AddLog("更新程序出现异常：异常信息：" + ex.Message);
                    //LogTool.AddLog("更新程序：更新失败，进行回滚操作");
                    //Restore();
                    break;
                }
                finally
                {
                    //删除下载的临时文件
                    //LogTool.AddLog("更新程序：删除临时文件" + item.ReleaseVersion);
                    DelTempFile(item.ReleaseVersion + ".zip");//删除更新包
                    //LogTool.AddLog("更新程序：临时文件删除完成" + item.ReleaseVersion);
                }
                OnUpdateProgess?.Invoke(80 + index++ * percent);
            }

            localInfo.LocalVersion = UpdateVerList.Last().ReleaseVersion;
            localInfo.SaveJson();
            Application.DoEvents();

            return this;
        }

        private UpdateWork Start()
        {
            OnUpdateStateChanged?.Invoke("正在启动...");
            String[] StartInfo = UpdateVerList[UpdateVerList.Count - 1].ApplicationStart.Split(',');
            if (StartInfo.Length > 0)
            {
                try
                {
                    foreach (var item in StartInfo)
                    {
                        //LogTool.AddLog("更新程序：启动" + item);
                        Process.Start(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, item));
                    }
                }
                catch
                {
                }
            }
            OnUpdateProgess?.Invoke(100);
            return this;
        }



        /// <summary>
        /// 文件拷贝
        /// </summary>
        /// <param name="srcdir">源目录</param>
        /// <param name="desdir">目标目录</param>
        private UpdateWork CopyDirectory(string srcdir, string desdir)
        {
            string folderName = srcdir.Substring(srcdir.LastIndexOf("\\") + 1);

            string desfolderdir = desdir + "\\" + folderName;

            if (desdir.LastIndexOf("\\") == (desdir.Length - 1))
            {
                desfolderdir = desdir + folderName;
            }
            string[] filenames = Directory.GetFileSystemEntries(srcdir);
            foreach (string file in filenames)// 遍历所有的文件和目录
            {
                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {
                    string currentdir = desfolderdir + "\\" + file.Substring(file.LastIndexOf("\\") + 1);
                    if (!Directory.Exists(currentdir))
                    {
                        Directory.CreateDirectory(currentdir);
                    }
                    CopyDirectory(file, desfolderdir);
                }
                else // 否则直接copy文件
                {
                    string srcfileName = file.Substring(file.LastIndexOf("\\") + 1);
                    srcfileName = desfolderdir + "\\" + srcfileName;
                    if (!Directory.Exists(desfolderdir))
                    {
                        Directory.CreateDirectory(desfolderdir);
                    }
                    File.Copy(file, srcfileName, true);
                }
            }
            return this;
        }


        /// <summary>
        /// 删除临时文件
        /// </summary>
        private UpdateWork DelTempFile(String name)
        {
            FileInfo file = new FileInfo(tempPath + name);
            file.Delete();
            return this;
        }

        /// <summary>
        /// 更新失败的情况下，回滚当前更新
        /// </summary>
        private UpdateWork Restore()
        {
            DelLocal();
            CopyDirectory(bakPath, AppDomain.CurrentDomain.BaseDirectory);
            return this;
        }
        /// <summary>
        /// 删除本地文件夹的文件
        /// </summary>
        private UpdateWork DelLocal()
        {
            DirectoryInfo di = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            foreach (var item in di.GetFiles())
            {
                if (item.Name != mainName)
                {
                    if (item.Name == "Local.json")
                    {
                    }
                    else
                    {
                        File.Delete(item.FullName);
                    }
                }
            }
            foreach (var item in di.GetDirectories())
            {
                if (item.Name != "bak" && item.Name != "temp")
                {
                    item.Delete(true);
                }
            }
            return this;
        }
        /// <summary>
        /// 校验程序版本号
        /// </summary>
        /// <param name="LocalVer">当前本地版本信息</param>
        /// <returns></returns>
        private UpdateWork CheckVer(string isClickUpdate)
        {
            var localVer = localInfo.AbsoluteValue;
            var localIgnoreVer = localInfo.IgnoreAbsoluteValue;

            UpdateVerList = UpdateVerList.Where(p => p.AbsoluteValue != localIgnoreVer && p.AbsoluteValue > localVer).OrderBy(p => p.AbsoluteValue).ToList();

            return this;
        }



        /// <summary>
        /// 更新配置信息
        /// </summary>
        private UpdateWork ExecuteINI()
        {
            DirectoryInfo TheFolder = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            if (File.Exists(Path.Combine(TheFolder.FullName, "config.update")))
            {
                string[] ss = File.ReadAllLines(Path.Combine(TheFolder.FullName, "config.update"));
                Int32 i = -1;//0[regedit_del] 表示注册表删除‘1[regedit_add]表示注册表新增 2[file_del] 表示删除文件
                foreach (var s in ss)
                {
                    String s1 = s.Trim();
                    if (s1 == "[regedit_del]")
                    {
                        i = 0;
                    }
                    else if (s1 == "[regedit_add]")
                    {
                        i = 1;
                    }
                    else if (s1 == "[file_del]")
                    {
                        i = 2;
                    }
                    else
                    {
                        if (i == 0)
                        {
                            string[] tempKeys = s1.Split(',');
                            DelRegistryKey(tempKeys[0], tempKeys[1]);
                        }
                        else if (i == 1)
                        {
                            string[] values = s1.Split('=');
                            string[] tempKeys = values[0].Split(',');
                            SetRegistryKey(tempKeys[0], tempKeys[1], values[1]);
                        }
                        else if (i == 2)
                        {
                            DelFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, s1));
                        }
                    }
                }
                DelFile(Path.Combine(TheFolder.FullName, "config.update"));
            }
            return this;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        private UpdateWork DelFile(string name)
        {
            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name)))
            {
                FileInfo file = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name));
                file.Delete();
            }
            return this;
        }

        /// <summary>
        /// 校验当前程序是否在运行
        /// </summary>
        /// <param name="programName"></param>
        /// <returns></returns>
        public bool CheckProcessExist()
        {
            return Process.GetProcessesByName(programName).Length > 0 ? true : false;
        }

        /// <summary>
        /// 杀掉当前运行的程序进程
        /// </summary>
        /// <param name="programName">程序名称</param>
        public void KillProcessExist()
        {
            Process[] processes = Process.GetProcessesByName(programName);
            foreach (Process p in processes)
            {
                p.Kill();
                p.Close();
            }
        }

        #region 暂时没用，如果需要将本地版本放注册表的话 那是有用的
        /// <summary>
        /// 设置注册表值
        /// </summary>
        /// <param name="subKey"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void SetRegistryKey(String subKey, String key, String value)
        {
            RegistryKey reg;
            RegistryKey reglocal = Registry.CurrentUser;

            reg = reglocal.OpenSubKey(subKey, true);
            if (reg == null)
                reg = reglocal.CreateSubKey(subKey);
            reg.SetValue(key, value, RegistryValueKind.String);
            if (reg != null)
            {
                reg.Close();
            }
        }
        private void DelRegistryKey(String subKey, String key)
        {
            RegistryKey reg;
            RegistryKey reglocal = Registry.CurrentUser;

            reg = reglocal.OpenSubKey(subKey, true);
            if (reg != null)
            {
                var res = reg.GetValue(key);
                if (res != null)
                {
                    reg.DeleteValue(key);

                }
            }
            reg.Close();
        }
        #endregion
    }
}
