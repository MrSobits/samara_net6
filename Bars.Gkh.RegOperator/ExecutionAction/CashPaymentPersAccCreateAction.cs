namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Microsoft.Extensions.Logging;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;

    using NHibernate.Linq;

    public class CashPaymentPersAccCreateAction : BaseExecutionAction
    {
        public override string Description => "Создать связь РКЦ с лицевыми счетами на основе ранее существовавших связей с домами";

        public override string Name => "Создать связь РКЦ с лицевыми счетами на основе ранее существовавших связей с домами";

        public override Func<IDataResult> Action => this.Execute;

        public ILogger Logger { get; set; }

        private BaseDataResult Execute()
        {
            var cpcRealObjDomain = this.Container.ResolveDomain<CashPaymentCenterRealObj>();
            var persAccDomain = this.Container.ResolveDomain<BasePersonalAccount>();

            try
            {
                var roContracts = cpcRealObjDomain.GetAll()
                    .Fetch(x => x.CashPaymentCenter)
                    .Fetch(x => x.RealityObject)
                    .ToList();

                var persAccIdByRo = persAccDomain.GetAll()
                    .Select(
                        x => new
                        {
                            x.Id,
                            RoId = x.Room.RealityObject.Id
                        })
                    .ToList()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Id).ToList());

                var listToSave = new List<CashPaymentCenterPersAcc>();
                foreach (var roContract in roContracts)
                {
                    var accIds = persAccIdByRo.Get(roContract.Id) ?? new List<long>();

                    foreach (var accId in accIds)
                    {
                        listToSave.Add(
                            new CashPaymentCenterPersAcc
                            {
                                CashPaymentCenter = roContract.CashPaymentCenter,
                                PersonalAccount = persAccDomain.Load(accId),
                                DateStart = roContract.DateStart,
                                DateEnd = roContract.DateEnd
                            });
                    }
                }

                TransactionHelper.InsertInManyTransactions(this.Container, listToSave, 10000, true, true);
            }
            finally
            {
                this.Container.Release(cpcRealObjDomain);
                this.Container.Release(persAccDomain);
            }

            return new BaseDataResult();
        }
    }
}