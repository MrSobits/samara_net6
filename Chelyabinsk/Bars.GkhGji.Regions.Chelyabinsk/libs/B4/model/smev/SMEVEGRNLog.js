Ext.define('B4.model.smev.SMEVEGRNLog', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVEGRNLog'
    },
    fields: [
        { name: 'Id'},
        { name: 'SMEVEGRN' },
        { name: 'OperationType' },
        { name: 'UserName' },
        { name: 'Login' },
        { name: 'ObjectCreateDate' },
        { name: 'CadastralNUmber' },
        { name: 'ReqNum' },
        { name: 'RealityObject' },
        { name: 'Room' },
        { name: 'FileInfo' }
    ]
});