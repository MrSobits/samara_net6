Ext.define('B4.store.eds.ListDocumentsForPetition', {
    extend: 'B4.base.Store',
    requires: ['B4.model.ProtocolGji'],
    autoLoad: false,
    model: 'B4.model.DocumentGji',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EDSScript',
        listAction: 'GetListGjiDoc'
    }
});