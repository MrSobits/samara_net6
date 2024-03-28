Ext.define('B4.controller.longtermprobject.ExecutionLongTermProgram', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.controller.objectcr.Navi'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        context: 'B4.mixins.Context'
    },
    
    models: [
        'longtermprobject.ExecutionLongTermProgram',
        'ObjectCr'
    ],

    stores: [
        'longtermprobject.ExecutionLongTermProgram',
        'ObjectCr'
    ],

    views: [
        'longtermprobject.executionlongtermprogram.Grid'
    ],

    mainView: 'longtermprobject.executionlongtermprogram.Grid',
    mainViewSelector: 'executionlongtermprogramgrid',

    refs: [
        {
            ref: 'executionLongTermProgramGrid',
            selector: 'executionlongtermprogramgrid'
        }
    ],
    
    init: function () {
        var me = this,
            actions = {
                'executionlongtermprogramgrid': {
                    render: me.onMainViewRender,
                    rowaction: { fn: me.onRowAction, scope: this }
                }
            };
        
        me.control(actions);
        me.callParent(arguments);
    },

    onMainViewRender: function (grid) {
        grid.getStore().on('beforeload', this.onBeforeLoadExecLongTermProgramStore, this);

        grid.getStore().load();
    },

    onBeforeLoadExecLongTermProgramStore: function (store, operation) {
        operation.params.realityObjectId = this.params.record.get('RealObjId');
    },
    
    onRowAction: function (grid, action, record) {
        var me = this,
            portal,
            model,
            params;

        me.mask('Загрузка', me.getMainView());
        if (action.toLowerCase() === 'edit') {
            Ext.History.add('objectcredit/' + objCrId + '/edit');
        }
    }
});