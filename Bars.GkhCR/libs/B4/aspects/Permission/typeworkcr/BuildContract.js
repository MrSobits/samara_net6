Ext.define('B4.aspects.permission.typeworkcr.BuildContract', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.buildcontracttypeworkcrperm',

    permissions: [
        { name: 'GkhCr.TypeWorkCr.Register.BuildContract.Edit', applyTo: 'b4savebutton', selector: 'workscrbuildcontractwin' },

        { name: 'GkhCr.TypeWorkCr.Register.BuildContract.Field.DocumentName_Edit', applyTo: '#tfDocumentName', selector: 'workscrbuildcontractwin' },
        { name: 'GkhCr.TypeWorkCr.Register.BuildContract.Field.DocumentNum_Edit', applyTo: '#tfDocumentNum', selector: 'workscrbuildcontractwin' },
        { name: 'GkhCr.TypeWorkCr.Register.BuildContract.Field.DocumentDateFrom_Edit', applyTo: '#tfDocumentDateFrom', selector: 'workscrbuildcontractwin' },
        { name: 'GkhCr.TypeWorkCr.Register.BuildContract.Field.DocumentFile_Edit', applyTo: '#ffDocumentFile', selector: 'workscrbuildcontractwin' },
        { name: 'GkhCr.TypeWorkCr.Register.BuildContract.Field.Builder_Edit', applyTo: '#sfBuilder', selector: 'workscrbuildcontractwin' },
        { name: 'GkhCr.TypeWorkCr.Register.BuildContract.Field.TypeContractBuild_Edit', applyTo: '#cbbxTypeContractBuild', selector: 'workscrbuildcontractwin' },
        { name: 'GkhCr.TypeWorkCr.Register.BuildContract.Field.Sum_Edit', applyTo: '#dcfSum', selector: 'workscrbuildcontractwin' },
        { name: 'GkhCr.TypeWorkCr.Register.BuildContract.Field.Inspector_Edit', applyTo: '#sfInspector', selector: 'workscrbuildcontractwin' },
        { name: 'GkhCr.TypeWorkCr.Register.BuildContract.Field.DateStartWork_Edit', applyTo: '#dfDateStartWork', selector: 'workscrbuildcontractwin' },
        { name: 'GkhCr.TypeWorkCr.Register.BuildContract.Field.DateAcceptOnReg_Edit', applyTo: '#dfDateAcceptOnReg', selector: 'workscrbuildcontractwin' },
        { name: 'GkhCr.TypeWorkCr.Register.BuildContract.Field.DateCancelReg_Edit', applyTo: '#dfDateCancelReg', selector: 'workscrbuildcontractwin' },
        { name: 'GkhCr.TypeWorkCr.Register.BuildContract.Field.DateEndWork_Edit', applyTo: '#dfDateEndWork', selector: 'workscrbuildcontractwin' },
        { name: 'GkhCr.TypeWorkCr.Register.BuildContract.Field.DateInGjiRegister_Edit', applyTo: '#dfDateInGjiRegister', selector: 'workscrbuildcontractwin' },
        { name: 'GkhCr.TypeWorkCr.Register.BuildContract.Field.Description_Edit', applyTo: '#taDescription', selector: 'workscrbuildcontractwin' },
        { name: 'GkhCr.TypeWorkCr.Register.BuildContract.Field.TabResultQual_Edit', applyTo: '#tabResultQual', selector: 'workscrbuildcontractwin' },
        { name: 'GkhCr.TypeWorkCr.Register.BuildContract.Field.ProtocolName_Edit', applyTo: '#tfProtocolName', selector: 'workscrbuildcontractwin' },
        { name: 'GkhCr.TypeWorkCr.Register.BuildContract.Field.ProtocolNum_Edit', applyTo: '#tfProtocolNum', selector: 'workscrbuildcontractwin' },
        { name: 'GkhCr.TypeWorkCr.Register.BuildContract.Field.ProtocolDateFrom_Edit', applyTo: '#tfProtocolDateFrom', selector: 'workscrbuildcontractwin' },
        { name: 'GkhCr.TypeWorkCr.Register.BuildContract.Field.ProtocolFile_Edit', applyTo: '#tfProtocolFile', selector: 'workscrbuildcontractwin' },
        {
            name: 'GkhCr.TypeWorkCr.Register.BuildContract.Field.BudgetMo_Edit', applyTo: '#tfBudgetMo', selector: 'workscrbuildcontractwin',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.TypeWorkCr.Register.BuildContract.Field.BudgetSubject_Edit', applyTo: '#tfBudgetSubject', selector: 'workscrbuildcontractwin',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.TypeWorkCr.Register.BuildContract.Field.OwnerMeans_Edit', applyTo: '#tfOwnerMeans', selector: 'workscrbuildcontractwin',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.TypeWorkCr.Register.BuildContract.Field.FundMeans_Edit', applyTo: '#tfFundMeans', selector: 'workscrbuildcontractwin',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.TypeWorkCr.Register.BuildContract.TypeWork.View', applyTo: 'buildcontrtypewrkgrid', selector: 'workscrbuildcontractwin',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'GkhCr.TypeWorkCr.Register.BuildContract.TypeWork.Create', applyTo: 'b4addbutton', selector: 'buildcontrtypewrkgrid' },
        {
            name: 'GkhCr.TypeWorkCr.Register.BuildContract.TypeWork.Delete', applyTo: 'b4deletecolumn', selector: 'buildcontrtypewrkgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});