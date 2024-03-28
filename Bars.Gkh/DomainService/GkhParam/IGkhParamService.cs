namespace Bars.Gkh.DomainService.GkhParam
{
    using System;

    using Bars.B4;

    [Obsolete("Заменено на IGkhConfigProvider (Единые настройки приложения)")]
    public interface IGkhParamService
    {
        [Obsolete("Заменено на IGkhConfigProvider (Единые настройки приложения)")]
        IDataResult SaveParams(string prefix, object parameters);

        [Obsolete("Заменено на IGkhConfigProvider (Единые настройки приложения)")]
        IDataResult SaveParams(BaseParams baseParams);

        [Obsolete("Заменено на IGkhConfigProvider (Единые настройки приложения)")]
        IDataResult GetParams(string prefix = "");

        [Obsolete("Заменено на IGkhConfigProvider (Единые настройки приложения)")]
        IDataResult GetClientParams();

        [Obsolete("Заменено на IGkhConfigProvider (Единые настройки приложения)")]
        string GetParamByKey(string key, string prefix = "");
    }
}