﻿namespace Bars.GkhGji.Rules
{
    using B4;
    using Enums;
    using Entities;

    using Castle.Windsor;
    using Bars.B4.IoC;

    /// <inheritdoc />
    public class BaseDispHeadCourtDocRule : IKindCheckRule
    {
        /// <inheritdoc />
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public int Priority => 0;

        /// <inheritdoc />
        public string Code => "BaseDispHeadCourtDocRule";

        /// <inheritdoc />
        public string Name => "Проверка по поручению руководителя. Основание «Передача документов в суд…», Форма проверки «Документарная»";

        /// <inheritdoc />
        public TypeCheck DefaultCode => TypeCheck.NotPlannedDocumentation;

        /// <inheritdoc />
        public bool Validate(Disposal entity)
        {
            if (entity.TypeDisposal != TypeDisposalGji.Base
                || entity.Inspection.TypeBase != TypeBase.DisposalHead)
            {
                return false;
            }

            var service = this.Container.Resolve<IDomainService<BaseDispHead>>();
            using(this.Container.Using(service))
            {
                var dispHead = service.Load(entity.Inspection.Id);

                return dispHead.TypeBaseDispHead == TypeBaseDispHead.TransferToCourt
                    && dispHead.TypeForm == TypeFormInspection.Documentary;
            }
        }
    }
}