namespace Bars.GkhGji.Regions.Nnovgorod.StateChange
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Castle.Windsor;

    public class AppealNumberValidationRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        public string Id 
        {
            get { return "gji_appeal_number_validation_rule_nnovgorod"; }
        }

        public string Name
        {
            get { return "Проверка формирования номера обращения НН"; }
        }

        public string TypeId
        {
            get { return "gji_appeal_citizens"; }
        }

        public string Description
        {
            get { return "Данное правило проверяет возможность перевода статуса обращения граждан"; }
        }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var appeal = statefulEntity as AppealCits;

            if (appeal == null)
            {
                return ValidateResult.No("Не удалось привести сущность к типу AppealCits");
            }

            if (!appeal.DateFrom.HasValue)
            {
                return ValidateResult.No("Не указана дата обращения");
            }

            var year = appeal.DateFrom.Value.Year;

            var service = Container.Resolve<IDomainService<AppealCits>>();

            try
            {
                var maxIntNums = service.GetAll()
                    .Where(x => x.Id != appeal.Id)
                    .Where(x => x.DateFrom.HasValue && x.DateFrom.Value.Year == year)
                    .Select(x => new
                    {
                        x.IntNumber,
                        x.IntSubnumber
                    })
                    .ToArray();

                int maxIntNumber = 0;
                int maxIntSubnumber = 0;

                if (maxIntNums.Any())
                {
                    maxIntNumber = appeal.IntNumber > 0 ? appeal.IntNumber : maxIntNums.Max(x => x.IntNumber) + 1;
                    maxIntSubnumber = appeal.IntSubnumber > 0 ? appeal.IntSubnumber : maxIntNums.Max(x => x.IntSubnumber) + 1;
                }

                maxIntNumber = Math.Max(maxIntNumber, 1);
                maxIntSubnumber = Math.Max(maxIntSubnumber, 1);

                string number = null;

                if (appeal.Accepting != Accepting.NotSet)
                {
                    if (appeal.ZonalInspection != null)
                    {
                        number = string.Format("{0}{1}/{2}ж",
                            maxIntNumber,
                            appeal.ZonalInspection.AppealCode,
                            maxIntSubnumber);
                    }
                    else
                    {
                        //обнуляем номер, т.к. используется подномер
                        maxIntNumber = 0;

                        number = maxIntSubnumber + "ж";
                    }
                }
                else
                {
                    if (appeal.ZonalInspection != null)
                    {
                        //обнуляем подномер, т.к. используется номер
                        maxIntSubnumber = 0;

                        number = maxIntNumber + appeal.ZonalInspection.AppealCode;
                    }
                    else
                    {
                        maxIntNumber = 0;
                        maxIntSubnumber = 0;
                    }
                }

                appeal.DocumentNumber = number;
                appeal.IntNumber = maxIntNumber;
                appeal.IntSubnumber = maxIntSubnumber;
            }
            finally
            {
                Container.Release(service);
            }

            return ValidateResult.Yes();
        }
    }
}