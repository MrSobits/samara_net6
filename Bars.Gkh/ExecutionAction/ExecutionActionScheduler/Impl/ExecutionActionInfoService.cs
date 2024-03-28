namespace Bars.Gkh.ExecutionAction.ExecutionActionScheduler.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Application;
    using Bars.B4.IoC;
    using Bars.B4.Utils;

    using Castle.Core;

    public class ExecutionActionInfoService : IExecutionActionInfoService, IInitializable
    {
        private IDictionary<string, ExecutionActionInfo> executionActionInfoCache = new Dictionary<string, ExecutionActionInfo>();

        public ExecutionActionInfo GetInfo(string actionCode)
        {
            return this.executionActionInfoCache.Get(actionCode);
        }

        /// <inheritdoc />
        public IList<ExecutionActionInfo> GetInfoAll(bool includeHidden)
        {
            return this.executionActionInfoCache.Values
                .WhereIf(!includeHidden, x => !x.IsHidden)
                .OrderBy(x => x.Name)
                .ToList();
        }

        /// <inheritdoc />
        public void Initialize()
        {
            ApplicationContext.Current.Container.UsingForResolvedAll<IExecutionAction>((container, actions) =>
            {
                this.executionActionInfoCache = actions.Select(x => new ExecutionActionInfo
                    {
                        Code = x.Code,
                        Name = x.Name,
                        Description = x.Description,
                        IsHidden = Attribute.IsDefined(x.GetType(), typeof(HiddenActionAttribute))
                    })
                    .ToDictionary(x => x.Code);
            });
        }
    }
}