namespace Bars.GkhGji.Regions.Saha.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.Gkh.ExecutionAction;

    using Castle.Windsor;

    public class SetFixedForDpkrStatusAction : BaseExecutionAction
    {
        
        public IRepository<RealityObject> RealityObjectRepo { get; set; }
        public IDomainService<State> StateDomain { get; set; }
        public IStateProvider StateProvider { get; set; }

        public override string Description => "Присваивание жилым домам статуса 'Зафиксировано для ДПКР', если в паспорте дома НЕ отмечен параметр 'Дом не участвует в ДПКР'";

        public override string Name => "Присваивание жилым домам статуса 'Зафиксировано для ДПКР', если в паспорте дома НЕ отмечен параметр 'Дом не участвует в ДПКР'";

        public override Func<IDataResult> Action => this.SetFixedForDpkrStatus;

        private BaseDataResult SetFixedForDpkrStatus()
        {
            var fixedForDpkrState = StateDomain.GetAll().FirstOrDefault(x => x.Name == "Зафиксировано для ДПКР");

            if (fixedForDpkrState == null)
            {
                return new BaseDataResult { Success = false, Message = "Статус 'Зафиксировано для ДПКР' для жилого дома отсутствует в системе" };
            }
            var listRoToUpdState = RealityObjectRepo.GetAll()
                .Where(x => !x.IsNotInvolvedCr && x.State != fixedForDpkrState)
                .ToList();

            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var ro in listRoToUpdState)
                    {
                        StateProvider.ChangeState(
                            ro.Id, 
                            "gkh_real_obj", 
                            fixedForDpkrState,
                            "Выполнение действия по смене статуса, если в паспорте дома НЕ отмечен параметр 'Дом не участвует в ДПКР'", 
                            true);
                    }

                    Gkh.Domain.TransactionHelper.InsertInManyTransactions(Container, listRoToUpdState, listRoToUpdState.Count, true, true);

                    transaction.Commit();
                    return new BaseDataResult { Success = true };
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return new BaseDataResult { Success = false, Message = e.Message };
                }
            }
        }
    }
}