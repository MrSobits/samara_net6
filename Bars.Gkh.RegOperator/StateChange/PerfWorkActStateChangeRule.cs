namespace Bars.Gkh.RegOperator.StateChange
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Правило перехода статуса акта выполненных работ
    /// </summary>
    public class PerfWorkActStateChangeRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<RealObjSupplierAccOperPerfAct> RealObjSupplierAccOperPerfActDomain { get; set; }

        public IDomainService<RealityObjectSupplierAccountOperation> RealObjSupplierAccOperDomain { get; set; }

        public IDomainService<RealityObjectSupplierAccount> RealObjSupplierAccDomain { get; set; }

        public string Id { get { return "cr_perf_work_act_update_oper_rule"; } }

        public string Name { get { return "Обновление данных операции счета расчета с поставщиками  в паспорте жилого дома"; } }

        public string TypeId { get { return "cr_obj_performed_work_act"; } }

        public string Description
        {
            get
            {
                return @"Данное правило обновляет поля 'Обороты по дебету' и 'Дата' в операции счета расчета с поставщиками  при смене статуса акта выполненных работ";
            }
        }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            if (statefulEntity is PerformedWorkAct)
            {
                var performedWorkAct = statefulEntity as PerformedWorkAct;

                //Получаем счет с поставщиками 
                var realObjSupplierAcc = RealObjSupplierAccDomain.GetAll().FirstOrDefault(x => x.RealityObject.Id == performedWorkAct.ObjectCr.RealityObject.Id);

                if (realObjSupplierAcc != null)
                {
                    //Получаем операцию  рассчета с поставщиками по акту выполненных работ
                    var realObjSupplierAccOper = RealObjSupplierAccOperPerfActDomain.GetAll()
                        .Where(x => x.PerformedWorkAct.Id == performedWorkAct.Id).Select(x => x.SupplierAccOperation).FirstOrDefault();

                    if (realObjSupplierAccOper == null)
                    {
                        realObjSupplierAccOper = new RealityObjectSupplierAccountOperation()
                        {
                            Date = performedWorkAct.DateFrom.ToDateTime(),
                            Account = realObjSupplierAcc,
                            Credit = 0,
                            OperationType = PaymentOperationType.OutcomeAccountPayment,
                            Debt = performedWorkAct.Sum.ToDecimal()
                        };

                        RealObjSupplierAccOperDomain.Save(realObjSupplierAccOper);

                        RealObjSupplierAccOperPerfActDomain.Save(
                            new RealObjSupplierAccOperPerfAct()
                            {
                                SupplierAccOperation = realObjSupplierAccOper,
                                PerformedWorkAct = performedWorkAct
                            });
                    }
                    else
                    {
                        realObjSupplierAccOper.Date = performedWorkAct.DateFrom.ToDateTime();
                        realObjSupplierAccOper.Debt = performedWorkAct.Sum.ToDecimal();

                        RealObjSupplierAccOperDomain.Update(realObjSupplierAccOper);
                    }
                }
            }

            return new ValidateResult { Success = true };
        }
    }
}