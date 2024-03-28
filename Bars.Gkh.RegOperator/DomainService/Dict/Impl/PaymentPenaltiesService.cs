namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Castle.Windsor;
    using Entities;
    using Entities.Dict;
    using Gkh.Domain;
    using Gkh.Utils;

    public class PaymentPenaltiesService : IPaymentPenaltiesService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddExcludePersAccs(BaseParams baseParams)
        {
            var payPenaltiesExcPersAccDomain = Container.ResolveDomain<PaymentPenaltiesExcludePersAcc>();
            var paymentPenaltiesDomain = Container.ResolveDomain<PaymentPenalties>();
            var personalAccDomain = Container.ResolveDomain<BasePersonalAccount>();

            try
            {
                var payPenaltiesId = baseParams.Params.GetAs<long>("payPenaltiesId");
                var persAccIds = baseParams.Params.GetAs<long[]>("persAccIds");

                if (persAccIds.Length == 0)
                {
                    return new BaseDataResult(false, "Необходимо выбрать исключения");
                }

                if (payPenaltiesId == 0)
                {
                    return new BaseDataResult(false, "Необходимо сначала сохранить параметр расчета пеней");
                }

                var existRecs =
                    payPenaltiesExcPersAccDomain.GetAll()
                        .Where(x => x.PaymentPenalties.Id == payPenaltiesId)
                        .Select(x => x.PersonalAccount.Id)
                        .ToHashSet();

                var paymentPenalties = paymentPenaltiesDomain.Load(payPenaltiesId);
                persAccIds = persAccIds.Where(x => !existRecs.Contains(x)).ToArray();

                var recToSave = new List<PaymentPenaltiesExcludePersAcc>();
                foreach (var id in persAccIds)
                {
                    var newObj = new PaymentPenaltiesExcludePersAcc
                    {
                        PaymentPenalties = paymentPenalties,
                        PersonalAccount = personalAccDomain.Load(id)
                    };

                    recToSave.Add(newObj);
                }

                TransactionHelper.InsertInManyTransactions(Container, recToSave, 10000, true, true);

                return new BaseDataResult();
            }
            finally
            {
                Container.Release(payPenaltiesExcPersAccDomain);
                Container.Release(paymentPenaltiesDomain);
                Container.Release(personalAccDomain);
            }          
        }   
    }
}