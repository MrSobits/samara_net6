namespace Bars.B4.Modules.Analytics.Domain
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Utils.Annotations;

    /// <summary>
    /// 
    /// </summary>
    public class DataProviderParamService: IDataProviderParamService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataProviders"></param>
        /// <returns></returns>
        public IEnumerable<DataProviderParam> List(params IDataProvider[] dataProviders)
        {
            ArgumentChecker.NotNull(dataProviders, "dataProviders");
            return dataProviders.SelectMany(x => x.Params);
        }
    }
}
