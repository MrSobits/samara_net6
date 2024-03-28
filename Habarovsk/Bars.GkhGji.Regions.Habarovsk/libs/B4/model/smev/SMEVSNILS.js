Ext.define('B4.model.smev.SMEVSNILS', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVSNILS'
    },
    fields: [
        { name: 'ReqId'},
        { name: 'RequestDate'},
        { name: 'Inspector'},
        { name: 'RequestState' },
        { name: 'Fio' },
        { name: 'BirthDate' },
        { name: 'SNILS' },
        { name: 'CalcDate' },
        { name: 'Surname' },
        { name: 'Name'},
        { name: 'PatronymicName' },
        { name: 'SMEVGender', defaultValue: 10 },
        { name: 'SnilsPlaceType', defaultValue: 10},
        { name: 'Settlement'},
        { name: 'District' },
        { name: 'Region' },
        { name: 'Country'},
        { name: 'Number' },
        { name: 'IssueDate' },
        { name: 'Issuer' },
        { name: 'Answer' },
        { name: 'MessageId' },
        { name: 'Series' }
    ]
});