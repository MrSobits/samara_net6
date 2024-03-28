Ext.define('B4.store.dict.FiasOktmo', {
    extend: 'B4.base.Store',
    autoLoad: false,
    requires: ['B4.model.dict.FiasOktmo'],
    storeId: 'FiasOktmo',
    model: 'B4.model.dict.FiasOktmo',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FiasOktmo',
        listAction: 'List'
    }
});