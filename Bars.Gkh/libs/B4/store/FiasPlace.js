Ext.define('B4.store.FiasPlace', {
    extend: 'B4.base.Store',
    model: 'B4.model.Fias',
    requires: ['B4.model.Fias'],
    autoLoad: true,
    proxy: {
        type: 'b4proxy',
        controllerName: 'Fias',
        listAction: 'ListPlaces'
    }
});