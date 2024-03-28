namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System.Linq;
    using B4.DataAccess;
    using B4.Utils;
    using Bars.B4.IoC;
    using Bars.Gkh.Modules.RegOperator.DomainService;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;

    using Castle.Windsor;
    using Gkh.Entities;
    using Gkh.Enums;
    using NHibernate.Linq;

    public class RegopService : IRegopService
    {
        public IWindsorContainer Container { get; set; }

        public RegOperator GetCurrentRegOperator()
        {
            var regopDomain = this.Container.ResolveDomain<RegOperator>();
            try
            {
                using (this.Container.Using(regopDomain))
                {
                    return regopDomain.GetAll()
                        .Fetch(x => x.Contragent)
                        .FirstOrDefault(x => x.Contragent.ContragentState == ContragentState.Active);
                }
            }
            finally
            {
                this.Container.Release(regopDomain);
            }
        }

        public Contragent GetRegopContragent(long regopId)
        {
            var regopDomain = this.Container.ResolveDomain<RegOperator>();
            try
            {
                using (this.Container.Using(regopDomain))
                {
                    return regopDomain.GetAll().FirstOrDefault(x => x.Id == regopId).Return(x => x.Contragent);
                }
            }
            finally
            {
                this.Container.Release(regopDomain);
            }
        }
    }
}