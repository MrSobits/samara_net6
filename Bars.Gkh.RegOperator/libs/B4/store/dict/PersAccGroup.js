Ext.define('B4.store.dict.PersAccGroup', {
    extend: 'B4.base.Store',
    autoLoad: false,
    model: 'B4.model.dict.PersAccGroup',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PersAccGroup'
    }
});