Ext.define('B4.store.email.ListAttachments', {
    extend: 'B4.base.Store',
    requires: ['B4.model.email.EmailGjiAttachment'],
    autoLoad: false,
    model: 'B4.model.email.EmailGjiAttachment',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EmailGji',
        listAction: 'GetListAttachments'
    }
});