Ext.define('B4.model.emailnewsletter.EmailNewsletterLog', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EmailNewsletterLog'
    },
    fields: [
        { name: 'Id' },
        { name: 'EmailNewsletter' },
        { name: 'Destination' },
        { name: 'Log' },
        { name: 'Success' }
    ]
});