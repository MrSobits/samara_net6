namespace Bars.B4.Modules.Analytics.Domain
{
    using System.Collections.Generic;
    using Bars.B4.Modules.Analytics.Data;

    /// <summary>
    /// 
    /// </summary>
    public interface IDataProviderParamService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataProviders"></param>
        /// <returns></returns>
        IEnumerable<DataProviderParam> List(params IDataProvider[] dataProviders);
    }
}
