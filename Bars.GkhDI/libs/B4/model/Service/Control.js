Ext.define('B4.model.service.Control', {
    extend: 'B4.model.service.Base',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ControlService'
    }
});