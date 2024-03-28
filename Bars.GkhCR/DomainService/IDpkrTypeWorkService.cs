namespace Bars.GkhCr.DomainService
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;

    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;

    /// <summary>
    /// Интерфейс сервиса по работам КР  
    /// </summary>
    public interface IDpkrTypeWorkService
    {
        /// <summary>
        /// Получить года работ из ДПКР
        /// </summary>
        /// <param name="typeWorkQuery">Коллекция видов работ</param>
        Dictionary<long, int> GetTypeWorkDpkrYear(IQueryable<TypeWorkCr> typeWorkQuery);

        /// <summary>
        /// Разделить конструктивный элемент от вида работ
        /// </summary>
        /// <param name="baseParams"> baseParams </param>
        /// <returns> IDataResult </returns>
        IDataResult SplitStructElementInTypeWork(BaseParams baseParams);

        /// <summary>
        /// Слить конструктивный элемент от вида работ к другому
        /// </summary>
        /// <param name="oldTypeWork"> Вид работ конструктивы которого удаляться </param>
        /// <param name="newTypeWork"> Вид работ конструктивы которому добавяться </param>
        void MergeStructElementInTypeWork(TypeWorkCr oldTypeWork, TypeWorkCr newTypeWork);

        /// <summary>
        /// Проверка на наличие связей в ДПКР у вида работ
        /// </summary>
        /// <param name="typeWork"> Вид работ </param>
        bool HasTypeWorkReferenceInDpkr(TypeWorkCr typeWork);

        /// <summary>
        /// Получить информацию по работам из ДПКР для объекта КР
        /// </summary>
        /// <param name="objectCrId">Объект КР</param>
        Dictionary<long, DpkrTypeWorkDto> GetWorksByObjectCr(long objectCrId);

        /// <summary>
        /// Получить информацию по работам из ДПКР для объектов КР
        /// </summary>
        /// <param name="objectCrIds">Объекты КР</param>
        Dictionary<long, DpkrTypeWorkDto> GetWorksByObjectCr(long[] objectCrIds);
    }

    public class DpkrTypeWorkDto
    {
        public PriceCalculateBy CalcBy;

        public string UnitMeasure;

        public decimal Volume;

        public decimal Sum;
    }
}