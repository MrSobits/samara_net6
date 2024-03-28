Ext.define('B4.model.smev.SMEVDISKVLIC', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVDISKVLIC'
    },
    fields: [
        { name: 'ReqId'},
        { name: 'Inspector'},
        { name: 'RequestState' },
        { name: 'RequestId' },
        { name: 'Answer' },
        { name: 'BirthDate' },
        { name: 'BirthPlace' },
        { name: 'CalcDate' },
        { name: 'FamilyName' },
        { name: 'FirstName'},
        { name: 'Patronymic' },
        { name: 'FormDate' },
        { name: 'EndDisqDate' },
        { name: 'RegNumber' },
        { name: 'DisqDays' },
        { name: 'DisqYears' },
        { name: 'DisqMonths' },
        { name: 'Article' },
        { name: 'LawDate' },
        { name: 'LawName' },
        { name: 'CaseNumber' },
        { name: 'MessageId' },
        { name: 'FIO' }
    ]
});