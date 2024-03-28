namespace Bars.GkhCr.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Вьюха на Объект КР
    /// </summary>
    /*
     * Данная вьюха предназначена для реестра Сметный расчет по всем объектам КР
     * с агрегированными показателями из реестра Виды Работ КР и Сметные расчеты Кр:
     * Количество Видов Работ по ОКР
     * Количество Сметных расчетов по ОКР
     */
    public class ViewObjCrEstimateCalc : PersistentObject
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual string RealityObjName { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual string Municipality { get; set; }

        /// <summary>
        /// Муниципальное образование Id
        /// </summary>
        public virtual int MunicipalityId { get; set; }

        /// <summary>
        /// Муниципальный район Id
        /// </summary>
        public virtual int SettlementId { get; set; }

        /// <summary>
        /// Муниципальный район Название
        /// </summary>
        public virtual string SettlementName { get; set; }

        /// <summary>
        /// Id жилого дома
        /// </summary>
        public virtual int RealityObjectId { get; set; }

        /// <summary>
        /// Программа КР
        /// </summary>
        public virtual int ProgramCrId { get; set; }

        /// <summary>
        /// Объект КР
        /// </summary>
        public virtual int ObjectCrId { get; set; }

        /// <summary>
        /// Программа КР
        /// </summary>
        public virtual string ProgramCrName { get; set; }

        /// <summary>
        /// Количество Видов Работ по ОКР
        /// </summary>
        public virtual int? TypeWorkCrCount { get; set; }

        /// <summary>
        /// Количество Сметных расчетов по ОКР
        /// </summary>
        public virtual int? EstimateCalculationsCount { get; set; }
    }
}