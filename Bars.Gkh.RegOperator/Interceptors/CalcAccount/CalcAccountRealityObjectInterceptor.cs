namespace Bars.Gkh.RegOperator.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Интерцептор сущности "Жилой дом расчетного счета"
    /// </summary>
    public class CalcAccountRealityObjectInterceptor : EmptyDomainInterceptor<CalcAccountRealityObject>
    {
        /// <summary>
        /// Метод выполняющий определенные действия перед созданием сущности "Жилой дом расчетного счета"
        /// </summary>
        /// <param name="service">Домен-сервис</param>
        /// <param name="entity">Сущность "Жилой дом расчетного счета"</param>
        public override IDataResult BeforeCreateAction(IDomainService<CalcAccountRealityObject> service, CalcAccountRealityObject entity)
        {
            if (!entity.DateStart.IsValid())
            {
                return this.Failure("Некорректная дата начала открытия Р/С по дому");
            }

            var endPreviousRelationsDate = service.GetAll()
                .Where(x => x.RealityObject.Id == entity.RealityObject.Id)
                .Where(x => x.DateEnd.HasValue)
                .Max(x => x.DateEnd);
            
            var previousRelations = service.GetAll()
                .Where(x => x.RealityObject.Id == entity.RealityObject.Id)
                .Where(x => !x.DateEnd.HasValue);

            foreach (var relation in previousRelations)
            {
                relation.DateEnd = entity.DateStart.AddDays(-1);
                service.Update(relation);
            }

            // Если дата закрытия счета больше даты вступления в силу то берем дату закрытия счета(+1 день)

            if (endPreviousRelationsDate >= entity.DateStart)
            {
                entity.DateStart = endPreviousRelationsDate.ToDateTime().AddDays(1);
            }

            return this.Success();
        }
    }
}