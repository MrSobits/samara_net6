namespace Bars.GkhGji.Regions.Tatarstan.Rules
{
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Rules;

    using Castle.Windsor;

    /// <summary>
    /// Правило вида проверки для "Решение на проверку предписания"
    /// </summary>
    public class DecisionPrescriptionCode1011Rule : DispPrescrCode1011Rule
    {
        /// <inheritdoc />
        public override string Code => nameof(DecisionPrescriptionCode1011Rule);

        /// <inheritdoc />
        public override string Name => "Решение на проверку предписания " +
            "(код вида проверки первого решения - 10,11)";

        /// <inheritdoc />
        protected override TypeDocumentGji AvailableTypeDocumentGji => TypeDocumentGji.Decision;
    }

    /// <remarks>
    /// Правило фиктивное - НЕ ЗАРЕГИСТРИРОВАНО.
    /// При появлении необходимости использования
    /// выделить в отдельный класс и зарегистрировать
    /// </remarks>
    public class DispPrescrCode1011Rule : IKindCheckRule
    {
        /// <summary>
        /// IoC-контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Repo с распоряжениями
        /// </summary>
        public IRepository<Disposal> DisposalRepository { get; set; }

        /// <summary>
        /// Текстовка наименования
        /// распоряжений для текущего региона
        /// </summary>
        public IDisposalText DisposalText { get; set; }

        /// <inheritdoc />
        public int Priority => 0;

        /// <inheritdoc />
        public virtual string Code => "DispPrescrCode1011Rule";

        /// <inheritdoc />
        public virtual string Name => $"{this.DisposalText.SubjectiveCase} на проверку предписания " +
            $"(код вида проверки первого {this.DisposalText.GenetiveCase.ToLower()} - 10,11)";

        /// <inheritdoc />
        public TypeCheck DefaultCode => TypeCheck.NotPlannedInspectionVisit;

        /// <summary>
        /// Доступный тип документа
        /// </summary>
        protected virtual TypeDocumentGji AvailableTypeDocumentGji => TypeDocumentGji.Disposal;

        /// <inheritdoc />
        public bool Validate(Disposal entity)
        {
            if (entity.TypeDocumentGji != this.AvailableTypeDocumentGji ||
                entity.TypeDisposal != TypeDisposalGji.DocumentGji)
                return false;

            var mainDisposal = this.DisposalRepository
                .GetAll()
                .FirstOrDefault(x => x.Inspection.Id == entity.Inspection.Id &&
                    x.TypeDisposal == TypeDisposalGji.Base &&
                    x.TypeDocumentGji == this.AvailableTypeDocumentGji);

            var availableTypeCheckCodes = new[]
            {
                TypeCheck.PlannedInspectionVisit,
                TypeCheck.NotPlannedInspectionVisit
            };

            return mainDisposal != null && availableTypeCheckCodes.Contains(mainDisposal.KindCheck.Code);
        }
    }
}