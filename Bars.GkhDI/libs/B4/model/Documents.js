Ext.define('B4.model.Documents', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Documents'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DisclosureInfo', defaultValue: null },
        { name: 'FileProjectContract', defaultValue: null },
        { name: 'NotAvailable', defaultValue: false },
        { name: 'DescriptionProjectContract' },
        { name: 'DescriptionCommunalCost' },
        { name: 'DescriptionCommunalTariff' },
        { name: 'FileCommunalService', defaultValue: null },
        { name: 'FileServiceApartment', defaultValue: null }
    ]
});