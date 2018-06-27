// Copyright Matthias Koch, Sebastian Karasek 2018.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Nuke.Common.Tools.VsWhere
{
    [Serializable]
    [PublicAPI]
    public class VsWhereVersionResult
    {
        public string InstanceId { get; set; }
        public DateTime InstallDate { get; set; }
        public string InstallationName { get; set; }
        public string InstallationPath { get; set; }
        public string InstallationVersion { get; set; }
        public string ProductId { get; set; }
        public string ProductPath { get; set; }

        [JsonProperty("isPrerelease")]
        public bool IsPreRelease { get; set; }

        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string ChannelId { get; set; }
        public string ChannelUri { get; set; }
        public string EnginePath { get; set; }
        public string ReleaseNotes { get; set; }
        public string ThirdPartyNotices { get; set; }
        public DateTime UpdateDate { get; set; }
        public VsWhereCatalog Catalog { get; set; }
        public Dictionary<string, object> Properties { get; set; }
    }
}
