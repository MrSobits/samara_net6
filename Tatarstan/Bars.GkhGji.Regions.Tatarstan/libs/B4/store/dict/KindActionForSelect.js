Ext.define('B4.store.dict.KindActionForSelect', {
    extend: 'Ext.data.Store',
    requires: ['B4.model.dict.KindAction'],
    model: 'B4.model.dict.KindAction',
    data: B4.enums.KindAction.getItemsMeta()
});