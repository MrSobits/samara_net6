namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using System.Linq;
    using B4.DataAccess;
    using Bars.Gkh.Enums;

    class SpecialAccountRowInterceptor : EmptyDomainInterceptor<SpecialAccountRow>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<ManOrgContractRealityObject> MorgRODomain { get; set; }

        public IDomainService<SpecialAccountRow> SpecialAccountReportRowDomain { get; set; }

        public IDomainService<SpecialAccountRow> SpecialAccountReportRowDomain2 { get; set; }

        public IDomainService<Contragent> ContragentDomain { get; set; }

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IDomainService<SpecialAccountReport> SpecialAccountReportDomain { get; set; }

        public override IDataResult BeforeUpdateAction(IDomainService<SpecialAccountRow> service, SpecialAccountRow entity)
        {
            Int32 year = Convert.ToInt32(entity.SpecialAccountReport.YearEnums.GetDisplayName());
            Int32 month = 1;
            Int32 factor = 1;
            switch (entity.SpecialAccountReport.MonthEnums)
            {
              
                case Enums.MonthEnums.Quarter1:
                    month = 4;
                    factor = 3;
                    break;
                case Enums.MonthEnums.Quarter2:
                    month = 7;
                    factor = 3;
                    break;
                case Enums.MonthEnums.Quarter3:
                    month = 10;
                    factor = 3;
                    break;
                case Enums.MonthEnums.Quarter4:
                    month = 1;
                    factor = 3;
                    break;
            }
            decimal charged = entity.Accured;
            decimal maxCharge = entity.AccuracyArea * 20 * factor;
            //if (charged > maxCharge)
            //{
            //    return Failure("Ошибка проверки данных. Слишком большая сумма начисления.");
            //}
            if (entity.IncomingTotal + entity.Incoming < entity.Transfer)
            {
                return Failure("Ошибка проверки данных. Слишком большая сумма трансфера. На счете недостаточно средств.");
            }
            if (!entity.StartDate.HasValue)
            {
                return Failure("Ошибка проверки данных. Не указана дата первого начисления");
            }
            DateTime dt = entity.StartDate.HasValue ? entity.StartDate.Value : new DateTime(2014, 12, 1);
            int months = (year - dt.Year) * 12 + month - dt.Month + 1;
            if (months < 1)
            {
                months = 1;
            }
            decimal maxChargeTotal = entity.AccuracyArea * 20 * months;
            decimal chargedTotal = entity.AccuredTotal;

            if (entity.Ballance == 0)
            {
                entity.Ballance = entity.IncomingTotal - entity.TransferTotal;
            }
            if (entity.AmmountDebt == 0)
            {
                entity.AmmountDebt = entity.AccuredTotal - entity.IncomingTotal;
            }
            if (entity.AmountDebtForPeriod == 0)
            {
                entity.AmountDebtForPeriod = entity.Accured - entity.Incoming;
            }
            //if (maxChargeTotal < chargedTotal)
            //{
            //    return Failure("Ошибка проверки данных. Слишком большая сумма начисления всего. Убедитесь что дата первого начисления и расчетная площадь указана верно.");
            //}

            //Новые проверки

            //if (entity.Accured == 0)
            //{
            //    return Failure("Ошибка проверки данных. Сведения о начислении взносов в отчетном периоде не могут быть равны нулю.");
            //}
            //if (entity.AccuredTotal < entity.Accured)
            //{
            //    return Failure("Ошибка проверки данных. Сведения о начислении взносов нарастающим итогом не могут быть меньше сведений о начислении в отчетном периоде.");
            //}


            return Success();
        }

    }
}
