Ext.define('B4.model.EmailLists', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'EmailLists'
    },
    fields: [
        { name: 'Id' },
        { name: 'AnswerNumber'},
        { name: 'AppealDate'},
        { name: 'AppealNumber'},
        { name: 'FileInfo' },
        { name: 'MailTo' },
        { name: 'SendDate' }, 
        { name: 'GjiNumber' }, 
        { name: 'AnswerDate' }, 
        { name: 'Executor' }, 
        { name: 'Title'}
    ]
});