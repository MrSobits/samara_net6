Ext.define('B4.model.mkdlicrequest.AnswerAttachment', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MKDLicRequestAnswerAttachment'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'MKDLicRequestAnswer', defaultValue: null },
        { name: 'Name' },
        { name: 'Description' },
        { name: 'FileInfo' }
    ]
});