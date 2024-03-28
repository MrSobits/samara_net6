namespace Bars.Gkh.Regions.Nnovgorod.DomainService.Impl
{
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using System.Linq;

    using Castle.Windsor;

    public class ContragentInfoService : IContragentInfoService
    {

        public virtual IWindsorContainer Container { get; set; }

        public IDataResult GetActivityInfo(B4.BaseParams baseParams)
        {
            var contragentService = Container.Resolve<IDomainService<Contragent>>();
            
            try
            {
                var id = baseParams.Params.GetAs("contragentId", 0L);

                var obj = contragentService.GetAll().FirstOrDefault(x => x.Id == id);

                var info = "Нет";

                if (obj != null)
                {
                    info = obj.ContragentState == ContragentState.Liquidated ? "Да" : "Нет";
                }
                // делаю возвращение объекат поскольку в дальнейшем возможно что будет больше полей возвращается, чтобы на клиенте не упало
                return new BaseDataResult(new { info });
            }
            finally 
            {
                Container.Release(contragentService);
            }
        }
    }
}
