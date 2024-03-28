namespace Bars.Gkh.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class ContragentContactServiceInterceptor : EmptyDomainInterceptor<ContragentContact>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ContragentContact> service, ContragentContact entity)
        {
            entity.FullName = string.Format("{0} {1} {2}", entity.Surname, entity.Name, entity.Patronymic);
            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ContragentContact> service, ContragentContact entity)
        {
            entity.FullName = string.Format("{0} {1} {2}", entity.Surname, entity.Name, entity.Patronymic);
            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ContragentContact> service, ContragentContact entity)
        {
            var managOrgServ = Container.Resolve<IDomainService<ManagingOrganization>>();

            try
            {
                managOrgServ.GetAll().Where(x => x.TsjHead.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => managOrgServ.Delete(x));

                return Success();
            }
            catch (Exception)
            {
                return Failure("Не удалось удалить связанные записи");
            }
            finally
            {
                Container.Release(managOrgServ);
            }
        }
    }
}