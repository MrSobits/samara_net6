namespace Bars.Gkh.DomainService
{
    using System;
    using Bars.Gkh.Entities;

    public interface IRealObjOverhaulDataService
    {
        /// <summary>
        /// Возвращает дату опубликования основной программы ДПКР, в который включен дом 
        /// </summary>
        DateTime? GetPublishDateByRo(RealityObject ro);
    }
}