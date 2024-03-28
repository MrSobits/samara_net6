Ext.define('B4.model.FinActivity', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FinActivity'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DisclosureInfo', defaultValue: null },
        { name: 'TaxSystem', defaultValue: null },
        { name: 'ValueBlankActive', defaultValue: null },
        { name: 'Description' },
        { name: 'ClaimDamage', defaultValue: null },
        { name: 'FailureService', defaultValue: null },
        { name: 'NonDeliveryService', defaultValue: null }
    ]
});