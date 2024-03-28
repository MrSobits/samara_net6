Ext.define('B4.model.passport.House', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HousePassport',
        timeout: 120000
    },
    fields: [
        { name: 'Id', defaultValue: 0 },
        { name: 'ReportYear', defaultValue: 0 },
        { name: 'ReportMonth', defaultValue: 0 },
        { name: 'RealityObject', defaultValue: 0 },
        { name: 'HouseType', defaultValue: 10 },
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