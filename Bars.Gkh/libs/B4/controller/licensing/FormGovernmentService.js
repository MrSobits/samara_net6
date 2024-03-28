Ext.define('B4.controller.licensing.FormGovernmentService', {
    extend: 'B4.base.Controller',
    requires: [ 
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },
    
    models: ['licensing.FormGovernmentService'],
    views: ['licensing.formgovernmentservice.Grid', 'licensing.formgovernmentservice.AddWindow'],

    mainView: 'licensing.formgovernmentservice.Grid',
    mainViewSelector: 'formgovernmentservicegrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'formgovernmentservicegrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhGji.Licensing.FormGovService.Create', applyTo: 'b4addbutton', selector: 'formgovernmentservicegrid' },
                {
                    name: 'GkhGji.Licensing.FormGovService.Delete', applyTo: 'b4deletecolumn', selector: 'formgovernmentservicegrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'formGovernmentServiceGridWindowAspect',
            gridSelector: 'formgovernmentservicegrid',
            editFormSelector: 'formgovernmentserviceaddwindow',
            modelName: 'licensing.FormGovernmentService',
            editWindowView: 'licensing.formgovernmentservice.AddWindow',
            controllerEditName: 'B4.controller.licensing.GovernmenServiceDetail',
            editRecord: function (record) {
                var me = this,
                    id = record ? record.get('Id') : null,
                    model;

                model = me.controller.getModel(me.modelName);

                if (id) {
                    if (me.controllerEditName) {
                        me.controller.application.redirectTo(Ext.String.format('governmenservicedetail/{0}/', id));
                    } else {
                        model.load(id, {
                            success: function (rec) {
                                me.setFormData(rec);
                            },
                            scope: me
                        });
                    }
                } else {
                    me.setFormData(new model({ Id: 0 }));
                }
            }
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('formgovernmentservicegrid');
        
        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    }
});