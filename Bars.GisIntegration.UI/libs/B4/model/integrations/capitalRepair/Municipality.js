Ext.define('B4.model.integrations.capitalrepair.Municipality', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CapitalRepair',
        listAction: 'GetMunicipalities'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' }
    ]
});