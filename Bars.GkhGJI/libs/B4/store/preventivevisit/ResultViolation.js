//стор приложений акта проверки
Ext.define('B4.store.preventivevisit.ResultViolation', {
    extend: 'B4.base.Store',
    requires: ['B4.model.preventivevisit.ResultViolation'],
    autoLoad: false,
    storeId: 'preventivevisitResultViolationStore',
    model: 'B4.model.preventivevisit.ResultViolation'
});