Ext.define('B4.aspects.permission.BankStatement', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.bankstatementperm',

    permissions: [
        { name: 'GkhCr.BankStatement.Create', applyTo: 'b4addbutton', selector: '#bankStatementGrid' },
        { name: 'GkhCr.BankStatement.Edit', applyTo: 'b4savebutton', selector: '#bankStatementEditPanel' },
        { name: 'GkhCr.BankStatement.Delete', applyTo: 'b4deletecolumn', selector: '#bankStatementGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }

    ]
});