Ext.define('B4.controller.realityobj.LiftRegister', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.realityobj.RealityObjectLiftEditPanelAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'ObjectCr',
        'realityobj.Lift'
    ],

    stores: [
        'realityobj.LiftRegister'
    ],

    views: [
        'realityobj.LiftRegisterGrid',
        'realityobj.LiftRegisterWindow'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'realityobjectliftregistergrid'
        }
    ],

    aspects: [
        {
            xtype: 'realityobjlifteditpanelaspect',
            name: 'realityobjliftregistergridwindowaspect',
            gridSelector: 'realityobjectliftregistergrid',
            editFormSelector: 'realityobjectliftregisterwindow',
            modelName: 'realityobj.Lift',
            editWindowView: 'realityobj.LiftRegisterWindow'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'liftRegisterButtonExportAspect',
            gridSelector: 'realityobjectliftregistergrid',
            buttonSelector: 'realityobjectliftregistergrid #btnExport',
            controllerName: 'RealityObject',
            actionName: 'ExportLiftsRegistry'
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'realityobjectliftregistergrid b4updatebutton': {
                click: { fn: me.updateGrid, scope: me }
            }
        });

        me.callParent(arguments);
    },

    index: function(id) {
        var me = this,
            view = me.getMainView() || Ext.widget('realityobjectliftregistergrid'),
            store = view.getStore();

        me.bindContext(view);
        me.application.deployView(view);

        store.clearFilter(true);
        store.filter('showAll', true);
    },

    updateGrid: function (btn) {
        btn.up('realityobjectliftregistergrid').getStore().load();
    }
});