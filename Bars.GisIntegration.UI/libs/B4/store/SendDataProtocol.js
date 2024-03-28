Ext.define('B4.store.SendDataProtocol', {
    extend: 'B4.base.Store',
    requires: ['B4.model.SendDataProtocolRecord'],
    autoLoad: false,
    storeId: 'sendDataProtocolStore',
    model: 'B4.model.SendDataProtocolRecord'
});
