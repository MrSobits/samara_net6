Ext.define('B4.model.appealcits.RequestAnswer', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppealCitsRequestAnswer'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AppealCitsRequest', defaultValue: null },    
        { name: 'DocumentName' },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'Description' },
        { name: 'Description' },
        { name: 'File', defaultValue: null },
        { name: 'Signature', defaultValue: null },
        { name: 'Certificate', defaultValue: null },
        { name: 'SignedFile', defaultValue: null }
    ]
});