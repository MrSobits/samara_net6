namespace Bars.Gkh.Overhaul.Hmao.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.Enum;

    public interface IStage3Service
    {
        /// <summary>
        /// Получение дерева ООИ с детализацией по КЭ
        /// </summary>
        /// <param name="baseParams">Содержащий st3Id - id 3 этапа ДПКР</param>
        /// <returns>Дерево</returns>
        IDataResult ListDetails(BaseParams baseParams);

        /// <summary>
        /// Получение краткой информации по 3 этапу ДПКР
        /// </summary>
        /// <param name="baseParams">Содержащий st3Id - id 3 этапа ДПКР</param>
        /// <returns>Объект</returns>
        IDataResult GetInfo(BaseParams baseParams);

        /// <summary>
        /// Получение видов работ по 3 этапу ДПКР
        /// </summary>
        /// <param name="baseParams">Содержащий st3Id - id 3 этапа ДПКР</param>
        /// <returns>Список видов работ</returns>
        IDataResult ListWorkTypes(BaseParams baseParams);

        /// <summary>
        /// Получение записей 3 этапа ДПКР с КЭ
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Список записей 3 этапа</returns>
        IQueryable<VersionRecordDto> ListWithStructElements(BaseParams baseParams);
    }

    /// <summary>
    /// Прокси объект записи 3 этапа
    /// </summary>
    public class VersionRecordDto
    {
        public virtual long Id { get; set; }

        public virtual string Municipality { get; set; }

        public virtual string RealityObject { get; set; }

        public virtual string CommonEstateObjects { get; set; }

        public virtual int Year { get; set; }

        public virtual bool FixedYear { get; set; }

        public virtual ChangeBasisType ChangeBasisType { get; set; }

        public virtual int YearCalculated { get; set; }

        public virtual int IndexNumber { get; set; }

        public virtual bool IsChangedYear { get; set; }

        public virtual decimal Point { get; set; }

        public virtual decimal Sum { get; set; }

        public virtual string Changes { get; set; }

        public virtual string Remark { get; set; }

        public virtual string HouseNumber { get; set; }

        public virtual string StructuralElements { get; set; }
        
        public virtual string EntranceNum { get; set; }

        public virtual string KPKR { get; set; }
        public virtual string WorkCode { get; set; }

        public virtual List<StoredPriorityParam> StoredCriteria { get; set; }

        public virtual List<StoredPointParam> StoredPointParams { get; set; }

        public bool Hidden { get; set; }

        public bool IsSubProgram { get; set; }
    }
}