namespace Bars.Gkh.Config
{
    using System.Collections.Generic;
    using Bars.B4;
    using Bars.B4.Utils;

    public interface IGkhParams
    {
        DynamicDictionary GetParams();

        DynamicDictionary SaveParams(BaseParams baseParams);

        List<string> Keys { get; }
    }
}
