Ext.define('B4.store.realestatetype.MunicipalityForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.Municipality'],
    autoLoad: false,
    model: 'B4.model.dict.Municipality',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealEstateType',
        listAction: 'GetMuList'
    }
});