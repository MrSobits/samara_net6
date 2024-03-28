namespace Bars.Gkh.Gis.DomainService.Indicator
{
    using System.Collections.Generic;
    using B4;
    using Entities.Dict;

    public interface IIndicatorService
    {        
        List<IndicatorGroupProxy> GetIndicatorTree(BaseParams baseParams);
        
        //IEnumerable<ServiceDictionary> GetServicesByIndicatorsGroup(ServiceDictionary serviceDictionary);
    }
}