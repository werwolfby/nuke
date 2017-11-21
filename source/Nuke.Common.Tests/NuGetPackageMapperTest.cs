// Copyright Matthias Koch 2017.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Nuke.Common.Tools.NuGet;
using Xunit;

namespace Nuke.Common.Tests
{
    public class NuGetPackageMapperTest
    {
        [Fact]
        public void Test ()
        {
            var package = new NuGetPackage()
                    .SetId("PackageId")
                    .SetVersion("1.2.3-prerelease001")
                    .SetDescription("Description")
                    .SetAuthors("Author1", "Author2")
                    .SetOwners("Owner1", "Owner2")
                    .SetTitle("Title")
                    .SetRequireLicenseAcceptance(requireLicenseAcceptance: false);

            var tempFile = Path.GetTempFileName();
            package.Save(tempFile);

            NuGetPackage.LoadFrom(tempFile).ShouldBeEquivalentTo(package);
        }
    }
}
