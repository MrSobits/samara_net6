Ext.define('B4.store.objectcr.TypeWorkCrForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.objectcr.TypeWorkCr'],
    model: 'B4.model.objectcr.TypeWorkCr',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProtocolCrTypeWork',
        listAction: 'ListProtocolCrTypeWork'
    }
});