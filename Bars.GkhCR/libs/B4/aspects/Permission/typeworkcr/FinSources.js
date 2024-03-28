Ext.define('B4.aspects.permission.typeworkcr.FinSources', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.finsourcestypeworkcrperm',

    permissions: [
               { name: 'GkhCr.TypeWorkCr.Register.FinanceSourceRes.Create', applyTo: 'b4addbutton', selector: 'financesourceresgrid' },
               { name: 'GkhCr.TypeWorkCr.Register.FinanceSourceRes.Edit', applyTo: 'b4savebutton', selector: 'financesourcereseditwin' },
               {
                   name: 'GkhCr.TypeWorkCr.Register.FinanceSourceRes.Delete', applyTo: 'b4deletecolumn', selector: 'financesourceresgrid',
                   applyBy: function (component, allowed) {
                       if (allowed) component.show();
                       else component.hide();
                   }
               },
               {
                   name: 'GkhCr.TypeWorkCr.Register.FinanceSourceRes.Column.BudgetMuIncome', applyTo: '[dataIndex=BudgetMuIncome]', selector: 'financesourceresgrid',
                   applyBy: function (component, allowed) { if (allowed) { component.show(); } else { component.hide(); } }
               },
               {
                   name: 'GkhCr.TypeWorkCr.Register.FinanceSourceRes.Column.BudgetMuPercent', applyTo: '[dataIndex=BudgetMuPercent]', selector: 'financesourceresgrid',
                   applyBy: function (component, allowed) { if (allowed) { component.show(); } else { component.hide(); } }
               },
               {
                   name: 'GkhCr.TypeWorkCr.Register.FinanceSourceRes.Column.BudgetSubjectIncome', applyTo: '[dataIndex=BudgetSubjectIncome]', selector: 'financesourceresgrid',
                   applyBy: function (component, allowed) { if (allowed) { component.show(); } else { component.hide(); } }
               },
               {
                   name: 'GkhCr.TypeWorkCr.Register.FinanceSourceRes.Column.BudgetSubjectPercent', applyTo: '[dataIndex=BudgetSubjectPercent]', selector: 'financesourceresgrid',
                   applyBy: function (component, allowed) { if (allowed) { component.show(); } else { component.hide(); } }
               },
               {
                   name: 'GkhCr.TypeWorkCr.Register.FinanceSourceRes.Column.FundResourceIncome', applyTo: '[dataIndex=FundResourceIncome]', selector: 'financesourceresgrid',
                   applyBy: function (component, allowed) { if (allowed) { component.show(); } else { component.hide(); } }
               },
               {
                   name: 'GkhCr.TypeWorkCr.Register.FinanceSourceRes.Column.FundResourcePercent', applyTo: '[dataIndex=FundResourcePercent]', selector: 'financesourceresgrid',
                   applyBy: function (component, allowed) { if (allowed) { component.show(); } else { component.hide(); } }
               }
    ]
});