namespace Bars.Gkh.RegOperator.Export
{
    using System.Collections;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.RegOperator.Entities;

    public class BasePersonalAccountExport : BaseDataExportService
    {
        #region Constructors and Destructors

        public BasePersonalAccountExport(IViewModel<BasePersonalAccount> viewModel, IDomainService<BasePersonalAccount> domainService)
        {
            this.ViewModel = viewModel;
            this.DomainService = domainService;
        }

        #endregion

        #region Properties

        private IDomainService<BasePersonalAccount> DomainService { get; set; }

        private IViewModel<BasePersonalAccount> ViewModel { get; set; }

        #endregion

        #region Public Methods and Operators

        public override IList GetExportData(BaseParams baseParams)
        {
            baseParams.Params["start"] = 0;
            baseParams.Params["limit"] = 0;

            return this.ViewModel.List(this.DomainService, baseParams).Data as IList;
        }

        #endregion
    }
}