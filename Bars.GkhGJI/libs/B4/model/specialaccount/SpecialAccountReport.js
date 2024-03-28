Ext.define('B4.model.specialaccount.SpecialAccountReport', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SpecialAccountReport'
    },
    fields: [
        { name: 'Id'},
        { name: 'Contragent', defaultValue: null },
        { name: 'ContragentName' },        
        { name: 'CreditOrg', defaultValue: null },
        { name: 'Executor' },
        { name: 'Author' },
        { name: 'AmmountMeasurement' },
        { name: 'MonthEnums' },
        { name: 'YearEnums', defaultValue: 2018 },
        { name: 'Inn' },
        { name: 'Bik' },
        { name: 'Tariff' },
        { name: 'OP' },
        { name: 'Sertificate', defaultValue: 'Не подписан' },
        { name: 'ObjectEditDate' },
        { name: 'DateAccept' },
        { name: 'File' },
        { name: 'Signature' },
        { name: 'Certificate' },
        { name: 'SignedXMLFile'}
    ]
});