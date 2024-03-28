Ext.define('B4.model.integrations.nsi.MunicipalService', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Nsi',
        listAction: 'GetMunicipalServices'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'UnitMeasure' }
    ]
});