Ext.define('B4.model.contragentclw.Municipality', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ContragentClwMunicipality'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ContragentClw', defaultValue: null },
        { name: 'Municipality', defaultValue: null }
    ]
});