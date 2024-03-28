Ext.define('B4.aspects.permission.objectcr.ContractCrView', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.contractcrviewperm',

    permissions: [
        //права на просмотр
        {
            name: 'GkhCr.ObjectCr.Register.ContractCr.Field.FinanceSource_View', applyTo: '#sflFinanceSource', selector: 'objectcrcontractwin',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.ContractCr.Field.Description_View', applyTo: '#taDescription', selector: 'objectcrcontractwin',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});