namespace Bars.GkhGji.Regions.Zabaykalye.Interceptors
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    public class ZabaykalyeAppealCitsAnswerInterceptor : EmptyDomainInterceptor<AppealCitsAnswer>
    {
        /// <summary>
        /// Задание номера ответа на обращение граждан.
        /// Формат номера "Ож-{Индекс Зональной Инспекции}-{Порядковый номер в пределах данной инспекции и текущего года}"
        /// Например, "Ож-123-3", если инспектор принадлежит зональной инспекции с индексом 123 и это третий ответ,
        /// в текущем году, данный всеми инспекторами этой инспекции.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override IDataResult BeforeCreateAction(IDomainService<AppealCitsAnswer> service, AppealCitsAnswer entity)
        {
            var inspector = entity.Executor;

            if (inspector == null)
            {
                return Failure("Не указан исполнитель ответа!");
            }

            var zonalInspInspectorDomain = Container.ResolveDomain<ZonalInspectionInspector>();

            using (Container.Using(zonalInspInspectorDomain))
            {
                var inspection = zonalInspInspectorDomain.GetAll().Select(x => new
                {
                    InspectorId = x.Inspector.Id,
                    InspectionId = x.ZonalInspection.Id,
                    InspectionIndex = x.ZonalInspection.IndexOfGji
                }).FirstOrDefault(x => x.InspectorId == inspector.Id);

                if (inspection == null)
                {
                    return Failure("Не указана инспекция исполнителя!");
                }

                var colleaguesIds =
                    zonalInspInspectorDomain.GetAll()
                        .Where(x => x.ZonalInspection.Id == inspection.InspectionId)
                        .Select(x => x.Inspector.Id)
                        .ToArray();

                var currentYearAnswersMaxNumber = service.GetAll()
                    // Берем только ответы за текущий год
                    .Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Year == DateTime.Now.Year)
                    // Берем только ответы, данные инспекторами той же зональной инспекции
                    .Where(x => colleaguesIds.Contains(x.Executor.Id))
                    .Where(x => x.DocumentNumber != null && x.DocumentNumber != string.Empty)
                    .Select(x => x.DocumentNumber).ToArray()
                    // Берем только ответы с нумерацией, удовлетворяющей правилам нумерации
                    .Where(x => Regex.IsMatch(x, "Ож-\\d+-\\d+"))
                    // Получаем максимальный порядковый номер
                    .Select(x => x.Substring(x.LastIndexOf('-') + 1).ToInt(0)).SafeMax(x => x);

                entity.DocumentNumber = string.Format("Ож-{0}-{1}", inspection.InspectionIndex,
                    currentYearAnswersMaxNumber + 1);

                return base.BeforeCreateAction(service, entity);
            }
        }
    }
}
