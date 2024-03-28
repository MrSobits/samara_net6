﻿Ext.define('B4.model.disposal.DisposalControlMeasures', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DisposalControlMeasures'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DateStart'},
        { name: 'DateEnd' },
        { name: 'Description' },
        { name: 'Disposal', defaultValue: null }, 
        { name: 'ControlMeasuresName' }
    ]
});