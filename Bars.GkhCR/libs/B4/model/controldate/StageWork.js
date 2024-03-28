Ext.define('B4.model.controldate.StageWork', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ControlDateStageWork'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ControlDate' },
        { name: 'StageWork' }
    ]
});