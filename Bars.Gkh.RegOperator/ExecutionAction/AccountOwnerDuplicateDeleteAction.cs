namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Microsoft.Extensions.Logging;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;

    public class AccountOwnerDuplicateDeleteAction : BaseExecutionAction
    {
        public override string Description => "Удаление дубликатов абонентов";

        public override string Name => "Удаление дубликатов абонентов (физ. лица)";

        public override Func<IDataResult> Action => this.Execute;

        public ILogger Logger { get; set; }

        private BaseDataResult Execute()
        {
            var domain = this.Container.ResolveDomain<IndividualAccountOwner>();

            var accs = domain.GetAll()
                .Select(
                    x => new
                    {
                        x.Id,
                        x.Name
                    })
                .ToList()
                .GroupBy(x => x.Name)
                .Where(x => x.Count() > 1)
                .SelectMany(x => x.Skip(1))
                .ToList();

            bool wasError = false;
            accs.ForEach(
                x =>
                {
                    try
                    {
                        domain.Delete(x.Id);
                    }
                    catch (Exception e)
                    {
                        this.Logger.LogError(e, "Ошибка удаления абонента из действия по удалению дубликатов");
                        wasError = true;
                    }
                });

            if (wasError)
            {
                return new BaseDataResult(false, "Не все дубликаты абонентов были удалены");
            }

            return new BaseDataResult();
        }
    }
}