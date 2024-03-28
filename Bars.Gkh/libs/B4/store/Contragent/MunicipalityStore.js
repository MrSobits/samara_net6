Ext.define("B4.store.contragent.MunicipalityStore", {    
    extend: 'B4.base.Store',
    
    fields: ['Id', 'Name'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'ContragentMunicipality'
    }
});