namespace Bars.GkhGji.Regions.Zabaykalye.ViewModel.Dict
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Entities.Dict;

    using Castle.Core.Internal;

	/// <summary>
	/// Вьюмодель для Тип обследования ГЖИ
	/// </summary>
	public class TypeSurveyGjiViewModel : BaseViewModel<TypeSurveyGji>
    {
		/// <summary>
		/// Домен сервис для Приказ
		/// </summary>
		public IDomainService<Disposal> DisposalDomain { get; set; }

		/// <summary>
		/// Домен сервис для Тип контрагента типа обследования
		/// </summary>
		public IDomainService<TypeSurveyContragentType> TypeSurveyContragentTypeDomain { get; set; }

		/// <summary>
		/// Получить список
		/// </summary>
		/// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
		/// <returns>Результат получения списка</returns>
		public override IDataResult List(IDomainService<TypeSurveyGji> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var ids = baseParams.Params.GetAs<string>("Id");

            // получаем вид проверки для загрузки типов обследования, содержащих этот вид проверки
            var kindCheckId = baseParams.Params.GetAs("kindCheckId", 0L);

            var disposalId = baseParams.Params.GetAsId("disposalId");

            List<long> listIds = null;

            if (!string.IsNullOrEmpty(ids))
            {
                listIds = new List<long>();
                listIds.AddRange(ids.Split(',').Select(x => x.ToLong()));
            }

            if (kindCheckId > 0)
            {
                this.Container.UsingForResolved<IDomainService<TypeSurveyKindInspGji>>(
                    (c, d) =>
                        {
                            var types = d.GetAll().Where(x => x.KindCheck.Id == kindCheckId).Select(x => x.TypeSurvey.Id).ToList();
                            listIds = listIds ?? new List<long>();
                            listIds.AddRange(types);
                        });
            }

            long[] jurTypeSurveyIds = null;
            if (disposalId > 0)
            {
                var disposal = DisposalDomain.Get(disposalId);
                if (disposal.Inspection.PersonInspection != PersonInspection.PhysPerson
                    && disposal.Inspection.PersonInspection != PersonInspection.RealityObject)
                {
                    var typeJurPerson = disposal.Inspection.TypeJurPerson;
                    jurTypeSurveyIds =
                        TypeSurveyContragentTypeDomain.GetAll()
                            .Where(x => x.TypeJurPerson == typeJurPerson)
                            .Select(x => x.TypeSurveyGji.Id)
                            .ToArray();
                }
            }

            var data =
                domainService.GetAll()
                    .WhereIf(listIds != null, x => listIds.Contains(x.Id))
                    .WhereIf(!jurTypeSurveyIds.IsEmpty(), x => jurTypeSurveyIds.Contains(x.Id))
                    .Select(x => new { x.Id, x.Code, x.Name })
                    .Filter(loadParam, Container);

            var totalCount = data.Count();

            var list = data.Order(loadParam).Paging(loadParam).ToList();
            var typeSurveyIds = list.Select(x => x.Id).ToArray();
            Dictionary<long, string> contragentTypes = null;
            this.Container.UsingForResolved<IDomainService<TypeSurveyContragentType>>(
                (c, d) =>
                    {
                        contragentTypes = d.GetAll()
                            .Where(x => typeSurveyIds.Contains(x.TypeSurveyGji.Id))
                            .ToList()
                            .GroupBy(x => x.TypeSurveyGji.Id)
                            .ToDictionary(x => x.Key, x => string.Join(", ", x.Select(y => y.TypeJurPerson.GetEnumMeta().Return(z => z.Display)).ToArray()));
                    });

            contragentTypes = contragentTypes ?? new Dictionary<long, string>();

            return new ListDataResult(list.Select(x => new { x.Id, x.Code, x.Name, ContragentTypes = contragentTypes.Get(x.Id) }), totalCount);
        }
    }
}