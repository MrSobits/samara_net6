Ext.define('B4.model.emailnewsletter.EmailNewsletter', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EmailNewsletter'
    },
    fields: [
        { name: 'Id' },
        { name: 'SendDate' },
        { name: 'Header' },
        { name: 'Body' },
        { name: 'Destinations' },
        { name: 'Success' },
        { name: 'Attachment' },
        { name: 'Sender' }
    ]
});