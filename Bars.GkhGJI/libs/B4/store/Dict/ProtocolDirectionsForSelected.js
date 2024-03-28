Ext.define('B4.store.dict.ProtocolDirectionsForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.ProtocolDirectionsForSelected'],
    autoLoad: false,
    storeId: 'protocolDirectionsForSelected',
    model: 'B4.model.dict.ProtocolDirectionsForSelected',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Protocol',
        listAction: 'ListDirections'
    }
});
