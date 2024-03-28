Ext.define('B4.model.utilityclaimwork.ExecutoryProcess', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    fields: [
        { name: 'Id' },
        { name: 'State' },

        { name: 'RealityObject', defaultValue: null },
        { name: 'OwnerType', defaultValue: 20 },
        { name: 'AccountOwner' },

        { name: 'Creditor' },
        { name: 'LegalOwnerRealityObject', defaultValue: null },
        { name: 'Inn' },
        { name: 'Clause' },
        { name: 'Paragraph' },
        { name: 'Subparagraph' },
        { name: 'JurInstitution', defaultValue: null },
        { name: 'RegistrationNumber' },
        { name: 'File', defaultValue: null },
        { name: 'Document' },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'DebtSum' },
        { name: 'PaidSum' },
        { name: 'DateStart' },
        { name: 'IsTerminated', defaultValue: false },
        { name: 'DateEnd' },
        { name: 'TerminationReasonType', defaultValue: 0 },
        { name: 'Notation' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'ExecutoryProcess'
    }
});