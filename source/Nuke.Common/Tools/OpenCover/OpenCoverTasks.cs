// Copyright 2018 Maintainers and Contributors of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Linq;
using Nuke.Common.Tooling;

namespace Nuke.Common.Tools.OpenCover
{
    partial class OpenCoverTasks
    {
        public static OpenCoverSettings DefaultOpenCover => new OpenCoverSettings()
            .SetWorkingDirectory(NukeBuild.Instance.RootDirectory)
            .SetRegistration(RegistrationType.User)
            .SetTargetExitCodeOffset(targetExitCodeOffset: 0)
            .SetFilters(
                "+[*]*",
                "-[xunit.*]*",
                "-[NUnit.*]*",
                "-[FluentAssertions.*]*",
                "-[Machine.Specifications.*]*")
            .SetExcludeByAttributes(
                "*.Explicit*",
                "*.Ignore*",
                "System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute")
            .SetExcludeByFile(
                "*/*.Generated.cs",
                "*/*.Designer.cs",
                "*/*.g.cs",
                "*/*.g.i.cs");
    }
}
