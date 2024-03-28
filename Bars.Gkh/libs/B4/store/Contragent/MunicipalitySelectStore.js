Ext.define('B4.store.contragent.MunicipalitySelectStore', {    
    extend: 'B4.base.Store',
    
    autoLoad: false,
    fields: ['Id', 'Name'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'ContragentMunicipality',
        listAction: 'ListAvailableMunicipality'
    }
});