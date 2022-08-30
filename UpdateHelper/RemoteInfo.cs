using System;
using System.Collections.Generic;
using System.Text;

namespace UpdateHelper
{
    public class RemoteInfo
    {
        public string ApplicationStart { get; set; }
        public string AppName { get; set; }
        public string ReleaseUrl { get; set; }
        public string ReleaseVersion { get; set; }
        public string UpdateMode { get; set; }
        public string VersionDesc { get; set; }
        /// <summary>
        /// 强制更新 0不强制 其他强制
        /// </summary>
        public string ForceFlag { get; set; }
    }
}
