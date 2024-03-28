namespace Bars.GkhDi.DomainService
{
    using System.Linq;
    using B4.DataAccess;
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.GkhDi.Entities;

    using Castle.Windsor;


    /// <summary>
    /// Управление финансовой деятельностью дома
    /// </summary>
    public class FinActivityManagRealityObjService : IFinActivityManagRealityObjService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Доменный сервис "Управление финансовой деятельностью дома"
        /// </summary>
        public IDomainService<FinActivityManagRealityObj> DomainService { get; set; }

        /// <summary>
        /// Доменный сервис "Деятельность УО в период раскрытия информации"
        /// </summary>
        public IDomainService<DisclosureInfo> DiDomainService { get; set; }

        /// <summary>
        /// Доменный сервис сущности <see cref="ManOrgContractRealityObject"/>
        /// </summary>
        public IDomainService<ManOrgContractRealityObject> MorgContractRobjectService { get; set; }

        /// <summary>
        /// Сохранить информацию о финансовой деятельности домов
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Информацию об успешности сохранения</returns>
        public IDataResult SaveManagRealityObj(BaseParams baseParams)
        {
            var records = baseParams.Params.GetAs<FinActivityManagRealityObj[]>("jsonString");

            var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");

            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var diInfo = DiDomainService.Get(disclosureInfoId);

                    foreach (var finActivityManagRealityObj in records)
                    {
                        finActivityManagRealityObj.DisclosureInfo = diInfo;

                        if (finActivityManagRealityObj.Id > 0)
                            DomainService.Update(finActivityManagRealityObj);
                        else
                            DomainService.Save(finActivityManagRealityObj);
                    }

                    // Находим неактуальные сохраненные записи(которые когда то отображались но в силу искл дома из договора в данном периоде не видны) и удаляем их
                    if (diInfo != null)
                    {
                        var robjectFilter =
                            MorgContractRobjectService.GetAll()
                                .Where(x => x.ManOrgContract.ManagingOrganization.Id == diInfo.ManagingOrganization.Id)
                                .Select(x => new
                                {
                                    x.RealityObject.Id,
                                    x.ManOrgContract.StartDate,
                                    x.ManOrgContract.EndDate
                                })
                                .Where(x => x.StartDate <= diInfo.PeriodDi.DateEnd && (!x.EndDate.HasValue || x.EndDate >= diInfo.PeriodDi.DateStart))
                                .Select(x => x.Id);

                        var notActualRecords = DomainService.GetAll()
                            .Where(x => x.DisclosureInfo.Id == diInfo.Id)
                            .Where(y => !robjectFilter.Any(x => x == y.RealityObject.Id));

                        foreach (var notActualRec in notActualRecords)
                        {
                            DomainService.Delete(notActualRec.Id);
                        }
                    }

                    tr.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException exc)
                {
                    tr.Rollback();
                    return new BaseDataResult(false, exc.Message);
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }
        }
    }
}