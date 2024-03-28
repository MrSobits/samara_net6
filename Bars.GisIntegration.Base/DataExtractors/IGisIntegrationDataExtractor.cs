namespace Bars.GisIntegration.Base.DataExtractors
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Entities;

    public interface IGisIntegrationDataExtractor
    {
        /// <summary>
        /// Код метода
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Поставщик данных
        /// </summary>
        RisContragent Contragent { get; set; }

        /// <summary>
        /// Вытащить данные из МЖФ в РИС
        /// </summary>
        Dictionary<Type, List<BaseRisEntity>> Extract(DynamicDictionary parameters = null);
    }
}