namespace Bars.GkhGji.Regions.Tomsk.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tomsk.Entities.Inspection;

    public class BaseStatementService : GkhGji.DomainService.BaseStatementService
    {
        protected override void SaveBaseStatementAppealCits(InspectionGji inspection, long[] appealCitsIds)
        {
            var serviceBaseStatementAppeal = Container.Resolve<IDomainService<InspectionAppealCits>>();
            var serviceAppeal = Container.Resolve<IDomainService<AppealCits>>();
            var servicePrimaryBaseStatementAppeal = Container.Resolve<IDomainService<PrimaryBaseStatementAppealCits>>();

            //делается так, потому что добавление обращений может быть еще в требование прокуратуры
            var first = inspection.TypeBase == TypeBase.CitizenStatement;

            foreach (var appealCitId in appealCitsIds)
            {
                var newRec = new InspectionAppealCits
                {
                    AppealCits = serviceAppeal.Load(appealCitId),
                    Inspection = inspection
                };
                
                serviceBaseStatementAppeal.Save(newRec);

                if (first)
                {
                    first = false;

                    servicePrimaryBaseStatementAppeal.Save(
                            new PrimaryBaseStatementAppealCits { BaseStatementAppealCits = newRec });
                }
            }
        }

        public override IDataResult GetInfo(BaseParams baseParams)
        {
            try
            {
                var appealCitizensNames = string.Empty;
                var appealCitizensIds = string.Empty;
                var primaryAppealCit = string.Empty;
                var primaryAppealCitId = (long?)null;

                if (baseParams.Params.ContainsKey("inspectionId"))
                {
                    var inspectionId = baseParams.Params.GetAs<long>("inspectionId");

                    // Получаем основное обращение проверки
                    var primaryBaseStatementAppealCits =
                        Container.Resolve<IDomainService<PrimaryBaseStatementAppealCits>>().GetAll()
                            .Where(x => x.BaseStatementAppealCits.Inspection.Id == inspectionId)
                            .Select(x => new
                            {
                                AppealCitId = x.BaseStatementAppealCits.AppealCits.Id,
                                x.BaseStatementAppealCits.AppealCits.NumberGji
                            })
                            .FirstOrDefault();

                    // Пробегаемся по обращениям и формируем итоговую строку наименований и строку идентификаторов
                    var baseStatementAppealCits =
                        Container.Resolve<IDomainService<InspectionAppealCits>>().GetAll()
                            .Where(x => x.Inspection.Id == inspectionId)
                            .Select(x => new { x.AppealCits.Id, x.AppealCits.NumberGji })
                            .ToList();

                    // Исключаем основное обращение проверки из остальных обращений
                    if (primaryBaseStatementAppealCits != null)
                    {
                        primaryAppealCit = primaryBaseStatementAppealCits.NumberGji;
                        primaryAppealCitId = primaryBaseStatementAppealCits.AppealCitId;

                        baseStatementAppealCits = baseStatementAppealCits.Where(x => x.Id != primaryBaseStatementAppealCits.AppealCitId).ToList();
                    }

                    foreach (var item in baseStatementAppealCits)
                    {
                        if (!string.IsNullOrEmpty(appealCitizensNames))
                        {
                            appealCitizensNames += ", ";
                        }

                        appealCitizensNames += item.NumberGji;

                        if (!string.IsNullOrEmpty(appealCitizensIds))
                        {
                            appealCitizensIds += ", ";
                        }

                        appealCitizensIds += item.Id.ToStr();
                    }
                }

                return new BaseDataResult(new { primaryAppealCitId, primaryAppealCit, appealCitizensNames, appealCitizensIds });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }
        
        protected override object[] ArrayToDeleteBaseStatementAppealCits(long inspectionId)
        {
            var servBaseStatementAppealCitizens = Container.Resolve<IDomainService<InspectionAppealCits>>();

            // Получаем основное обращение проверки
            var primaryBaseStatementAppealCitId =
                Container.Resolve<IDomainService<PrimaryBaseStatementAppealCits>>().GetAll()
                    .Where(x => x.BaseStatementAppealCits.Inspection.Id == inspectionId)
                    .Select(x => (long?)x.BaseStatementAppealCits.Id)
                    .FirstOrDefault();

            // Исключаем из списка на удаление основное обращение
            var array = servBaseStatementAppealCitizens.GetAll()
                .WhereIf(primaryBaseStatementAppealCitId != null, x => x.Id != primaryBaseStatementAppealCitId)
                .Where(x => x.Inspection.Id == inspectionId)
                .Select(x => (object)x.Id)
                .ToArray();

            return array;
        }
    }
}