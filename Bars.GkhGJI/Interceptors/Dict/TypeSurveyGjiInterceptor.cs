namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class TypeSurveyGjiInterceptor : EmptyDomainInterceptor<TypeSurveyGji>
    {
        #region Public Methods and Operators

        public override IDataResult BeforeDeleteAction(IDomainService<TypeSurveyGji> service, TypeSurveyGji entity)
        {
            var refFuncs = this.GetDeleteValidators();

            var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

            var message = string.Empty;

            if (refs.Length > 0)
            {
                message = refs.Aggregate(message, (current, str) => current + string.Format(" {0}; ", str));
                message = string.Format("Существуют связанные записи в следующих таблицах: {0}", message);
                return this.Failure(message);
            }

            return this.Success();
        }

        #endregion

        #region Methods

        protected virtual List<Func<long, string>> GetDeleteValidators()
        {
            return new List<Func<long, string>>
                       {
                           id =>
                           this.Container.Resolve<IDomainService<TypeSurveyGoalInspGji>>().GetAll().Any(x => x.TypeSurvey.Id == id) ? "Цели проверки типа обследования" : null, 
                           id =>
                           this.Container.Resolve<IDomainService<TypeSurveyInspFoundationGji>>().GetAll().Any(x => x.TypeSurvey.Id == id)
                               ? "Правовое основание проверки типа обследования"
                               : null, 
                           id =>
                           this.Container.Resolve<IDomainService<TypeSurveyKindInspGji>>().GetAll().Any(x => x.TypeSurvey.Id == id)
                               ? "Виды обследования типа обследования"
                               : null, 
                           id =>
                           this.Container.Resolve<IDomainService<TypeSurveyTaskInspGji>>().GetAll().Any(x => x.TypeSurvey.Id == id)
                               ? "Задачи проверки типа обследования"
                               : null, 
                           id =>
                           this.Container.Resolve<IDomainService<DisposalTypeSurvey>>().GetAll().Any(x => x.TypeSurvey.Id == id) ? "Типы обследования рапоряжения ГЖИ" : null
                       };
        }

        #endregion
    }
}