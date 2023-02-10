using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace MAutoUpdate
{
    public class RemoteInfo
    {
        public string ApplicationStart { get; set; }
        public string ReleaseUrl { get; set; }
        public string ReleaseVersion { get; set; }
        public string UpdateMode { get; set; }
        public string VersionDesc { get; set; }
        /// <summary>
        /// 强制更新 0不强制 其他强制
        /// </summary>
        public string ForceFlag { get; set; }
        /// <summary>
        /// 0默认 1直接更新并启动程序 
        /// </summary>
        public string DirectUpdate { get; set; }

        public string GetVersionDesc()
        {
            var temp = WebRequest.Create(VersionDesc);
            var stream = temp.GetResponse().GetResponseStream();
            using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("GBK")))
                return reader.ReadToEnd();
        }

        public int AbsoluteValue
        {
            get
            {
                var version = ReleaseVersion.Split('.');
                var ver1 = Convert.ToInt32(version[0]);
                var ver2 = Convert.ToInt32(version[1]);
                var ver3 = Convert.ToInt32(version[2]);
                var ver4 = Convert.ToInt32(version[3]);

                return ver1 * 100000000 + ver2 * 100000 + ver3 * 100 + ver4 / 10;
            }
        }
    }
}
