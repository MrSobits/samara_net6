namespace Bars.Gkh.Overhaul.DomainService
{
    using System.Collections.Generic;

    using Bars.B4;

    public interface IDpkrParamsService
    {
        List<string> Keys { get; }

        Dictionary<string, string> SaveParams(BaseParams baseParams);

        Dictionary<string, string> GetParams();
    }
}
