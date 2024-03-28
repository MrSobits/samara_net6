Ext.define('B4.model.passport.HouseProviderPassport', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HouseProviderPassport',
        listAction: 'ListByPassport'
    },
    fields: [
        { name: 'Id', defaultValue: 0 },
        { name: 'HousePassport', defaultValue: 0 },
        { name: 'ReportYear', defaultValue: 0 },
        { name: 'ReportMonth', defaultValue: 0 },
        { name: 'RealityObject', defaultValue: 0 },
        { name: 'State', defaultValue: 0 },
        { name: 'ContragentType', defaultValue: 0 },
        { name: 'Contragent', defaultValue: 0 },
        { name: 'Xml', defaultValue: 0 },
        { name: 'Signature', defaultValue: 0 },
        { name: 'SignDate', defaultValue: 0 },
        { name: 'Pdf', defaultValue: 0 },
        { name: 'Percent', defaultValue: 0 },
        { name: 'ObjectCreateDate', defaultValue: 0 }
    ]
});