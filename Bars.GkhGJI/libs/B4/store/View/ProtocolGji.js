Ext.define('B4.store.view.ProtocolGji', {
    requires: ['B4.model.ProtocolGji'],
    extend: 'B4.base.Store',
    autoLoad: false,
    storeId: 'viewProtocolStore',
    model: 'B4.model.ProtocolGji',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Protocol',
        listAction: 'ListView',
        timeout: 300000 //Значения по умолчанию (30000) не хватает
    }
});