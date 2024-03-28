Ext.define('B4.model.integrations.orgRegistry.Contragent', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'OrgRegistry',
        listAction: 'GetContragentList'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Ogrn' },
        { name: 'JuridicalAddress' }
    ]
});