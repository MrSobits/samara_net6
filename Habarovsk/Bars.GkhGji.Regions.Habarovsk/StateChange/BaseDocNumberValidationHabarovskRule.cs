namespace Bars.GkhGji.Regions.Habarovsk.StateChange
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Regions.Voronezh.Entities.Dicts;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Базовое правило перехода статуса
    /// </summary>
    public abstract class BaseDocNumberValidationHabarovskRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }
        public IDomainService<DocumentGjiInspector> DocumentGjiInspectorDomain { get; set; }
        public IDomainService<ZonalInspectionInspector> ZonalInspectionInspectorDomain { get; set; }
        public IDomainService<ZonalInspectionPrefix> ZonalInspectionPrefixDomain { get; set; }
        public IDomainService<DocumentGji> DocumentGjiDomain { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public abstract string Id { get; }

        /// <summary>
        /// Имя
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Код типа
        /// </summary>
        public abstract string TypeId { get; }

        /// <summary>
        /// Описание
        /// </summary>
        public abstract string Description { get; }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var document = statefulEntity as DocumentGji;

            if (document == null)
            {
                return ValidateResult.No("Документ не найден");
            }

            Action(document);

            return ValidateResult.Yes();
        }

        protected void Action(DocumentGji document)
        {
            if (document.DocumentNumber.IsEmpty())
            {
                var docNum = this.GetDocNum(document);
                var prefix = this.GetPrefix(document);

                document.DocumentNumber = $"{prefix}{docNum}";
                document.DocumentNum = docNum;
            }
        }

        protected virtual int GetDocNum(DocumentGji document)
        {
            var year = DateTime.Now.Year;
            var docNum = this.DocumentGjiDomain.GetAll()
                .Where(x => x.TypeDocumentGji == document.TypeDocumentGji && x.DocumentYear.HasValue && x.DocumentYear.Value == year)
                .Where(x => x.DocumentNum.HasValue)
                .Select(x => x.DocumentNum.Value)
                .SafeMax(x => x);

            return ++docNum;
        }

        public virtual string GetPrefix(DocumentGji document)
        {
            return string.Empty;
        }
    }
}
