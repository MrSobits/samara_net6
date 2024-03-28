Ext.define('B4.aspects.permission.specialobjectcr.ContractCrView', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.contractspecialobjectcrviewperm',

    permissions: [ 
        //права на просмотр
        {
            name: 'GkhCr.ObjectCr.Register.ContractCr.Field.FinanceSource_View', applyTo: 'b4selectfield[name=FinanceSource]', selector: 'specialobjectcrcontractwin',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.ContractCr.Field.Description_View', applyTo: 'textarea[name=Description]', selector: 'specialobjectcrcontractwin',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});