Ext.define('B4.model.passport.OkiProviderPassport', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'OkiProviderPassport',
        listAction: 'ListByPassport'
    },
    fields: [
        { name: 'Id', defaultValue: 0 },
        { name: 'OkiPassport', defaultValue: 0 },
        { name: 'ReportYear', defaultValue: 0 },
        { name: 'ReportMonth', defaultValue: 0 },
        { name: 'Municipality', defaultValue: 0 },
        { name: 'State', defaultValue: 0 },
        { name: 'ContragentType', defaultValue: 0 },
        { name: 'Contragent', defaultValue: 0 },
        { name: 'Xml', defaultValue: 0 },
        { name: 'Signature', defaultValue: 0 },
        { name: 'Pdf', defaultValue: 0 },
        { name: 'Percent', defaultValue: 0 },
        { name: 'ObjectCreateDate', defaultValue: 0 },
        { name: 'SignDate', defaultValue: 0 }
    ]
});