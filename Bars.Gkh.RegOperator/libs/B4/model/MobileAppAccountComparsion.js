Ext.define('B4.model.MobileAppAccountComparsion', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MobileAppAccountComparsion'
    },
    fields: [
        { name: 'Id', useNull: true },   
        { name: 'DecisionType', defaultValue: 1 },           
        { name: 'IsViewed' },
        { name: 'IsWorkOut' },
        { name: 'MobileAccountNumber' },
        { name: 'MobileAccountOwnerFIO' },
        { name: 'ExternalAccountNumber' },
        { name: 'FkrUserFio' },
        { name: 'PersonalAccountOwnerFIO' },
        { name: 'PersonalAccountNumber' },
        { name: 'OperatinDate' },     
    ]
});