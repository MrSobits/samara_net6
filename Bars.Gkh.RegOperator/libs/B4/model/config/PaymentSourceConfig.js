Ext.define('B4.model.config.PaymentSourceConfig', {
    extend: 'B4.base.Model',
    idProperty: 'Path',
    fields: [
        { name: 'Id', useNull: true },
        { name: 'PaymentDocumentType' },
        { name: 'Path' },
        { name: 'Enabled' }
    ]
});