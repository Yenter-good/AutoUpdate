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
            using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.Default))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
