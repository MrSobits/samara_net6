namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Overhaul.Hmao.DomainService;
    using Bars.Gkh.Overhaul.Hmao.Entities.PriorityParams;
    using Bars.Gkh.Overhaul.Hmao.Enum;

    using Castle.Windsor;

    public class PriorityParamAdditionService : IPriorityParamAdditionService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetValue(BaseParams baseParams)
        {
            var code = baseParams.Params["Code"].ToString();
            var domain = Container.Resolve<IDomainService<PriorityParamAddition>>();
            var entity = domain.GetAll().FirstOrDefault(x => x.Code == code);
            if (entity == null)
            {
                entity = new PriorityParamAddition()
                             {
                                 Code = code,
                                 AdditionFactor = PriorityParamAdditionFactor.NotUsing,
                                 FinalValue = PriorityParamFinalValue.PointScore
                             };
                domain.Save(entity);
            }
            return new ListDataResult(entity,1);
        }
    }
}