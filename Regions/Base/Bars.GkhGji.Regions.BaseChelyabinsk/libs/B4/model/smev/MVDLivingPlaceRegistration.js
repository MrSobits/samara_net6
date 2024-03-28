Ext.define('B4.model.smev.MVDLivingPlaceRegistration', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MVDLivingPlaceRegistration'
    },
    fields: [
        { name: 'ReqId'},
        { name: 'RequestDate' },
        { name: 'Inspector'},
        { name: 'RequestState' },
        { name: 'Answer' },
        { name: 'MessageId' },
        { name: 'FileInfo' },
        { name: 'CalcDate' },
        { name: 'BirthDate' },
        { name: 'Surname' },
        { name: 'Name' },
        { name: 'PatronymicName' },
        { name: 'BirthPlace' },
        { name: 'PassportSeries' },
        { name: 'PassportNumber' },
        { name: 'IssueDate' },
        { name: 'AnswerInfo' }
    ]
});