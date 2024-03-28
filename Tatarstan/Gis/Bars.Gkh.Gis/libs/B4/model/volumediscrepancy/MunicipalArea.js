Ext.define('B4.model.volumediscrepancy.MunicipalArea', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Municipality',
        listAction: 'ListMoArea'
    },
    fields: [
        { name: 'Id', mapping: 'FiasId' },
        { name: 'Name' }
    ]
});