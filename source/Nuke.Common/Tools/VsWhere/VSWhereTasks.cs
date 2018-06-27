// Copyright Matthias Koch, Sebastian Karasek 2018.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities;

namespace Nuke.Common.Tools.VsWhere
{
    public partial class VsWhereTasks
    {
        public const string VcComponent = "Microsoft.VisualStudio.Component.VC.Tools.x86.x64";
        public const string MsBuildComponent = "Microsoft.Component.MSBuild";
        public const string NetCoreComponent = "Microsoft.Net.Core.Component.SDK";

        private static string GetResult(IProcess process, VsWhereSettings toolSettings, ProcessSettings processSettings)
        {
            return process.HasOutput ? process.Output.Where(x => x.Type == OutputType.Std).Select(x => x.Text).JoinNewLine() : string.Empty;
        }

        private static IProcess StartProcess(VsWhereSettings toolSettings, [CanBeNull] ProcessSettings processSettings)
        {
            processSettings = processSettings ?? new ProcessSettings();
            return ProcessTasks.StartProcess(toolSettings, processSettings.EnableRedirectOutput()).NotNull();
        }

        /// <summary><p>VsWhere is designed to be a redistributable, single-file executable that can be used in build or deployment scripts to find where Visual Studio - or other products in the Visual Studio family - is located.</p><p>For more details, visit the <a href="https://github.com/Microsoft/vswhere">official website</a>.</p></summary>
        public static string VsWhere(
            out IReadOnlyCollection<VsWhereVersionResult> parsedVersions,
            Configure<VsWhereSettings> configurator = null,
            ProcessSettings processSettings = null)
        {
            var toolSettings = configurator.InvokeSafe(new VsWhereSettings())
                .EnableUTF8()
                .SetFormat(Format.json)
                .ResetProperty();

            PreProcess(toolSettings);
            var process = StartProcess(toolSettings, processSettings);
            process.AssertZeroExitCode();
            PostProcess(toolSettings);
            var result = GetResult(process, toolSettings, processSettings);
            parsedVersions = ParseVersions(result);
            return result;
        }

        public static IReadOnlyCollection<VsWhereVersionResult> ParseVersions(string versionJson)
        {
            return SerializationTasks.JsonDeserialize<VsWhereVersionResult[]>(versionJson);
        }
    }
}
