namespace Bars.GkhGji.Regions.Tatarstan.Entities.Decision
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities.Base;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.ErknmTypeDocuments;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanDecision;

    /// <summary>
    /// Сущность "Основание проведения КНМ"
    /// </summary>
    public class KnmReason : BaseEntity, IEntityUsedInErknm
    {
        /// <summary>
        /// Тип документа ЕРКНМ
        /// </summary>
        public virtual ErknmTypeDocument ErknmTypeDocument { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Решение 
        /// </summary>
        public virtual TatarstanDecision Decision { get; set; }
        
        /// <inheritdoc />
        public virtual string ErknmGuid { get; set; }
    }
}