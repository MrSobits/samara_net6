Ext.define('B4.model.TechnicalCustomer', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TechnicalCustomer'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Contragent' },
        { name: 'Period' },
        { name: 'OrganizationForm' },
        { name: 'File', defaultValue: null }
    ]
});