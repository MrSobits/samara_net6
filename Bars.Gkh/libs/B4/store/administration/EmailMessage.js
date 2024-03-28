Ext.define('B4.store.administration.EmailMessage', {
    extend: 'B4.base.Store',
    requires: ['B4.model.administration.EmailMessage'],
    autoLoad: false,
    storeId: 'emailMessageStore',
    model: 'B4.model.administration.EmailMessage'
});