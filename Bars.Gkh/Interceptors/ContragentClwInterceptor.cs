using System.Linq;
using Bars.B4;
using Bars.Gkh.Entities;

namespace Bars.Gkh.Interceptors
{
    using System;

    using Bars.B4.Utils;

    public class ContragentClwInterceptor : EmptyDomainInterceptor<ContragentClw>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ContragentClw> service, ContragentClw entity)
        {
            return CheckContragent(service, entity)
                       ? Failure("Для указанного контрагента уже существует контрагент ПИР.")
                       : Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ContragentClw> service, ContragentClw entity)
        {
            return CheckContragent(service, entity)
                       ? Failure("Для указанного контрагента уже существует контрагент ПИР.")
                       : Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ContragentClw> service, ContragentClw entity)
        {
            var contragentClwMunicServ = Container.Resolve<IDomainService<ContragentClwMunicipality>>();

            try
            {
                contragentClwMunicServ.GetAll().Where(x => x.ContragentClw.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => contragentClwMunicServ.Delete(x));

                return Success();
            }
            catch (Exception)
            {
                return Failure("Не удалось удалить связанные записи");
            }
            finally
            {
                Container.Release(contragentClwMunicServ);
            }
        }

        private bool CheckContragent(IDomainService<ContragentClw> service, ContragentClw entity)
        {
            return
                service.GetAll()
                    .Any(x => x.Contragent.Id == entity.Contragent.Id && x.Id != entity.Id);
        }
    }
}