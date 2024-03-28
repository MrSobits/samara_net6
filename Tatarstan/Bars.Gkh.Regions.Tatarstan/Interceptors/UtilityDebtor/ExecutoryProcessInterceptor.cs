namespace Bars.Gkh.Regions.Tatarstan.Interceptors.UtilityDebtor
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Regions.Tatarstan.Entities.UtilityDebtor;
    
    /// <summary>
    /// Интерцептор для исполнительного производства
    /// </summary>
    public class ExecutoryProcessInterceptor : EmptyDomainInterceptor<ExecutoryProcess>
    {
        /// <summary>Метод вызывается перед созданием объекта</summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Объект</param>
        /// <returns>Результат выполнения</returns>
        public override IDataResult BeforeCreateAction(IDomainService<ExecutoryProcess> service, ExecutoryProcess entity)
        {
            var stateProvider = this.Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);        

            return this.Success();
        }

        /// <summary>
        /// Метод вызывается перед удалением объекта
        /// </summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Объект</param>
        /// <returns> Результат выполнения </returns>
        public override IDataResult BeforeDeleteAction(IDomainService<ExecutoryProcess> service, ExecutoryProcess entity)
        {
            var documentDomain = this.Container.Resolve<IRepository<ExecutoryProcessDocument>>();

            try
            {
                //удаляем связанные документы
                var idsDoc = documentDomain.GetAll()
                    .Where(x => x.ExecutoryProcess.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList();
                foreach (var id in idsDoc)
                {
                    documentDomain.Delete(id);
                }

                return this.Success();
            }
            finally
            {
                this.Container.Release(documentDomain);
            }
        }
    }
}