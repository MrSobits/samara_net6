namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class HeatInputPeriodInterceptor : EmptyDomainInterceptor<HeatInputPeriod>
    {
        private readonly IDomainService<HeatInputInformation> _heatInputInfoService;
        private readonly IDomainService<RealityObject> _realObjService;
        private readonly IDomainService<WorkInWinterMark> _workWinterMarkService;
        private readonly IDomainService<WorkWinterCondition> _workWinterConditionService; 

        public HeatInputPeriodInterceptor(
            IDomainService<HeatInputInformation> heatInputInfoService,
            IDomainService<RealityObject> realObjService,
            IDomainService<WorkInWinterMark> workWinterMarkService,
            IDomainService<WorkWinterCondition> workWinterConditionService)
        {
            _heatInputInfoService = heatInputInfoService;
            _realObjService = realObjService;
            _workWinterMarkService = workWinterMarkService;
            _workWinterConditionService = workWinterConditionService;
        }

        public override IDataResult BeforeCreateAction(IDomainService<HeatInputPeriod> service, HeatInputPeriod entity)
        {
            return
                service.GetAll()
                       .Any(x => x.Month == entity.Month && x.Year == entity.Year && x.Municipality == entity.Municipality)
                       ? Failure("Уже существует элемент за указанный период")
                       : Success();
        }

        public override IDataResult AfterCreateAction(IDomainService<HeatInputPeriod> service, HeatInputPeriod entity)
        {
            try
            {
                foreach (var type in (TypeHeatInputObject[])Enum.GetValues(typeof(TypeHeatInputObject)))
                {
                    var heatInputInfo = new HeatInputInformation
                    {
                        HeatInputPeriod = entity,
                        TypeHeatInputObject = type
                    };

                    _heatInputInfoService.Save(heatInputInfo);
                }
                
                // создаю данные для таблицы WorkWinterCondition для только что созданного периода
                var marks = _workWinterMarkService.GetAll().ToList();

                if (marks.Any())
                {
                    foreach (var mark in marks)
                    {
                        var workWinterCondition = new WorkWinterCondition
                        {
                            HeatInputPeriod = entity,
                            WorkInWinterMark = mark
                        };

                        _workWinterConditionService.Save(workWinterCondition);
                    }
                }

                return Success();
            }
            catch (Exception e)
            {
                return Failure(e.Message);
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<HeatInputPeriod> service, HeatInputPeriod entity)
        {
            this._workWinterConditionService.GetAll()
                .Where(x => x.HeatInputPeriod.Id == entity.Id)
                .Select(x => x.Id)
                .ToList()
                .ForEach(x => this._workWinterConditionService.Delete(x));

            this._heatInputInfoService.GetAll()
                .Where(x => x.HeatInputPeriod.Id == entity.Id)
                .Select(x => x.Id)
                .ToList()
                .ForEach(x => this._heatInputInfoService.Delete(x));

            return this.Success();
        }
    }
}