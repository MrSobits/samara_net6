Ext.define('B4.model.email.EmailGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EmailGji'
    },
    fields: [
        { name: 'Id' },
        { name: 'From' },
        { name: 'SenderInfo' },
        { name: 'Theme' },
        { name: 'EmailDate' },
        { name: 'GjiNumber' },
        { name: 'SystemNumber' },
        { name: 'LivAddress' },
        { name: 'DeclineReason' },
        { name: 'EmailType', defaultValue: 30 },
        { name: 'EmailGjiSource', defaultValue: 0 },
        { name: 'EmailDenailReason', defaultValue: 0 },
        { name: 'Registred', defaultValue: false },
        { name: 'Description' },
        { name: 'EmailPdf' },
    ]
});