Ext.define('B4.store.administration.instruction.Instructions', {
    extend: 'B4.base.Store',
    requires: ['B4.model.administration.instruction.Instructions'],
    autoLoad: false,
    storeId: 'instructionsStore',
    model: 'B4.model.administration.instruction.Instructions'
});