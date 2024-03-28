namespace Bars.Gkh.Modules.ClaimWork.DomainService.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Castle.Windsor;
    using B4;
    using B4.Utils;

    using Bars.B4.Modules.Tasks.Common.Service;

    using Castle.MicroKernel.Lifestyle;

    using Entities;
    using Enums;
    using Bars.B4.DataAccess;

    public class BaseClaimWorkService : BaseClaimWorkService<BaseClaimWork>
    {

    }

    public class BaseClaimWorkService<T> : IBaseClaimWorkService<T>
         where T : BaseClaimWork
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Диспетчер задач
        /// </summary>
        public ITaskManager TaskManager { get; set; }
        
        public virtual ClaimWorkTypeBase ClaimWorkTypeBase
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Метод получения списка используется в реестрах и экспорте
        /// </summary>
        public virtual IList GetList(BaseParams baseParams, bool isPaging, ref int totalCount)
        {
            var service = this.Container.Resolve<IDomainService<T>>();

            try
            {
                var loadParam = baseParams.GetLoadParam();

                var data = service.GetAll()
                            .Filter(loadParam, this.Container);

                totalCount = data.Count();

                return data.Order(loadParam).Paging(loadParam).ToList();
            }
            finally 
            {
                this.Container.Release(service);
            }
        }

        public void UpdateStatesWithNewScope()
        {
            using (this.Container.BeginScope())
            {
                this.UpdateStates(
                    new BaseParams { Params = new DynamicDictionary() }
                );
            }
        }

        /// <summary>
        /// Метод для обновления статусов оснований
        /// </summary>
        public virtual IDataResult UpdateStates(BaseParams baseParams, bool inTask = false)
        {
            return new BaseDataResult();
        }

        public virtual IDataResult PauseState(BaseParams baseParams)
        {
            return new BaseDataResult();
        }

        public virtual IDataResult MassCreateDocs(BaseParams baseParams)
        {
            return new BaseDataResult();
        }

        public virtual IDataResult GetDocsTypeToCreate()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ClaimWorkDocumentType> GetNeedDocs(BaseParams baseParams)
        {
            return (IEnumerable<ClaimWorkDocumentType>) Enum.GetValues(typeof(ClaimWorkDocumentType));
        }

        public void Execute(DynamicDictionary @params)
        {
            this.UpdateStatesWithNewScope();
        }
    }
}