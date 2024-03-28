namespace Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;

    using Bars.B4.Utils;

    using Entities;

    public class SetSignOperation : PersonalAccountOperationBase
    {
        public IDomainService<BasePersonalAccount> PersonalAccountDomain { get; set; }

       
        public static string Key
        {
            get { return "SetSignOperation"; }
        }

        public override string Name
        {
            get { return "Установить признаки рассрочки и недолжника"; }
        }

        public override string Code
        {
            get { return Key; }
        }

        public override IDataResult Execute(BaseParams baseParams)
        {
            var accIds = baseParams.Params.GetAs<long[]>("accIds").ToList();
            var isNotDebtor = baseParams.Params.GetAs<bool>("IsNotDebtor");
            var installmentPlan = baseParams.Params.GetAs<bool>("InstallmentPlan");
            
            var accs = PersonalAccountDomain.GetAll().WhereContains(x => x.Id,accIds).ToList();

            return SetSign(accs, isNotDebtor, installmentPlan);
        }

        private IDataResult SetSign(List<BasePersonalAccount> accs, bool isNotDebtor, bool installmentPlan)
        {
            try
            {
                foreach (var acc in accs)
                {
                    acc.IsNotDebtor = isNotDebtor;
                    acc.InstallmentPlan = installmentPlan;
                    PersonalAccountDomain.Update(acc);
                }
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);;
            }


            return new BaseDataResult(true, "Признаки успешно установлены");
        }
    }
}