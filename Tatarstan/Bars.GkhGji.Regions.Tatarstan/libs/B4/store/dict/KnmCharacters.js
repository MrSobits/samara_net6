Ext.define('B4.store.dict.KnmCharacters', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.KnmCharacter'],
    autoLoad: false,
    storeId: 'knmcharacters',
    model: 'B4.model.dict.KnmCharacter'
});