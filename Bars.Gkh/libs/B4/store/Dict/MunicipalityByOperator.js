Ext.define('B4.store.dict.MunicipalityByOperator', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.Municipality'],
    autoLoad: false,
    storeId: 'municipalityByOperator',
    model: 'B4.model.dict.Municipality',
    proxy: {
        autoLoad: false,
        type: 'b4proxy',
        controllerName: 'Municipality',
        listAction: 'ListByOperator'
    }
});