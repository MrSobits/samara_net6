Ext.define('B4.model.passport.Oki', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'OkiPassport'
    },
    fields: [
        { name: 'Id', defaultValue: 0 },
        { name: 'ReportYear', defaultValue: 0 },
        { name: 'ReportMonth', defaultValue: 0 },
        { name: 'Municipality', defaultValue: 0 },
        { name: 'State', defaultValue: 0 },
        { name: 'Xml', defaultValue: 0 },
        { name: 'Signature', defaultValue: 0 },
        { name: 'Pdf', defaultValue: 0 },
        { name: 'Percent', defaultValue: 0 },
        { name: 'Count', defaultValue: 0 },
        { name: 'NumberNotCreated', defaultValue: 0 },
        { name: 'ContragentsNotCreated', defaultValue: "" }
    ]
});