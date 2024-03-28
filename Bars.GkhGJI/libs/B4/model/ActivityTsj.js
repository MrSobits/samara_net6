Ext.define('B4.model.ActivityTsj', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActivityTsj'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ManagingOrganization', defaultValue: null },
        { name: 'ManOrgName' },
        { name: 'MunicipalityName' },
        { name: 'JuridicalAddress' },
        { name: 'MailingAddress' },
        { name: 'Inn' },
        { name: 'Kpp' },
        { name: 'StateMemberTsj', defaultValue: null },
        { name: 'HasStatute', defaultValue: false }        
    ]
});