Ext.define('B4.model.planreductionexpense.Works', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PlanReductionExpenseWorks'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'PlanReductionExpense', defaultValue: null },
        { name: 'Name' },
        { name: 'DateComplete', defaultValue: null },
        { name: 'PlannedReductionExpense', defaultValue: null },
        { name: 'FactedReductionExpense', defaultValue: null },
        { name: 'ReasonRejection' }
    ]
});