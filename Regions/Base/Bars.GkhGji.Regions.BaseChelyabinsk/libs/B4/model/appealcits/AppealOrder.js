Ext.define('B4.model.appealcits.AppealOrder', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppealOrder'
    },
    fields: [
        { name: 'AppealCits', defaultValue: null },
        { name: 'Executant', defaultValue: null },
        { name: 'Person' },
        { name: 'PersonPhone' },
        { name: 'DocumentNumber'},
        { name: 'OrderDate' },
        { name: 'AppealDate' },
        { name: 'DateFrom' },
        { name: 'PerformanceDate' },
        { name: 'Correspondent' },
        { name: 'CorrespondentAddress' },
        { name: 'File' },
        { name: 'State', defaultValue: null },
        { name: 'Email' },
        { name: 'Description' },
        { name: 'AppealText' },
        { name: 'Phone' },
        { name: 'ContragentName' },
        { name: 'ContragentINN' },
        { name: 'YesNoNotSet', defaultValue: 30 },
        { name: 'Confirmed', defaultValue: 30 },
        { name: 'ConfirmedGJI', defaultValue: 30 }
    ]
});