namespace Bars.Gkh.RegOperator.Imports.Ches.PreImport
{
    using Bars.B4;

    using Castle.Windsor;

    /// <summary>
    /// Билдер обработчика импорта с временными таблицами
    /// </summary>
    public class ChesTempDataProviderBuilder : IChesTempDataProviderBuilder
    {
        private BaseParams builderParams;

        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Задать параметры
        /// </summary>
        /// <param name="baseParams"></param>
        public IChesTempDataProviderBuilder SetParams(BaseParams baseParams)
        {
            this.builderParams = baseParams;

            return this;
        }

        /// <summary>
        /// Получить импортер
        /// </summary>
        /// <param name="fileInfo"><see cref="ImportFileInfo"/></param>
        public IChesTempDataProvider Build(IPeriodImportFileInfo fileInfo)
        {
            var baseParams = this.builderParams ?? new BaseParams();
            switch (fileInfo.FileType)
            {
                case FileType.Calc:
                    return new ChesTempCalcProvider(this.Container, fileInfo as CalcFileInfo, baseParams);
                case FileType.Pay:
                    return new ChesTempPaymentsProvider(this.Container, fileInfo as PayFileInfo, baseParams);
                default:
                    return new ChesTempDataProvider(this.Container, fileInfo, baseParams);
            }
        }
    }
}