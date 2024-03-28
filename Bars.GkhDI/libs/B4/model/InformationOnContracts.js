Ext.define('B4.model.InformationOnContracts', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'InformationOnContracts'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DisclosureInfo', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },
        { name: 'Name' },
        { name: 'AddressName' },
        { name: 'Number' },
        { name: 'Cost' },
        { name: 'From' },
        { name: 'DateStart' },
        { name: 'DateEnd'},
        { name: 'PartiesContract' },
        { name: 'Comments' }
    ]
});