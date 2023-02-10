using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace UpdateHelper
{
    internal class BuildVersion
    {
        private string _workDir = "";
        private string _backupDir = "";
        private Uri _releaseUri;
        private string _startApplicationName;

        public RemoteInfo Handler(string fileName, string desc, RemoteInfo remoteInfo)
        {
            this.InitConfig();
            return this.CreateNewFile(fileName, desc, remoteInfo);
        }

        private void InitConfig()
        {
            var workdir = System.Configuration.ConfigurationManager.AppSettings["workDir"];
            if (string.IsNullOrEmpty(workdir))
                _workDir = AppDomain.CurrentDomain.BaseDirectory;
            else
                _workDir = workdir;

            var backupDir = System.Configuration.ConfigurationManager.AppSettings["backupDir"];
            if (string.IsNullOrEmpty(backupDir))
                _backupDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "backup");
            else
            {
                if (backupDir == workdir)
                    _backupDir = Path.Combine(backupDir, "backup");
                else
                    _backupDir = backupDir;
            }

            var releaseUri = System.Configuration.ConfigurationManager.AppSettings["releaseUri"];
            _releaseUri = new Uri(releaseUri);

            _startApplicationName = System.Configuration.ConfigurationManager.AppSettings["startApplicationName"];

        }

        private RemoteInfo CreateNewFile(string fileName, string desc, RemoteInfo remoteInfo)
        {
            var verFolder = Path.Combine(_workDir, remoteInfo.ReleaseVersion);
            if (!Directory.Exists(verFolder))
                Directory.CreateDirectory(verFolder);

            File.Move(fileName, Path.Combine(_workDir, remoteInfo.ReleaseVersion, "最新版本.zip"));
            File.WriteAllText(Path.Combine(_workDir, remoteInfo.ReleaseVersion, "最新版本更新说明.txt"), desc, Encoding.GetEncoding("GBK"));

            return this.EditServer(remoteInfo);
        }

        private RemoteInfo EditServer(RemoteInfo remoteInfo)
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<RemoteInfo>));
            List<RemoteInfo> remoteInfos = null;
            try
            {
                using (var stream = new FileStream(Path.Combine(_workDir, "Server.json"), FileMode.Open))
                    remoteInfos = jsonSerializer.ReadObject(stream) as List<RemoteInfo>;
            }
            catch
            {
            }

            if (remoteInfos == null)
                remoteInfos = new List<RemoteInfo>();

            remoteInfo.ApplicationStart = _startApplicationName;
            remoteInfo.ReleaseUrl = new Uri(_releaseUri, $"{remoteInfo.ReleaseVersion}/最新版本.zip").OriginalString;
            remoteInfo.VersionDesc = new Uri(_releaseUri, $"{remoteInfo.ReleaseVersion}/最新版本更新说明.txt").OriginalString;

            remoteInfos.Add(remoteInfo);

            string json;
            using (MemoryStream ms = new MemoryStream())
            {
                jsonSerializer.WriteObject(ms, remoteInfos);
                json = Encoding.UTF8.GetString(ms.ToArray());
            }

            File.WriteAllText(Path.Combine(_workDir, "Server.json"), json, new UTF8Encoding(false));

            return remoteInfo;
        }
    }
}
