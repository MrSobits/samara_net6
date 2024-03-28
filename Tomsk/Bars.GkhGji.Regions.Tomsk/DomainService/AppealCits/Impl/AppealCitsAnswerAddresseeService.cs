namespace Bars.GkhGji.Regions.Tomsk.DomainService.Impl
{
    using System.Collections.Generic;

    using B4.Utils;
    using System.Linq;
    using B4;

    using Bars.B4.DataAccess;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    using GkhGji.Entities;
    using Castle.Windsor;

    public class AppealCitsAnswerAddresseeService : IAppealCitsAnswerAddressee
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddAddressee(BaseParams baseParams)
        {

            var service = Container.Resolve<IDomainService<AppealCitsAnswerAddressee>>();

            try
            {
                var answerId = baseParams.Params.GetAs("answerId",0L);
                var objectIds = baseParams.Params.GetAs<long[]>("objectIds");

                var listToSave = new List<AppealCitsAnswerAddressee>();

                // В этом словаре будет существующие адресаты
                // key - идентификатор адресата
                // value - Id самой записи
                var dict =
                    service.GetAll()
                        .Where(x => x.Answer.Id == answerId)
                        .Select(x => new { x.Id, AddresseeId = x.Addressee.Id })
                        .AsEnumerable()
                        .GroupBy(x => x.AddresseeId)
                        .ToDictionary(x => x.Key, y => y.Select(x => x.Id).FirstOrDefault());

                // По переданным id инспекторов если их нет в списке существующих, то добавляем
                foreach (var id in objectIds)
                {
                    var newId = id.ToLong();

                    if (dict.ContainsKey(newId))
                    {
                        continue;
                    }

                    if (newId > 0)
                    {
                        var newObj = new AppealCitsAnswerAddressee
                        {
                            Answer = new AppealCitsAnswer { Id = answerId },
                            Addressee = new RevenueSourceGji { Id = newId }
                        };

                        listToSave.Add(newObj);
                    }
                }

                using (var transaction = Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        listToSave.ForEach(service.Save);

                        transaction.Commit();
                        return new BaseDataResult();
                    }
                    catch (ValidationException e)
                    {
                        transaction.Rollback();
                        return new BaseDataResult { Success = false, Message = e.Message };
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}
