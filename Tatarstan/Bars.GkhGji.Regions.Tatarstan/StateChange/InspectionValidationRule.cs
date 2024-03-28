namespace Bars.GkhGji.Regions.Tatarstan.StateChange
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Правило для проверки заполненности инспекционных проверок
    /// </summary>
    public class InspectionValidationRule : IRuleChangeStatus
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id
        {
            get { return "gji_inspection_validation_rule"; }
        }

        /// <summary>
        /// Название
        /// </summary>
        public string Name
        {
            get { return "Проверка заполненности инспекционных проверок"; }
        }

        /// <summary>
        /// Тип
        /// </summary>
        public string TypeId
        {
            get { return "gji_inspection"; }
        }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description
        {
            get { return "Данное правило проверяет заполненность необходимых полей инспекционных проверок"; }
        }

        /// <summary>
        /// Валидация
        /// </summary>
        /// <param name="statefulEntity">Сущность</param>
        /// <param name="oldState">Старый статус</param>
        /// <param name="newState">Новый статус</param>
        /// <returns>Результат валидации</returns>
        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var inspection = statefulEntity as InspectionGji;

            if (inspection != null)
            {
                var inspectionViolService = this.Container.Resolve<IDomainService<InspectionGjiViol>>();
                var servActCheckRo = this.Container.Resolve<IDomainService<ActCheckRealityObject>>();
                var servInspectionRo = this.Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
                var disposalService = this.Container.Resolve<IDomainService<Disposal>>();

                try
                {
                    if (newState.FinalState)
                    {
                        var disposalAgreementCheck = disposalService.GetAll()
                            .Where(x => x.Inspection.Id == inspection.Id)
                            .Where(
                                x => x.TypeAgreementProsecutor != TypeAgreementProsecutor.RequiresAgreement
                                    || x.TypeAgreementResult != TypeAgreementResult.NotAgreed)
                            .Select(x => x.Id);

                        // Если прошла проверку по Согласованию с прокуратурой, разрешаем смену статуса
                        // Иначе запускаем остальные проверки

                        if (disposalAgreementCheck.Any())
                        {

                            var realObjsIds = servActCheckRo.GetAll()
                                .Where(
                                    x =>
                                        x.ActCheck.Inspection.Id == inspection.Id
                                            && x.HaveViolation == YesNoNotSet.No)
                                .Where(x => x.RealityObject != null)
                                .Select(x => x.RealityObject.Id)
                                .ToHashSet();

                            realObjsIds.UnionWith(
                                inspectionViolService.GetAll()
                                    .Where(x => x.Inspection.Id == inspection.Id && x.DateFactRemoval.HasValue)
                                    .Where(x => x.RealityObject != null)
                                    .Select(x => x.RealityObject.Id)
                                    .ToArray());

                            var violationWithoutDate = inspectionViolService.GetAll()
                                .Where(
                                    x =>
                                        x.Inspection.Id == inspection.Id && !x.DateFactRemoval.HasValue);

                            var realObjWithoutViolation = servInspectionRo.GetAll()
                                .Where(
                                    x =>
                                        x.Inspection.Id == inspection.Id
                                            && !realObjsIds.Contains(x.RealityObject.Id));

                            if (violationWithoutDate.Any() || realObjWithoutViolation.Any())
                            {
                                return
                                    ValidateResult.No(" Не указаны результаты проверки, либо не каждое нарушение имеет дату фактического исполнения.");
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    this.Container.Release(inspectionViolService);
                    this.Container.Release(servActCheckRo);
                    this.Container.Release(servInspectionRo);
                    this.Container.Release(disposalService);
                }
            }

            return ValidateResult.Yes();
        }
    }
}