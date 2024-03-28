Ext.define('B4.store.regoperator.MunicipalityForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.Municipality'],
    autoLoad: false,
    model: 'B4.model.dict.Municipality',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RegOperatorMunicipality',
        listAction: 'ListMuByRegOp'
    }
});