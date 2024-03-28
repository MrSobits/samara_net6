Ext.define('B4.model.sendingdataresult.SendingDataResult', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SendingDataResult',
        readAction: 'Get'
    },
    fields: [
        { name: 'ContragentName'},
        { name: 'FormatFileLoaded'},
        { name: 'MethodsInfo'},
        { name: 'UrlRegistry'},
        { name: 'ContragentId'}
    ]
});