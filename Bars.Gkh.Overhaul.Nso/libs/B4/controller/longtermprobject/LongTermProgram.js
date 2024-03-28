Ext.define('B4.controller.longtermprobject.LongTermProgram', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.Ajax',
        'B4.Url'
    ],
    
    models: [
        'longtermprobject.LongTermProgram'
    ],
    
    stores: [
        'longtermprobject.LongTermProgram'
    ],
    
    views: [
        'longtermprobject.longtermprogram.Grid'
    ],

    mainView: 'longtermprobject.longtermprogram.Grid',
    mainViewSelector: 'longtermprogramgrid',
    
    refs: [
        {
            ref: 'longTermProgramGrid',
            selector: 'longtermprogramgrid'
        }
    ],

    init: function () {
        var me = this,
            actions = {
                'longtermprogramgrid': {
                    render: me.onMainViewRender
                }
            };
        
        me.control(actions);
        this.callParent(arguments);
    },

    onMainViewRender: function (grid) {
        grid.getStore().on('beforeload', this.onBeforeLoadLongTermProgramStore, this);

        grid.getStore().load();
    },

    onBeforeLoadLongTermProgramStore: function (store, operation) {
        operation.params.realityObjectId = this.params.record.get('RealObjId');
    }
});