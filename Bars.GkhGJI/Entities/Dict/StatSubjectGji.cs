namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Тематика обращения ГЖИ
    /// </summary>
    public class StatSubjectGji : BaseGkhEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Код тематики в Тематическом классификаторе
        /// </summary>
        public virtual string QuestionCode { get; set; }

        /// <summary>

        /// Код ССТУ
        /// </summary>
        public virtual string SSTUCode { get; set; }

        /// <summary>
        /// Код ССТУ
        /// </summary>
        public virtual string SSTUName { get; set; }

        /// <summary>
        /// Выгружать в СОПР
        /// </summary>
        public virtual bool ISSOPR { get; set; }

        /// <summary>
        /// Отслеживать обращение
        /// </summary>
        public virtual bool TrackAppealCits { get; set; }
        
        /// Учитывается в СОПР
        /// </summary>
        public virtual bool NeedInSopr { get; set; }
    }
}