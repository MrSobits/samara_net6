Ext.define('B4.store.specialobjectcr.ObjectCrBuilder', {
    extend: 'B4.base.Store',
    requires: ['B4.model.Builder'],
    autoLoad: false,
    model: 'B4.model.Builder',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SpecialObjectCr',
        listAction: 'GetBuilders'
    }
});