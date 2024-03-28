Ext.define('B4.model.administration.EmailMessage', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EmailMessage'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'EmailMessageType' },
        { name: 'RecipientName' },
        { name: 'EmailAddress' },
        { name: 'AdditionalInfo' },
        { name: 'SendingTime' },
        { name: 'SendingStatus' },
        { name: 'LogFileId' }
    ]
});