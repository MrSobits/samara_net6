Ext.define('B4.model.integrationtor.Subject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TorIntegration',
        listAction: 'GetSubjects'
    },
    fields: [
        { name: 'TorId' },
        { name: 'Name' },
        { name: 'JuridicalAddress' },
        { name: 'Inn' },
        { name: 'Kpp' },
        { name: 'Ogrn' }
    ]
});