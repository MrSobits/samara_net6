using Bars.B4;
using Bars.Gkh1468.Entities;
namespace Bars.Gkh1468.DomainService.Passport.Impl
{
    using System.Linq;

    using Bars.B4.DataAccess;

    /// <summary>
    /// Реализация интерфейса для работы с разделами структур паспортов
    /// </summary>
    public class StructPartService : BaseDomainService<Part>, IStructPartService
    {
        /// <summary>
        /// Удаляет переданный раздел
        /// </summary>
        /// <param name="id">Идентификатор раздела</param>
        /// <returns>Результат операции</returns>
        public IDataResult RemovePart(long id)
        {
            var domain = Container.ResolveDomain<Part>();
            var part = domain.Get(id);

            IDataResult operationResult = null;
            InTransaction(
                () => operationResult = RemoveChilds(part, domain)
            );

            return operationResult;
        }

        private IDataResult RemoveChilds(Part part, IDomainService<Part> partService)
        {
            var childs = partService.GetAll().Where(x => x.Parent != null && x.Parent.Id == part.Id).ToList();
            foreach (var child in childs)
            {
                RemoveAttributes(child);
                RemoveChilds(child, partService);
            }

            IDataResult removeAttributeResult = RemoveAttributes(part);
            if (!removeAttributeResult.Success)
            {
                return removeAttributeResult;
            }

            partService.Delete(part.Id);

            return new BaseDataResult { Success = true };
        }

        private IDataResult RemoveAttributes(Part part)
        {
            var serv = Container.Resolve<IDomainService<MetaAttribute>>();
            var atrIds = serv.GetAll()
                .Where(x => x.ParentPart != null
                    && x.ParentPart.Id == part.Id
                    && x.Parent == null)
                .Select(x => x.Id)
                .ToList();

            foreach (var atrId in atrIds)
            {
                var result = Container.Resolve<IMetaAttributeService>().RemoveMetaAttribute(atrId, serv);
                if (!result.Success)
                {
                    return result; 
                }
            }

            return new BaseDataResult { Success = true }; 
        }


    }
}
