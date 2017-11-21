// Copyright Matthias Koch 2017.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.IO;
using System.Linq;
using NuGet.Packaging;
using NuGet.Versioning;
using Nuke.Core.Utilities;

namespace Nuke.Common.Tools.NuGet
{
    partial class NuGetPackage
    {
        public static NuGetPackage LoadFrom (string path)
        {
            using (var stream = File.OpenRead(path))
            {
                return LoadFrom(Manifest.ReadFrom(stream, validateSchema: true));
            }
        }

        public static NuGetPackage LoadFrom (Manifest manifest)
        {
            var metadata = manifest.Metadata;
            var package = new NuGetPackage()
                    .SetId(metadata.Id)
                    .SetVersion(metadata.Version.ToString())
                    .SetDescription(metadata.Description)
                    .SetAuthors(metadata.Authors)
                    .SetOwners(metadata.Owners)
                    .SetTags(metadata.Tags.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                    .SetTitle(metadata.Title)
                    .SetSummary(metadata.Summary)
                    .SetLanguage(metadata.Language)
                    .SetCopyright(metadata.Copyright)
                    .SetReleaseNotes(metadata.ReleaseNotes)
                    .SetRequireLicenseAcceptance(metadata.RequireLicenseAcceptance)
                    .SetLicenseUrl(metadata.LicenseUrl?.ToString())
                    .SetProjectUrl(metadata.ProjectUrl?.ToString())
                    .SetIconUrl(metadata.IconUrl?.ToString());

            return package;
        }

        public void Save (string path)
        {
            using (var stream = File.OpenWrite(path))
            {
                var metadata = new ManifestMetadata
                               {
                                   Id = Id,
                                   Version = NuGetVersion.Parse(Version),
                                   Description = Description,
                                   Authors = Authors,
                                   Owners = Owners,
                                   Tags = Tags.JoinSpace(),
                                   Title = Title,
                                   Summary = Summary,
                                   Language = Language,
                                   Copyright = Copyright,
                                   ReleaseNotes = ReleaseNotes,
                                   RequireLicenseAcceptance = RequireLicenseAcceptance ?? false
                               };

                metadata.SetLicenseUrl(LicenseUrl);
                metadata.SetProjectUrl(ProjectUrl);
                metadata.SetIconUrl(IconUrl);

                var manifest = new Manifest(metadata);
                manifest.Save(stream);
            }
        }
    }
}
