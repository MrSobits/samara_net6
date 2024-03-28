Ext.define('B4.controller.realityobj.ProgramCr', {
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.aspects.GridEditWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: [
        'ObjectCr'
    ],
    
    stores: [
        'ObjectCr'
    ],
    
    views: [
        'realityobj.ProgramCrGrid'
    ],
    
    refs: [
        {
            ref: 'mainView',
            selector: 'realobjprogramcrgrid'
        }
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',
    aspects: [
        {
            xtype: 'gkhpermissionaspect', 
            name: 'realityObjProgramCrPerm',
            permissions: [
                {
                    name: 'GkhCr.ObjectCrViewCreate.View',
                    applyTo: 'actioncolumn',
                    selector: 'realobjprogramcrgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'realityobjProgramCrGridWindowAspect',
            gridSelector: 'realobjprogramcrgrid',
            storeName: 'ObjectCr',
            modelName: 'ObjectCr',
            rowAction: function (grid, action, record) {
                switch (action.toLowerCase()) {
                    case 'redirect':
                        this.controller.application.getRouter().redirectTo('objectcredit/' + record.get('Id'));
                        break;
                }
            }
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'realobjprogramcrgrid': {'store.beforeload': { fn: me.onBeforeLoad, scope: me }}
        });

        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('realobjprogramcrgrid');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');

        view.getStore().load();      
    },

    onBeforeLoad: function (store, operation) {
        var me = this,
            view = me.getMainView();

        operation.params.realityObjectId = me.getContextValue(view, 'realityObjectId');
    }
});