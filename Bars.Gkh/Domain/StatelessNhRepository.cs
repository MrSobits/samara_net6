namespace Bars.Gkh.Domain
{
    using System.Linq;
    using B4.DataAccess;
    using B4.Utils.Annotations;
    using NHibernate;
    using NHibernate.Linq;

    public class StatelessNhRepository<T> : IRepository<T> where T: IEntity
    {
        private readonly IStatelessSession _session;

        public StatelessNhRepository(IStatelessSession session)
        {
            ArgumentChecker.NotNull(session, "session");

            _session = session;
        }

        public void Evict(object entity)
        {
            
        }

        /// <summary>
        /// Удалить объект
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        void IRepository<T>.Delete(object id)
        {
            _session.Delete(id);
        }

        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <returns>
        /// Экземпляр объекта
        /// </returns>
        public T Get(object id)
        {
            return default(T);
        }

        /// <summary>
        /// Метод для формирования запроса. Перед вызовом необходимо открыть транзакцию
        /// </summary>
        /// <returns>
        /// Коллекцию объектов типа IQueryable
        /// </returns>
        public IQueryable<T> GetAll()
        {
            return _session.Query<T>();
        }

        /// <summary>
        /// Получить прокси объекта
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <returns>
        /// Прокси объекта
        /// </returns>
        public T Load(object id)
        {
            return _session.Get<T>(id);
        }

        /// <summary>
        /// Создать объект
        /// </summary>
        /// <param name="value">Создаваемый объект</param>
        public void Save(T value)
        {
            ArgumentChecker.NotNull(value, "value");

            if ((long)value.Id > 0)
            {
                _session.Update(value);
            }
            else
            {
                _session.Insert(value);
            }
        }

        /// <summary>
        /// Сохранить изменения объекта
        /// </summary>
        /// <param name="value">Сохраняемый объект</param>
        public void Update(T value)
        {
            ArgumentChecker.NotNull(value, "value");

            if ((long)value.Id > 0)
            {
                _session.Update(value);
            }
            else
            {
                _session.Insert(value);
            }
        }

        public void Evict(T entity)
        {
        }

        void IRepository.Delete(object id)
        {
            _session.Delete(Get(id));
        }

        object IRepository.Get(object id)
        {
            return Get(id);
        }

        IQueryable IRepository.GetAll()
        {
            return GetAll();
        }

        object IRepository.Load(object id)
        {
            return Load(id);
        }

        public void Save(object value)
        {
            Save((T)value);
        }

        public void Update(object value)
        {
            Update((T)value);
        }
    }
}
