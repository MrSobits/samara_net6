Ext.define('B4.model.actcheck.DocRequestAction', {
    extend: 'B4.model.actcheck.Action',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DocRequestAction'
    },
    fields: [
        { name: 'ContrPersType' },
        { name: 'ContrPersContragent' },
        { name: 'DocProvidingPeriod' },
        { name: 'DocProvidingAddress' },
        { name: 'ContrPersEmailAddress' },
        { name: 'PostalOfficeNumber' },
        { name: 'EmailAddress' },
        { name: 'CopyDeterminationDate' },
        { name: 'LetterNumber' }
    ]
});