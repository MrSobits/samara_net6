namespace Bars.Gkh.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Сервис получения <see cref="DuOuUslugaProxy"/>
    /// </summary>
    public class DuOuUslugaSelectorService : BaseProxySelectorService<DuOuUslugaProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, DuOuUslugaProxy> GetCache()
        {
            throw new NotImplementedException("Экспорт секции DUOUUSLUGA не реализован");
        }
    }
}