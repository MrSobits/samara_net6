Ext.define('B4.model.email.EmailGjiAttachment', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EmailGjiAttachment'
    },
    fields: [
        { name: 'Id' },
        { name: 'AttachmentFile' },
        { name: 'FileName' }
    ]
});