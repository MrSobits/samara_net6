Ext.define('B4.model.regop.LocationCode', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'FiasLevel1' },
        { name: 'FiasLevel2' },
        { name: 'FiasLevel3' },
        { name: 'AOGuid' },
        { name: 'CodeLevel1' },
        { name: 'CodeLevel2' },
        { name: 'CodeLevel3' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'LocationCode'
    }
});