namespace Bars.Gkh.Interceptors
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Интерцептор для сущности <see cref="WorkKindCurrentRepair"/>
    /// </summary>
    public class WorkKindCurrentRepairServiceInterceptor : EmptyDomainInterceptor<WorkKindCurrentRepair>
    {

        /// <summary>
        /// Действие, выполняемое до создания сущности
        /// </summary>
        /// <param name="service">Домен-сервис сущности <see cref="WorkKindCurrentRepair"/></param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeCreateAction(IDomainService<WorkKindCurrentRepair> service, WorkKindCurrentRepair entity)
        {
            return this.CheckEntity(entity);
        }

        /// <summary>
        /// Действие, выполняемое до обновления сущности
        /// </summary>
        /// <param name="service">Домен-сервис сущности <see cref="WorkKindCurrentRepair"/></param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<WorkKindCurrentRepair> service, WorkKindCurrentRepair entity)
        {
            return this.CheckEntity(entity);
        }


        /// <summary>
        /// Действие, выполняемое до удаления сущности
        /// </summary>
        /// <param name="service">Домен-сервис сущности <see cref="WorkKindCurrentRepair"/></param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeDeleteAction(IDomainService<WorkKindCurrentRepair> service, WorkKindCurrentRepair entity)
        {
            var roCurentRepairDomain = Container.Resolve<IDomainService<RealityObjectCurentRepair>>();

            using (Container.Using(roCurentRepairDomain))
            {
                if (roCurentRepairDomain.GetAll().Any(x => x.WorkKind.Id == entity.Id))
                {
                    return Failure("Существуют связанные записи в следующих таблицах: Текущий ремонт жилого дома;");
                }
            }

            return Success();
        }

        private IDataResult CheckEntity(WorkKindCurrentRepair entity)
        {
            var errorsProps = new List<string>();

            if (entity.Name.IsEmpty())
            {
                errorsProps.Add("Наименование");
            }

            if (entity.UnitMeasure == null)
            {
                errorsProps.Add("Единица измерения");
            }

            if (entity.Code.IsEmpty())
            {
                errorsProps.Add("Код");
            }

            if (errorsProps.IsEmpty())
            {
                return Success();
            }

            var errorMessage = errorsProps.AggregateWithSeparator(", ");

            return Failure(errorMessage);
        }
    }
}
