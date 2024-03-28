namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using Entities;

    /// <summary>
    /// Интерцептор для отопительного сезона (Не путать с проверкой по отопительному сезону)
    /// </summary>
    public class HeatSeasonInterceptor : EmptyDomainInterceptor<HeatSeason>
    {
        public override IDataResult BeforeCreateAction(IDomainService<HeatSeason> service, HeatSeason entity)
        {
            if (service.GetAll().Any(x => x.Period.Id == entity.Period.Id && x.RealityObject.Id == entity.RealityObject.Id))
            {
                return Failure(string.Format("Запись отопительного сезона за период {0} по адресу {1} уже существует",
                                          entity.Period.Name, entity.RealityObject.Address));
            }

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<HeatSeason> service, HeatSeason entity)
        {
            var refFuncs = new List<Func<long, string>>
                               {
                                  id => this.Container.Resolve<IDomainService<HeatSeasonDoc>>().GetAll().Any(x => x.HeatingSeason.Id == id) ? "Документ подготовки к отопительному сезону" : null,
                                  id => this.Container.Resolve<IDomainService<BaseHeatSeason>>().GetAll().Any(x => x.HeatingSeason.Id == id) ? "Подготовка к отопительному сезону" : null
                               };

            var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

            var message = string.Empty;

            if (refs.Length > 0)
            {
                message = refs.Aggregate(message, (current, str) => current + string.Format(" {0}; ", str));
                message = string.Format("Существуют связанные записи в следующих таблицах: {0}", message);
                return Failure(message);
            }

            return this.Success();
        }
    }
}