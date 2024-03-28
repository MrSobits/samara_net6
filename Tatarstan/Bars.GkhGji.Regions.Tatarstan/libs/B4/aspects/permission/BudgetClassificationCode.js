Ext.define('B4.aspects.permission.BudgetClassificationCode', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.budgetclassificationcodeperm',

    permissions: [
        { name: 'GkhGji.Dict.BudgetClassificationCode.Create', applyTo: 'b4addbutton', selector: 'budgetclassificationcodegrid' },
        {
            name: 'GkhGji.Dict.BudgetClassificationCode.Delete', applyTo: 'b4deletecolumn', selector: 'budgetclassificationcodegrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhGji.Dict.BudgetClassificationCode.Edit', applyTo: 'b4editcolumn', selector: 'budgetclassificationcodegrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});