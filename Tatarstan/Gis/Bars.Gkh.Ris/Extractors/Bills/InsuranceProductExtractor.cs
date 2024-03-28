namespace Bars.Gkh.Ris.Extractors.Bills
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.Bills;

    /// <summary>
    /// Экстрактор для страховых продуктов
    /// </summary>
    public class InsuranceProductExtractor : BaseSlimDataExtractor<InsuranceProduct>
    {
        /// <summary>
        /// Получить сущности сторонней системы - страховой продукт
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы - подъезды</returns>
        public override List<InsuranceProduct> Extract(DynamicDictionary parameters)
        {
            List<InsuranceProduct> result = null;
            var result1 = new InsuranceProduct
            {
                Name = "Name1",
                Description = "Description1",
                CloseDate = DateTime.MaxValue
            };
            result.Add(result1);
            var result2 = new InsuranceProduct
            {
                Name = "Name2",
                Description = "Description2",
                CloseDate = DateTime.MaxValue
            };
            result.Add(result2);
            //List<InsuranceProduct> result = null;
            return result;
        }
    }
}
