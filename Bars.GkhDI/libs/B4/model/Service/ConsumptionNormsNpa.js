Ext.define('B4.model.service.ConsumptionNormsNpa', {
    extend: 'B4.base.Model',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ConsumptionNormsNpa'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'BaseService', defaultValue: null },
        { name: 'NpaDate', defaultValue: null, type: 'date' },
        { name: 'NpaNumber', defaultValue: null },
        { name: 'NpaAcceptor', defaultValue: null }
    ]
});