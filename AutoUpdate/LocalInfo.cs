using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace MAutoUpdate
{
    [DataContract]
    public class LocalInfo
    {
        [DataMember]
        public string LocalVersion { get; set; }
        [DataMember]
        public string ServerUpdateUrl { get; set; }
        [DataMember]
        public string LocalIgnoreVersion { get; set; }

        private string url = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Local.json");


        public LocalInfo(string localAddress)
        {
            url = Path.Combine(localAddress, "Local.json");
        }

        public void LoadJson()
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(LocalInfo));
            LocalInfo localInfo = null;
            try
            {
                using (var stream = new FileStream(url, FileMode.Open))
                    localInfo = jsonSerializer.ReadObject(stream) as LocalInfo;
            }
            catch
            {
            }

            if (localInfo == null)
                return;

            this.LocalVersion = localInfo.LocalVersion;
            this.ServerUpdateUrl = localInfo.ServerUpdateUrl;
            this.LocalIgnoreVersion = localInfo.LocalIgnoreVersion;
        }

        public void SaveJson()
        {
            string json;
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(LocalInfo));
            using (MemoryStream ms = new MemoryStream())
            {
                jsonSerializer.WriteObject(ms, this);
                json = Encoding.UTF8.GetString(ms.ToArray());
            }

            File.WriteAllText(url, json, new UTF8Encoding(false));
        }

        public int AbsoluteValue
        {
            get
            {
                var version = LocalVersion.Split('.');
                var ver1 = Convert.ToInt32(version[0]);
                var ver2 = Convert.ToInt32(version[1]);
                var ver3 = Convert.ToInt32(version[2]);
                var ver4 = Convert.ToInt32(version[3]);

                return ver1 * 1000 + ver2 * 100 + ver3 * 10 + ver4;
            }
        }

        public int IgnoreAbsoluteValue
        {
            get
            {
                var version = LocalVersion.Split('.');
                var ver1 = Convert.ToInt32(version[0]);
                var ver2 = Convert.ToInt32(version[1]);
                var ver3 = Convert.ToInt32(version[2]);
                var ver4 = Convert.ToInt32(version[3]);

                return ver1 * 1000 + ver2 * 100 + ver3 * 10 + ver4;
            }
        }
    }
}
