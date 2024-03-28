namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Нарушение ГЖИ
    /// </summary>
    public class ViolationGji : BaseGkhEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        ///     Наименование
        /// </summary>
        public virtual ViolationGji ParentViolationGji { get; set; }

        /// <summary>
        ///     ЖК РФ
        /// </summary>
        public virtual string GkRf { get; set; }

        /// <summary>
        /// Код ПИН
        /// </summary>
        public virtual string CodePin { get; set; }

        /// <summary>
        /// Строка в которую будет сохранятся все Нормативные документы через запятую 
        /// </summary>
        public virtual string NormativeDocNames { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// ПП РФ №25
        /// </summary>
        public virtual string PpRf25 { get; set; }

        /// <summary>
        /// ПП РФ №307
        /// </summary>
        public virtual string PpRf307 { get; set; }

        /// <summary>
        /// ПП РФ №491
        /// </summary>
        public virtual string PpRf491 { get; set; }

        /// <summary>
        /// ПП РФ №170
        /// </summary>
        public virtual string PpRf170 { get; set; }

        /// <summary>
        /// Прочие нормативные документы
        /// </summary>
        public virtual string OtherNormativeDocs { get; set; }

        /// <summary>
        /// Актуальное
        /// </summary>
        public virtual bool IsNotActual { get; set; }
        
        /// Актуальность
        /// </summary>
        public virtual bool IsActual { get; set; }
    }
}