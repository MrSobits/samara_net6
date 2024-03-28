namespace Bars.Gkh.RegOperator.Interceptors.FsGorod
{
    using B4;
    using B4.Utils;
    using Entities;
    using System.Linq;

    public class FsGorodImportInfoInterceptor : EmptyDomainInterceptor<FsGorodImportInfo>
    {
        public override IDataResult BeforeCreateAction(IDomainService<FsGorodImportInfo> service, FsGorodImportInfo entity)
        {
            if (service.GetAll().Any(x => x.Code == entity.Code))
            {
                return Failure("Уже есть настройка импорта с таким кодом");
            }

            if (entity.Delimiter.IsEmpty())
            {
                entity.Delimiter = ";";
            }

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<FsGorodImportInfo> service, FsGorodImportInfo entity)
        {
            if (service.GetAll().Any(x => x.Code == entity.Code && x.Id != entity.Id))
            {
                return Failure("Уже есть настройка импорта с таким кодом");
            }

            if (entity.Delimiter.IsEmpty())
            {
                entity.Delimiter = ";";
            }

            return base.BeforeUpdateAction(service, entity);
        }
    }
}