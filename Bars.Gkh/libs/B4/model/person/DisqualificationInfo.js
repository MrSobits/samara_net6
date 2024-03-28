Ext.define('B4.model.person.DisqualificationInfo', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PersonDisqualificationInfo'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Person', defaultValue: null },
        { name: 'TypeDisqualification', defaultValue: 10 },
        { name: 'DisqDate' },
        { name: 'EndDisqDate' },
        { name: 'PetitionDate' },
        { name: 'NameOfCourt' },
        { name: 'PetitionNumber' },
        { name: 'PetitionFile' }
    ]
});