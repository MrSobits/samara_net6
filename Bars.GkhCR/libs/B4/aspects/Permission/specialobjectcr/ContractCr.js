Ext.define('B4.aspects.permission.specialobjectcr.ContractCr', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.contractspecialobjectcrperm',

    permissions: [
        //общие
        { name: 'GkhCr.SpecialObjectCr.Register.ContractCr.Edit', applyTo: 'b4savebutton', selector: 'specialobjectcrcontractwin' },
        
        //поля панели редактирования
        { name: 'GkhCr.SpecialObjectCr.Register.ContractCr.Field.DocumentName_Edit', applyTo: 'textfield[name=DocumentName]', selector: 'specialobjectcrcontractwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.ContractCr.Field.DocumentNum_Edit', applyTo: 'textfield[name=DocumentNum]', selector: 'specialobjectcrcontractwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.ContractCr.Field.DateFrom_Edit', applyTo: 'datefield[name=DateFrom]', selector: 'specialobjectcrcontractwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.ContractCr.Field.TypeContractObject_Edit', applyTo: 'b4combobox[name=TypeContractObject]', selector: 'specialobjectcrcontractwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.ContractCr.Field.SumContract_Edit', applyTo: 'gkhdecimalfield[name=SumContract]', selector: 'specialobjectcrcontractwin' },
        {
            name: 'GkhCr.SpecialObjectCr.Register.ContractCr.Field.BudgetMo_Edit', applyTo: 'textfield[name=BudgetMo]', selector: 'specialobjectcrcontractwin',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.ContractCr.Field.BudgetSubject_Edit', applyTo: 'textfield[name=BudgetSubject]', selector: 'specialobjectcrcontractwin',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.ContractCr.Field.OwnerMeans_Edit', applyTo: 'textfield[name=OwnerMeans]', selector: 'specialobjectcrcontractwin',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.ContractCr.Field.FundMeans_Edit', applyTo: 'textfield[name=FundMeans]', selector: 'specialobjectcrcontractwin',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        { name: 'GkhCr.SpecialObjectCr.Register.ContractCr.Field.File_Edit', applyTo: 'b4filefield[name=File]', selector: 'specialobjectcrcontractwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.ContractCr.Field.Contragent_Edit', applyTo: 'b4selectfield[name=Contragent]', selector: 'specialobjectcrcontractwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.ContractCr.Field.FinanceSource_Edit', applyTo: 'b4selectfield[name=FinanceSource]', selector: 'specialobjectcrcontractwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.ContractCr.Field.Description_Edit', applyTo: 'textarea[name=Description]', selector: 'specialobjectcrcontractwin' },

        { name: 'GkhCr.SpecialObjectCr.Register.ContractCr.Field.DateStartWork_Edit', applyTo: 'datefield[name=DateStartWork]', selector: 'specialobjectcrcontractwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.ContractCr.Field.DateEndWork_Edit', applyTo: 'datefield[name=DateEndWork]', selector: 'specialobjectcrcontractwin' },

        {
            name: 'GkhCr.SpecialObjectCr.Register.ContractCr.Field.Customer_View', applyTo: 'b4selectfield[name=Customer]', selector: 'specialobjectcrcontractwin',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
                component.setDisabled(!allowed);
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.ContractCr.Field.Customer_Edit', applyTo: 'b4selectfield[name=Customer]', selector: 'specialobjectcrcontractwin',
            applyBy: function (component, allowed) {
                component.setDisabled(!allowed);
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.ContractCr.TypeWork.View', applyTo: 'contracttypeworkspecialcrgrid', selector: 'specialobjectcrcontractwin',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        },
        { name: 'GkhCr.SpecialObjectCr.Register.ContractCr.TypeWork.Create', applyTo: 'b4addbutton', selector: 'contracttypeworkspecialcrgrid' },
        {
            name: 'GkhCr.SpecialObjectCr.Register.ContractCr.TypeWork.Delete', applyTo: 'b4deletecolumn', selector: 'contracttypeworkspecialcrgrid',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        }
    ]
});