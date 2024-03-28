Ext.define('B4.model.suggestion.ApplicantNotification', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ApplicantNotification'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Code' },
        { name: 'EmailSubject' },
        { name: 'EmailTemplate' },
        { name: 'State' }
    ]
});