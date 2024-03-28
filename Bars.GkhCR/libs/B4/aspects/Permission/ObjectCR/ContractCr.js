Ext.define('B4.aspects.permission.objectcr.ContractCr', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.contractcrperm',

    permissions: [
        //общие
        { name: 'GkhCr.ObjectCr.Register.ContractCr.Edit', applyTo: 'b4savebutton', selector: 'objectcrcontractwin' },
        
        //поля панели редактирования
        { name: 'GkhCr.ObjectCr.Register.ContractCr.Field.DocumentName_Edit', applyTo: '#tfDocumentName', selector: 'objectcrcontractwin' },
        { name: 'GkhCr.ObjectCr.Register.ContractCr.Field.DocumentNum_Edit', applyTo: '#tfDocumentNum', selector: 'objectcrcontractwin' },
        { name: 'GkhCr.ObjectCr.Register.ContractCr.Field.DateFrom_Edit', applyTo: '#dfDateFrom', selector: 'objectcrcontractwin' },
        { name: 'GkhCr.ObjectCr.Register.ContractCr.Field.TypeContractObject_Edit', applyTo: '#cbTypeContractObject', selector: 'objectcrcontractwin' },
        { name: 'GkhCr.ObjectCr.Register.ContractCr.Field.SumContract_Edit', applyTo: '#nfSumContract', selector: 'objectcrcontractwin' },
        {
            name: 'GkhCr.ObjectCr.Register.ContractCr.Field.BudgetMo_Edit', applyTo: '#tfBudgetMo', selector: 'objectcrcontractwin',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.ContractCr.Field.BudgetSubject_Edit', applyTo: '#tfBudgetSubject', selector: 'objectcrcontractwin',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.ContractCr.Field.OwnerMeans_Edit', applyTo: '#tfOwnerMeans', selector: 'objectcrcontractwin',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.ContractCr.Field.FundMeans_Edit', applyTo: '#tfFundMeans', selector: 'objectcrcontractwin',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        { name: 'GkhCr.ObjectCr.Register.ContractCr.Field.File_Edit', applyTo: '#ffFile', selector: 'objectcrcontractwin' },
        { name: 'GkhCr.ObjectCr.Register.ContractCr.Field.Contragent_Edit', applyTo: '#sflContragent', selector: 'objectcrcontractwin' },
        { name: 'GkhCr.ObjectCr.Register.ContractCr.Field.FinanceSource_Edit', applyTo: '#sflFinanceSource', selector: 'objectcrcontractwin' },
        { name: 'GkhCr.ObjectCr.Register.ContractCr.Field.Description_Edit', applyTo: '#taDescription', selector: 'objectcrcontractwin' },

        { name: 'GkhCr.ObjectCr.Register.ContractCr.Field.DateStartWork_Edit', applyTo: '#sfDateStartWork', selector: 'objectcrcontractwin' },
        { name: 'GkhCr.ObjectCr.Register.ContractCr.Field.DateEndWork_Edit', applyTo: '#sfDateEndWork', selector: 'objectcrcontractwin' },

        {
            name: 'GkhCr.ObjectCr.Register.ContractCr.Field.Customer_View', applyTo: 'b4selectfield[name=Customer]', selector: 'objectcrcontractwin',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
                component.setDisabled(!allowed);
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.ContractCr.Field.Customer_Edit', applyTo: 'b4selectfield[name=Customer]', selector: 'objectcrcontractwin',
            applyBy: function (component, allowed) {
                component.setDisabled(!allowed);
            }
        },

        {
            name: 'GkhCr.ObjectCr.Register.ContractCr.TypeWork.View', applyTo: 'contrcrtypewrkgrid', selector: 'objectcrcontractwin',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        },
        { name: 'GkhCr.ObjectCr.Register.ContractCr.TypeWork.Create', applyTo: 'b4addbutton', selector: 'contrcrtypewrkgrid' },
        {
            name: 'GkhCr.ObjectCr.Register.ContractCr.TypeWork.Delete', applyTo: 'b4deletecolumn', selector: 'contrcrtypewrkgrid',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        }
    ]
});