Ext.define('B4.model.regoperator.Municipality', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RegOPeratorMunicipality'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RegOperator', defaultValue: null },
        { name: 'Municipality', defaultValue: null }
    ]
});