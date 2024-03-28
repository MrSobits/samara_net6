Ext.define('B4.model.regop.ImportedPayment', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'Account' },
        { name: 'PaymentType' },
        { name: 'Sum' },
        { name: 'PaymentDate' },
        { name: 'PaymentState' },
        { name: 'PaymentNumberUs' },
        { name: 'ReceiverNumber' },
        { name: 'FactReceiverNumber' },
        { name: 'ExternalAccountNumber' },
        { name: 'IsDeterminateManually' },
        { name: 'PersonalAccount' },
        { name: 'PersAccNumExternalSystems' },
        { name: 'AddressByImport' },
        { name: 'OwnerByImport' },
        { name: 'Address' },
        { name: 'Owner' },
        { name: 'ReceiverNumberFact' },
        { name: 'PaymentConfirmationState' },
        { name: 'PersonalAccountDeterminationState' },
        { name: 'BankDocumentImportId' },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'PaymentAgentName' },
        { name: 'PersonalAccountState'},
        { name: 'AcceptDate' },
        { name: 'PersonalAccountNotDeterminationStateReason' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'ImportedPayment'
    }
});