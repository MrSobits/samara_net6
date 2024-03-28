Ext.define('B4.controller.realityobj.Lift', {
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.realityobj.RealityObjectLiftEditPanelAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'ObjectCr',
        'realityobj.Lift',
        'realityobj.LiftSummary'
    ],

    stores: [

    ],

    views: [
        'realityobj.LiftGrid',
        'realityobj.LiftWindow',
        'realityobj.LiftPanel',
        'realityobj.LiftSummaryPanel'
    ],

    mainView: 'realityobj.LiftPanel',
    mainViewSelector: 'realityobjliftpanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'realityobjliftpanel'
        }
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'realityObjectLiftPermissions',
            permissions: [
                {
                     name: 'Gkh.RealityObject.Register.Lift.Create', applyTo: 'b4addbutton', selector: 'realityobjectliftgrid'
                },
                {
                    name: 'Gkh.RealityObject.Register.Lift.Edit', applyTo: 'b4savebutton', selector: 'realityobjectliftwindow'
                },
                {
                    name: 'Gkh.RealityObject.Register.Lift.Delete', applyTo: 'b4deletecolumn', selector: 'realityobjectliftgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'gkheditpanel',
            name: 'realityobjLiftPanelAspect',
            editPanelSelector: 'roliftsummarypanel',
            modelName: 'realityobj.LiftSummary'
        },
        {
            xtype: 'realityobjlifteditpanelaspect',
            name: 'realityobjliftgridwindowaspect'
        }
    ],

    index: function(id) {
        var me = this,
            view = me.getMainView() || Ext.widget('realityobjliftpanel'),
            store;

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');
        me.getAspect('realityobjLiftPanelAspect').setData(id);
        me.getAspect('realityObjectLiftPermissions').setPermissionsByRecord({ getId: function () { return id; } });

        store = view.down('realityobjectliftgrid').getStore();
        store.clearFilter(true);
        store.filter('ro_id', id);
    }
});