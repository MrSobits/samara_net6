namespace Bars.Gkh.GeneralState
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Bars.B4.IoC;
    using Bars.B4.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Провайдер для хранения описателей обобщенных состояний
    /// </summary>
    public class GeneralStateProvider : IGeneralStateProvider
    {
        private IReadOnlyDictionary<string, GeneralStatefulEntityInfo> entityInfoDict;
        private readonly IWindsorContainer container;

        public GeneralStateProvider(IWindsorContainer container)
        {
            this.container = container;
        }
 
        /// <inheritdoc />
        public void Init()
        {
            var data = new Dictionary<string, GeneralStatefulEntityInfo>();

            this.container.UsingForResolvedAll<IGeneralStatefulEntityManifest>((cnt, entityManifestList) =>
            {
                entityManifestList.ForEach(x => x.GetAllInfo().ForEach(y => data.Add(y.Code, y)));
            });

            this.entityInfoDict = new ReadOnlyDictionary<string, GeneralStatefulEntityInfo>(data);
        }

        /// <inheritdoc />
        public IEnumerable<KeyValuePair<string, GeneralStatefulEntityInfo>> GetStatefulInfos() => this.entityInfoDict;      
    }
}