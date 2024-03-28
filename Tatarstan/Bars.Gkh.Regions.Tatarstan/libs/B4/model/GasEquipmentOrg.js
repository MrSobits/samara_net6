Ext.define('B4.model.GasEquipmentOrg', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GasEquipmentOrg'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Contragent', defaultValue: null },
        { name: 'ContragentId' },
        { name: 'Contact', defaultValue: null },
        { name: 'Municipality', defaultValue: null },
        { name: 'Inn' },
        { name: 'Kpp' },
        { name: 'Ogrn' },
        { name: 'JuridicalAddress' },
        { name: 'Phone' },
        { name: 'Name' },
        { name: 'DateRegistration' },
        { name: 'DateTermination' }
    ]
});