Ext.define('B4.store.import.chesimport.payments.Assigned', {
    extend: 'B4.base.Store',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'ChesImport',
        listAction: 'PaymentsList',
        timeout: 1000 * 60 * 5
    },
    fields: [
        { name: 'RowNumber' },
        { name: 'LsNum' },
        { name: 'PaymentDate' },
        { name: 'TariffDecisionPayment' },
        { name: 'TariffPayment' },
        { name: 'PenaltyPayment' },
        { name: 'PaymentType' },
        { name: 'RegistryNum' },
        { name: 'RegistryDate' },
        { name: 'Id' },
        { name: 'PaymentDay' },
        { name: 'Version' },
        { name: 'IsValid' },
        { name: 'IsImported' },
        { name: 'ImportPaymentsState' },
        { name: 'ReportDate' }
    ],
    listeners: {
        beforeload: function(store, operation) {
            operation.params.isValid = true;
        }
    }
});