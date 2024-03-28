namespace Bars.Gkh.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class BelayPolicyInterceptor : EmptyDomainInterceptor<BelayPolicy>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<BelayPolicy> service, BelayPolicy entity)
        {
            var belayPolicyEventServ = Container.Resolve<IDomainService<BelayPolicyEvent>>();
            var belayPolicyMkdServ = Container.Resolve<IDomainService<BelayPolicyMkd>>();
            var belayPolicyPaymServ = Container.Resolve<IDomainService<BelayPolicyPayment>>();
            var belayPolicyRiskServ = Container.Resolve<IDomainService<BelayPolicyRisk>>();

            try
            {
                var belayPolicyEventList =
                    belayPolicyEventServ.GetAll().Where(x => x.BelayPolicy.Id == entity.Id).Select(x => x.Id).ToArray();
                var belayPolicyMkdList =
                    belayPolicyMkdServ.GetAll().Where(x => x.BelayPolicy.Id == entity.Id).Select(x => x.Id).ToArray();
                var belayPolicyPaymList =
                    belayPolicyPaymServ.GetAll().Where(x => x.BelayPolicy.Id == entity.Id).Select(x => x.Id).ToArray();
                var belayPolicyRiskList =
                    belayPolicyRiskServ.GetAll().Where(x => x.BelayPolicy.Id == entity.Id).Select(x => x.Id).ToArray();
                foreach (var id in belayPolicyEventList)
                {
                    belayPolicyEventServ.Delete(id);
                }
                foreach (var id in belayPolicyMkdList)
                {
                    belayPolicyMkdServ.Delete(id);
                }
                foreach (var id in belayPolicyPaymList)
                {
                    belayPolicyPaymServ.Delete(id);
                }
                foreach (var id in belayPolicyRiskList)
                {
                    belayPolicyRiskServ.Delete(id);
                }

                return Success();
            }
            catch (Exception)
            {
                return Failure("Не удалось удалить связанные записи");
            }
            finally
            {
                Container.Release(belayPolicyEventServ);
                Container.Release(belayPolicyMkdServ);
                Container.Release(belayPolicyPaymServ);
                Container.Release(belayPolicyRiskServ);
            }
        }
    }
}
