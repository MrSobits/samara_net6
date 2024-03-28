Ext.define('B4.model.contragent.Contact', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'contragentcontact'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Contragent', defaultValue: null },
        { name: 'ContragentMunicipality', defaultValue: null },
        { name: 'Email' },
        { name: 'Phone' },
        { name: 'Annotation' },
        { name: 'Gender' },
        { name: 'BirthDate' },
        { name: 'DateStartWork' },
        { name: 'DateEndWork' },
        { name: 'Name' },
        { name: 'Surname' },
        { name: 'Patronymic' },
        { name: 'FullName' },
        { name: 'Position', defaultValue: null },
        { name: 'OrderDate' },
        { name: 'OrderName' },
        { name: 'OrderNum' },
        { name: 'Snils' },
        { name: 'NameGenitive' },
        { name: 'SurnameGenitive' },
        { name: 'PatronymicGenitive' },
        
        { name: 'NameDative' },
        { name: 'SurnameDative' },
        { name: 'PatronymicDative' },

        { name: 'FLDocIssuedDate' },
        { name: 'FLDocSeries' },
        { name: 'FLDocNumber' },
        { name: 'FLDocIssuedBy' },
        
        { name: 'NameAccusative' },
        { name: 'SurnameAccusative' },
        { name: 'PatronymicAccusative' },
        
        { name: 'NameAblative' },
        { name: 'SurnameAblative' },
        { name: 'PatronymicAblative' },
        
        { name: 'NamePrepositional' },
        { name: 'SurnamePrepositional' },
        { name: 'PatronymicPrepositional' }
    ]
});