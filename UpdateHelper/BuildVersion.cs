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

        public RemoteInfo Handler(string fileName, string version, string desc, string forceFlag, string directUpdate)
        {
            this.InitConfig();
            return this.CreateNewFile(fileName, version, desc, forceFlag, directUpdate);
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

        private RemoteInfo CreateNewFile(string fileName, string version, string desc, string forceFlag, string directUpdate)
        {
            var verFolder = Path.Combine(_workDir, version);
            if (!Directory.Exists(verFolder))
                Directory.CreateDirectory(verFolder);

            File.Move(fileName, Path.Combine(_workDir, version, "最新版本.zip"));
            File.WriteAllText(Path.Combine(_workDir, version, "最新版本更新说明.txt"), desc, Encoding.GetEncoding("GBK"));

            return this.EditServer(version, desc, forceFlag, directUpdate);
        }

        private RemoteInfo EditServer(string version, string desc, string forceFlag, string directUpdate)
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

            var newRemoteInfo = new RemoteInfo()
            {
                ApplicationStart = _startApplicationName,
                DirectUpdate = directUpdate,
                ForceFlag = forceFlag,
                ReleaseUrl = new Uri(_releaseUri, $"{version}/最新版本.zip").OriginalString,
                ReleaseVersion = version,
                UpdateMode = "Increment",
                VersionDesc = new Uri(_releaseUri, $"{version}/最新版本更新说明.txt").OriginalString,
            };

            remoteInfos.Add(newRemoteInfo);

            string json;
            using (MemoryStream ms = new MemoryStream())
            {
                jsonSerializer.WriteObject(ms, remoteInfos);
                json = Encoding.UTF8.GetString(ms.ToArray());
            }

            File.WriteAllText(Path.Combine(_workDir, "Server.json"), json, new UTF8Encoding(false));

            return newRemoteInfo;
        }
    }
}
