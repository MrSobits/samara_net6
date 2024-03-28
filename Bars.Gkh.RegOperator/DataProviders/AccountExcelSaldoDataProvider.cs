namespace Bars.Gkh.RegOperator.DataProviders
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.Gkh.RegOperator.DataProviders.Meta;

    using Castle.Windsor;

    /// <summary>
    /// Экспорт сальдо в Excel
    /// </summary>
    public class AccountExcelSaldoDataProvider : BaseCollectionDataProvider<PersonalAccountSaldoInfo>
    {

        private IList<PersonalAccountSaldoInfo> personalAccountSaldoInfo;

        /// <summary>
        /// .ctor
        /// </summary>
        public AccountExcelSaldoDataProvider(IWindsorContainer container, IList<PersonalAccountSaldoInfo> personalAccountSaldoInfos)
            : base(container)
        {
            this.personalAccountSaldoInfo = personalAccountSaldoInfos;
        }

        /// <summary>
        /// Метод получения данных.
        /// К этим данным будут применены фильтры.
        /// </summary>
        /// <returns><see cref="IQueryable{T}"/>.</returns>
        protected override IQueryable<PersonalAccountSaldoInfo> GetDataInternal(BaseParams baseParams)
        {
            return this.personalAccountSaldoInfo.AsQueryable();
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Выгрузка сальдо";

        /// <summary>
        /// Описание
        /// </summary>
        public override string Description => this.Name;

        /// <summary>
        /// Является ли данный поставщик скрытым.
        /// Если поставщик скрытый, то его нельзя использовать
        /// для создания хранимых источников данных.
        /// </summary>
        public override bool IsHidden => true;
    }
}