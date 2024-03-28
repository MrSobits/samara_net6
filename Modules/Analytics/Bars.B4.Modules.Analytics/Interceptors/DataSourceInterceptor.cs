namespace Bars.B4.Modules.Analytics.Interceptors
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Entities;

    /// <summary>
    /// Интерсептор для валидации сущности DataSource
    /// </summary>
    public class DataSourceInterceptor : EmptyDomainInterceptor<DataSource>
    {
        public override IDataResult BeforeCreateAction(IDomainService<DataSource> service, DataSource entity)
        {
            if (service.GetAll().Any(e => entity.Name == e.Name))
            {
                return new BaseDataResult(false, "Источник данных с таким имененем уже создан");
            }

            return new BaseDataResult();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<DataSource> service, DataSource entity)
        {
            if (service.GetAll().Any(e => entity.Name == e.Name && e.Id != entity.Id))
            {
                return new BaseDataResult(false, "Источник данных с таким имененем уже создан");
            }

            return new BaseDataResult();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<DataSource> service, DataSource entity)
        {
            //проверяем, является ли удаляемый элемент родителем для кого-то
            var child = service.GetAll().FirstOrDefault(e => e.Parent != null && e.Parent.Id == entity.Id);
            if (child != null)
            {
                return new BaseDataResult(false, string.Format("Источник данных является родителем для источника данных \"{0}\"", child.Name));
            }

            return new BaseDataResult();
        }
    }
}
