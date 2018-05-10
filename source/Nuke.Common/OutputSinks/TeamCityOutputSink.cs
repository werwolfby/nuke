// Copyright Matthias Koch, Sebastian Karasek 2018.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using Nuke.Common.BuildServers;
using Nuke.Common.Execution;
using Nuke.Common.Utilities;

namespace Nuke.Common.OutputSinks
{
    [UsedImplicitly]
    internal class TeamCityOutputSink : ConsoleOutputSink
    {
        private readonly TeamCity _teamCity;

        internal TeamCityOutputSink(TeamCity teamCity)
        {
            _teamCity = teamCity;
        }

        public override void Write(string text)
        {
            _teamCity.WriteMessage(text);
        }

        public override IDisposable WriteBlock(string text)
        {
            return DelegateDisposable.CreateBracket(
                () =>
                {
                    _teamCity.OpenBlock(text);
                    _teamCity.StartProgress(text);
                },
                () =>
                {
                    _teamCity.FinishProgress(text);
                    _teamCity.CloseBlock(text);
                });
        }

        public override void Trace(string text)
        {
            _teamCity.WriteMessage(text);
        }

        public override void Info(string text)
        {
            _teamCity.WriteMessage(text);
        }

        public override void Warn(string text, string details = null)
        {
            _teamCity.WriteWarning(text);
            if (details != null)
                _teamCity.WriteWarning(details);
        }

        public override void Error(string text, string details = null)
        {
            _teamCity.WriteError(text, details);
            _teamCity.AddBuildProblem(text);
        }

        public override void Success(string text)
        {
            _teamCity.WriteMessage(text);
        }

        public override void WriteSummary(IReadOnlyCollection<TargetDefinition> executionList)
        {
            foreach (var target in executionList.Where(x => x.Status == ExecutionStatus.Executed))
                _teamCity.AddStatisticValue($"buildStageDuration:{target.Name}", target.Duration.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
            
            base.WriteSummary(executionList);
        }
    }
}
