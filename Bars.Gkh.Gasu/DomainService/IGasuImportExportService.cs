namespace Bars.Gkh.Gasu.DomainService
{
    using System.Collections.Generic;
    using Bars.Gkh.Gasu.Enums;

    public interface IGasuIndicatorService
    {
        List<EbirModule> GetAvailableModulesEbir();
    }
}
