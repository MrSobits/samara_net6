namespace Bars.GkhRf.Interceptors
{
    using System;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;
    using Bars.GkhRf.Entities;

    public class RequestTransferRfServiceInterceptor : EmptyDomainInterceptor<RequestTransferRf>
    {
        public override IDataResult BeforeCreateAction(IDomainService<RequestTransferRf> service, RequestTransferRf entity)
        {
            int documentNum = 0;
            if (service.GetAll().Any())
            {
                documentNum = service.GetAll().Max(x => x.DocumentNum);
            }

            entity.DocumentNum = documentNum + 1;

            // Перед сохранением проставляем начальный статус
            var stateProvider = this.Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            Check(entity);

            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<RequestTransferRf> service, RequestTransferRf entity)
        {
            Check(entity);
            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<RequestTransferRf> service, RequestTransferRf entity)
        {
            var transfFundsRfServ = Container.Resolve<IDomainService<TransferFundsRf>>();

            try
            {
                transfFundsRfServ.GetAll().Where(x => x.RequestTransferRf.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => transfFundsRfServ.Delete(x));

                return Success();
            }
            catch (Exception)
            {
                return Failure("Не удалось удалить связанные записи");
            }
            finally
            {
                Container.Release(transfFundsRfServ);
            }
        }

        /// <summary>
        /// метод проверки соответствия циферок в заявках в регфонде и циферок в средствах источников финансирования в объекте кр
        /// </summary>
        /// <param name="service"></param>
        /// <param name="entity"></param>
        private void Check(RequestTransferRf entity)
        {
            var serviceFunds = Container.Resolve<IDomainService<TransferFundsRf>>();

            var curRequestRealObjs =
                serviceFunds.GetAll()
                            .Where(x => x.RequestTransferRf.Id == entity.Id)
                            .Select(x => x.RealityObject.Id)
                            .ToList();

            // если у текущей заявки нет жилых домов, то проверку не делаем
            if (curRequestRealObjs.Count == 0)
            {
                return;
            }

            var realObjs =
                serviceFunds.GetAll()
                    .Where(x => curRequestRealObjs.Contains(x.RealityObject.Id))
                    .Where(x => x.RequestTransferRf.ProgramCr.Id == entity.ProgramCr.Id  && x.RequestTransferRf.TypeProgramRequest == entity.TypeProgramRequest 
                        && x.RequestTransferRf.DateFrom.Value.Year == DateTime.Now.Year)
                    .Select(x => new
                    {
                        x.RealityObject.Id,
                        x.RealityObject.Address,
                        x.RequestTransferRf.DocumentNum,
                        x.Sum
                    })
                    .AsEnumerable()
                    .GroupBy(x => new { x.Id, x.Address })
                    .ToDictionary(
                        x => x.Key,
                        y =>
                        new
                            {
                                Docs = y.Aggregate("", (result, rec) => result + (!string.IsNullOrEmpty(result) ? ", " + rec.DocumentNum.ToString() : rec.DocumentNum.ToString())),
                                Sum = y.Select(z => z.Sum).Sum()
                            });

            var reposObjectCr = Container.Resolve<IRepository<ObjectCr>>(); // Так не делать!!! Использую репозиторий в силу того что нужно вызывать базовый GetaAll
            var serviceObjCrFinSources = Container.Resolve<IDomainService<FinanceSourceResource>>();
            var requestFinSources =
                Container.Resolve<IDomainService<LimitCheckFinSource>>()
                         .GetAll()
                         .Where(x => x.LimitCheck.TypeProgram == entity.TypeProgramRequest)
                         .Select(x => x.FinanceSource.Id)
                         .ToList();

            var limitMessage = new StringBuilder("Сумма в заявке по объекту: &lt;br&gt;");
            var programMessage = new StringBuilder("Следующие объекты: &lt;br&gt;");

            bool hasLimitMessage = false;
            bool hasProgramMessage = false;

            foreach (var ro in realObjs)
            {
                var objCr = reposObjectCr.GetAll().FirstOrDefault(x => x.RealityObject.Id == ro.Key.Id && x.ProgramCr.Id == entity.ProgramCr.Id);

                if (objCr == null)
                {
                    hasProgramMessage = true;
                    programMessage.AppendFormat("{0} &lt;br&gt;", ro.Key.Address);
                    continue;
                }

                var finSource =
                    serviceObjCrFinSources.GetAll().Where(x => requestFinSources.Contains(x.FinanceSource.Id));
                var finSourceResource =
                    finSource.Where(x => x.ObjectCr.Id == objCr.Id)
                             .Select(x => x.OwnerResource)
                             .Sum()
                             .GetValueOrDefault(0);

                if (finSource.Any() && finSourceResource < ro.Value.Sum)
                {
                    hasLimitMessage = true;
                    limitMessage.AppendFormat("{0} - заявка № {1}; &lt;br&gt;", ro.Key.Address, ro.Value.Docs);
                }
            }

            limitMessage.Append("превышает лимит по программе КР");
            programMessage.Append("не включены в указанную программу КР");

            if (hasLimitMessage || hasProgramMessage)
            {
                var message = hasLimitMessage ? limitMessage.ToString() : "";
                message += hasLimitMessage && hasProgramMessage ? "&lt;br&gt;&lt;br&gt;" : "";
                message += hasProgramMessage ? programMessage.ToString() : "";
                throw new ValidationException(message);
            }
        }
    }
}