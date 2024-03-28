namespace Bars.GkhGji.Regions.Tatarstan.DomainService
{
    using B4;
    using B4.Utils;

    public interface IGjiTatParamService
    {
        DynamicDictionary GetConfig();

        IDataResult SaveConfig(BaseParams baseParams);

        IDataResult SaveDict(BaseParams baseParams);
    }
}