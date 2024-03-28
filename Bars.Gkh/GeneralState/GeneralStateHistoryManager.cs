namespace Bars.Gkh.GeneralState
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.DataModels;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Менеждер для массового создания и сохранения истотрии смены обощенных состояний
    /// </summary>
    public class GeneralStateHistoryManager : IGeneralStateHistoryManager
    {
        public IWindsorContainer Container { get; set; }
        
        private readonly IDomainService<GeneralStateHistory> stateHistoryDomain;
        private readonly List<GeneralStateHistory> stateHistoryBuffer;
        private readonly IGeneralStateHistoryService stateService;
        private GeneralStatefulEntityInfo stateEntityInfo;

        public GeneralStateHistoryManager(IDomainService<GeneralStateHistory> stateHistoryDomain, IGeneralStateHistoryService stateService)
        {
            this.stateHistoryDomain = stateHistoryDomain;
            this.stateService = stateService;
            this.stateHistoryBuffer = new List<GeneralStateHistory>();
        }

        /// <inheritdoc />
        public void Init(Type entityType, string propertyName = null)
        {
            this.stateEntityInfo = this.stateService.GetStateHistoryInfo(entityType, propertyName);
        }

        /// <inheritdoc />
        public void CreateStateHistory(IHaveId entity, object oldValue, object newValue, bool stronglyTyped = true)
        {
            var stateHistory = stronglyTyped
                ? this.stateService.CreateStateHistory(entity, this.stateEntityInfo, oldValue, newValue)
                : this.stateService.CreateStateHistory(entity, oldValue, newValue);

            this.stateHistoryBuffer.Add(stateHistory);
        }

        /// <inheritdoc />
        public void SaveStateHistories()
        {
            if (!this.stateHistoryBuffer.Any())
            {
                return;
            }

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                this.stateHistoryBuffer.ForEach(x => this.stateHistoryDomain.Save(x));                  
                transaction.Commit();
                this.stateHistoryBuffer.Clear();
            }         
        }
    }
}