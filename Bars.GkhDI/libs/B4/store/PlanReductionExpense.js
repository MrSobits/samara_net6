Ext.define('B4.store.PlanReductionExpense', {
    extend: 'B4.base.Store',
    requires: ['B4.model.PlanReductionExpense'],
    autoLoad: false,
    storeId: 'planReductionExpenseStore',
    model: 'B4.model.PlanReductionExpense'
});