namespace Bars.GkhCr.DomainService.Impl
{
    using System;
    using System.Linq;
    using B4;
    using B4.Utils;

    using Bars.Gkh.ConfigSections.Cr;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    using Enums;
    using Gkh.Domain;
    using Gkh.DomainService.GkhParam;
    using Gkh.Entities.Dicts;
    using GkhCr.DomainService;
    using GkhCr.Entities;

    /// <summary>
    /// Базовая реализация интерфейса IDefectService, на случай, если так вышло и решение не включает в себя один из модулей:
    ///     Bars.Gkh.Overhaul.Hmao
    ///     Bars.Gkh.Overhaul.Nso
    ///     Bars.Gkh.Overhaul.Tat
    /// </summary>
    public class DefectService : IDefectService
    {
        private readonly IWindsorContainer container;

        private readonly IDomainService<ObjectCr> objectCrService;

        private readonly IDomainService<ProgramCrChangeJournal> programCrChangeService;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">Windsor контейнер</param>
        /// <param name="objectCrService">Домен-сервис объекта капитального ремонта</param>
        /// <param name="programCrChangeService">Домен-сервис журнала изменений программы КР</param>
        public DefectService(
            IWindsorContainer container,
            IDomainService<ObjectCr> objectCrService,
            IDomainService<ProgramCrChangeJournal> programCrChangeService)
        {
            this.container = container;
            this.objectCrService = objectCrService;
            this.programCrChangeService = programCrChangeService;
        }

        /// <summary>
        /// В модуле GkhCr этот метод не вызывается, поэтому он не имеет реализации.
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Генерирует исключение NotImplementedException</returns>
        public IDataResult CalcInfo(BaseParams baseParams)
        {
            throw new NotImplementedException("Метод CalcInfo не реализован в модуле GkhCr");
        }

        public IDataResult CalcInfo(DefectList defectList)
        {
            var newParams = new DynamicDictionary
                                {
                                    { "date", defectList.DocumentDate },
                                    { "objectcr", defectList.ObjectCr.Id },
                                    { "volume", defectList.Volume },
                                    { "work", defectList.Work.Id },
                                    { "costPerUnitVolume", defectList.CostPerUnitVolume },
                                    { "sum", defectList.Sum },
                                    { "id", defectList.Id }
                                };
            return this.CalcInfo(new BaseParams { Params = newParams });
        }

        public IDataResult WorksForDefectList(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var typeWorkCrDomain = this.container.Resolve<IDomainService<TypeWorkCr>>();
            var objCrId = baseParams.Params.GetAs<int>("objCrId");

            var data = this.container.Resolve<IDomainService<Work>>().GetAll()
                .WhereIf(objCrId > 0, y => typeWorkCrDomain.GetAll().Any(x => x.ObjectCr.Id == objCrId && x.Work.Id == y.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Code,
                    x.ReformCode,
                    x.Description,
                    UnitMeasureName = x.UnitMeasure.Name,
                    x.Consistent185Fz,
                    x.IsActual
                })
                .Filter(loadParams, this.container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public IDataResult GetDefectListViewValue(BaseParams baseParams)
        {
            var objectCrId = baseParams.Params.GetAsId("objectCrId");

            var programCrChange = TypeChangeProgramCr.Manually;
            var objectCr = this.objectCrService.Get(objectCrId);
            if (objectCr.Return(x => x.ProgramCr).Return(x => x.Id) > 0)
            {
                programCrChange = this.programCrChangeService
                    .GetAll()
                    .Where(x => x.ProgramCr.Id == objectCr.ProgramCr.Id)
                    .OrderByDescending(x => x.ChangeDate)
                    .FirstOrDefault().Return(x => x.TypeChange);
            }

            if (programCrChange == TypeChangeProgramCr.Manually)
            {
                return new BaseDataResult();
            }

            return new BaseDataResult(container.GetGkhConfig<GkhCrConfig>().DpkrConfig.TypeDefectListView);
        }
    }
}