Ext.define('B4.store.dict.ResettlementProgramForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.ResettlementProgram'],
    autoLoad: false,
    storeId: 'resettlementProgramStoreForSelected',
    model: 'B4.model.dict.ResettlementProgram'
});