Ext.define('B4.aspects.permission.typeworkcr.ContractCr', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.contracttypeworkcrperm',

    permissions: [
        //общие
        { name: 'GkhCr.TypeWorkCr.Register.ContractCr.Edit', applyTo: 'b4savebutton', selector: 'objectcrcontractwin' },
        
        //поля панели редактирования
        { name: 'GkhCr.TypeWorkCr.Register.ContractCr.Field.DocumentName_Edit', applyTo: '#tfDocumentName', selector: 'objectcrcontractwin' },
        { name: 'GkhCr.TypeWorkCr.Register.ContractCr.Field.DocumentNum_Edit', applyTo: '#tfDocumentNum', selector: 'objectcrcontractwin' },
        { name: 'GkhCr.TypeWorkCr.Register.ContractCr.Field.DateFrom_Edit', applyTo: '#dfDateFrom', selector: 'objectcrcontractwin' },
        { name: 'GkhCr.TypeWorkCr.Register.ContractCr.Field.TypeContractObject_Edit', applyTo: '#cbTypeContractObject', selector: 'objectcrcontractwin' },
        { name: 'GkhCr.TypeWorkCr.Register.ContractCr.Field.SumContract_Edit', applyTo: '#nfSumContract', selector: 'objectcrcontractwin' },
        {
            name: 'GkhCr.TypeWorkCr.Register.ContractCr.Field.BudgetMo_Edit', applyTo: '#tfBudgetMo', selector: 'objectcrcontractwin',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhCr.TypeWorkCr.Register.ContractCr.Field.BudgetSubject_Edit', applyTo: '#tfBudgetSubject', selector: 'objectcrcontractwin',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhCr.TypeWorkCr.Register.ContractCr.Field.OwnerMeans_Edit', applyTo: '#tfOwnerMeans', selector: 'objectcrcontractwin',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhCr.TypeWorkCr.Register.ContractCr.Field.FundMeans_Edit', applyTo: '#tfFundMeans', selector: 'objectcrcontractwin',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        { name: 'GkhCr.TypeWorkCr.Register.ContractCr.Field.File_Edit', applyTo: '#ffFile', selector: 'objectcrcontractwin' },
        { name: 'GkhCr.TypeWorkCr.Register.ContractCr.Field.Contragent_Edit', applyTo: '#sflContragent', selector: 'objectcrcontractwin' },
        { name: 'GkhCr.TypeWorkCr.Register.ContractCr.Field.FinanceSource_Edit', applyTo: '#sflFinanceSource', selector: 'objectcrcontractwin' },
        { name: 'GkhCr.TypeWorkCr.Register.ContractCr.Field.Description_Edit', applyTo: '#taDescription', selector: 'objectcrcontractwin' }
    ]
});