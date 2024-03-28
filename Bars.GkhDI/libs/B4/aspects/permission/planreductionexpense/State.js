Ext.define('B4.aspects.permission.planreductionexpense.State', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.planreductionexpensestateperm',

    permissions: [
        // План мер
        { name: 'GkhDi.DisinfoRealObj.PlanReductionExpense.Add', applyTo: 'b4addbutton', selector: '#planReductionExpenseGrid' },
        {
            name: 'GkhDi.DisinfoRealObj.PlanReductionExpense.Delete', applyTo: 'b4deletecolumn', selector: '#planReductionExpenseGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        
        // Меры
        { name: 'GkhDi.DisinfoRealObj.PlanReductionExpense.Details.Add', applyTo: 'b4addbutton', selector: '#planReductionExpenseWorksGrid' },
        {
            name: 'GkhDi.DisinfoRealObj.PlanReductionExpense.Details.Delete', applyTo: 'b4deletecolumn', selector: '#planReductionExpenseWorksGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'GkhDi.DisinfoRealObj.PlanReductionExpense.Details.Edit', applyTo: '#planReductionExpenseWorksSaveButton', selector: '#planReductionExpenseWorksGrid' }
    ]
});