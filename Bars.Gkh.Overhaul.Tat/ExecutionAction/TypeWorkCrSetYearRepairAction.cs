namespace Bars.Gkh.Overhaul.Tat.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class TypeWorkCrSetYearRepairAction : BaseExecutionAction
    {
        public IDomainService<TypeWorkCrVersionStage1> typeWorkVersionDomain { get; set; }

        public override string Name => "ДПКР - Проставление года ремонта для видов работ";

        public override string Description
            => @"Для записей, которые созданы из ДПКР проставляется год ремонта из РЕзультатов корректировки. 
                    При повторном выполнении действия старые значения будт затиратся значениями из ДПКР 
                    (То ест ьесли пользователб ввел уже другео значение то перетрется значением из ДПКР)";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            // Получаем год неоюходимый для вида работы
            var data = this.typeWorkVersionDomain.GetAll()
                .Select(
                    x =>
                        new
                        {
                            typeWorkId = x.TypeWorkCr.Id,
                            x.Stage1Version.Stage2Version.Stage3Version.CorrectYear
                        })
                .ToList();

            var session = this.Container.Resolve<ISessionProvider>().OpenStatelessSession();

            using (var tr = session.BeginTransaction())
            {
                try
                {
                    foreach (var item in data)
                    {
                        session.CreateSQLQuery(@"UPDATE cr_obj_type_work 
                                                 SET year_repair = :year
                                                 WHERE id = :twId")
                            .SetParameter("year", item.CorrectYear)
                            .SetParameter("twId", item.typeWorkId)
                            .ExecuteUpdate();
                    }

                    tr.Commit();
                }
                catch (Exception exc)
                {
                    tr.Rollback();
                    throw exc;
                }
            }

            return new BaseDataResult();
        }
    }
}