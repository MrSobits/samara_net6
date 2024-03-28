Ext.define('B4.store.specaccowner.SpecialAccountOwner', {
    extend: 'B4.base.Store',
    requires: ['B4.model.specaccowner.SpecialAccountOwner'],
    autoLoad: false,
    storeId: 'SpecialAccountOwnerStore',
    model: 'B4.model.specaccowner.SpecialAccountOwner'
});