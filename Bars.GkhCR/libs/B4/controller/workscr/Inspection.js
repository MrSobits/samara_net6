Ext.define('B4.controller.workscr.Inspection', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.Ajax', 'B4.Url',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    models: ['workscr.Inspection'],
    stores: ['workscr.Inspection', 'dict.Official'],

    views: [
        'workscr.InspectionGrid',
        'workscr.InspectionEditWindow'
    ],

    mainView: 'workscr.InspectionGrid',
    mainViewSelector: 'workscrinspectiongrid',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    parentCtrlCls: 'B4.controller.workscr.Navi',

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'inspectionEditWindow',
            gridSelector: 'workscrinspectiongrid',
            editFormSelector: 'workscrinspectionwin',
            storeName: 'workscr.Inspection',
            modelName: 'workscr.Inspection',
            editWindowView: 'workscr.InspectionEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('TypeWork', asp.controller.getTypeWorkId());
                    }
                }
            }
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'inspectionStatePermAspect',
            permissions: [
                { name: 'GkhCr.TypeWorkCr.Register.Inspection.Create', applyTo: 'b4addbutton', selector: 'workscrinspectiongrid' },
                { name: 'GkhCr.TypeWorkCr.Register.Inspection.Edit', applyTo: 'b4savebutton', selector: '#inspectionTypeWorkCrEditWindow' },
                {
                    name: 'GkhCr.TypeWorkCr.Register.Inspection.Delete', applyTo: 'b4deletecolumn', selector: 'workscrinspectiongrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }

            ]
        }
    ],

    index: function(id, objectId) {
        var me = this,
            view = me.getMainView();

        if (!view) {
            view = Ext.widget('workscrinspectiongrid');

            view.getStore().on('beforeload', function(arg0, operation) {
                operation.params.twId = id;
                operation.params.objectId = objectId;
            });
        }

        me.bindContext(view);
        me.setContextValue(view, 'twId', id);
        me.setContextValue(view, 'objectId', objectId);
        me.application.deployView(view, 'works_cr_info');

        view.getStore().load();
        me.getAspect('inspectionStatePermAspect').setPermissionsByRecord({ getId: function () { return id; } });
    },
    
    getTypeWorkId: function() {
        var me = this;
        return me.getContextValue(me.getMainView(), 'twId');
    }
});