Ext.define('B4.store.dict.WorkPriceByMunicipalityForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.WorkPrice'],
    autoLoad: false,
    model: 'B4.model.dict.WorkPrice',
    proxy: {
        type: 'b4proxy',
        controllerName: 'WorkPrice',
        listAction: 'ListByFromMunicipality'
    }
});