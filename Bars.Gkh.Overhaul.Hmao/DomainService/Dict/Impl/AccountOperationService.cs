namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System.Linq;
    using B4;
    using Castle.Windsor;
    using Entities;

    public class AccountOperationService : IAccountOperationService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult ListNoPaging(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var data =
                Container.Resolve<IDomainService<AccountOperation>>().GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        x.Name
                    })
                    .Filter(loadParams, Container)
                    .OrderBy(x => x.Name);

            return new ListDataResult(data.ToList(), data.Count());
        }
    }
}