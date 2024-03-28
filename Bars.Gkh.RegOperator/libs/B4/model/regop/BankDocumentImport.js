Ext.define('B4.model.regop.BankDocumentImport', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'ImportDate' },
        { name: 'DocumentDate' },
        { name: 'DocumentNumber' },
        { name: 'ImportedSum' },
        { name: 'LogImport' },
        { name: 'PaymentAgentCode' },
        { name: 'PaymentAgentName' },
        { name: 'PACount' },
        { name: 'PersonalAccountDeterminationState' },
        { name: 'PaymentConfirmationState' },
        { name: 'FileName' },
        { name: 'File' },
        { name: 'BankStatement' },
        { name: 'CheckState' },
        { name: 'AcceptedSum' },
        { name: 'DistributedSum' },
        { name: 'AcceptDate' },
        { name: 'ImportType' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'BankDocumentImport',
        timeout: 5 * 60 * 1000 // 5 минут
    }
});