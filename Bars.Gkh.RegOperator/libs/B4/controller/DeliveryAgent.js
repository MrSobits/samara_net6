Ext.define('B4.controller.DeliveryAgent', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.controller.deliveryagent.Navigation',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['DeliveryAgent'],
    stores: ['DeliveryAgent'],
    views: [
        'deliveryagent.Grid',
        'deliveryagent.AddWindow'
    ],

    refs: [
        {
            ref: 'AddWindow',
            selector: 'deliveryagentaddwindow'
        },
        {
            ref: 'mainView',
            selector: 'deliveryagentgrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Orgs.DeliveryAgent.Create', applyTo: 'b4addbutton', selector: 'deliveryagentgrid' },
                { name: 'Gkh.Orgs.DeliveryAgent.Delete', applyTo: 'b4deletecolumn', selector: 'deliveryagentgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'gkhgrideditformaspect',
            gridSelector: 'deliveryagentgrid',
            editFormSelector: 'deliveryagentaddwindow',
            storeName: 'DeliveryAgent',
            modelName: 'DeliveryAgent',
            editWindowView: 'deliveryagent.AddWindow',
            controllerEditName: 'B4.controller.deliveryagent.Navigation',
            deleteWithRelatedEntities: true,
            updateGrid: function () {
                this.getGrid().getStore().load();
            },
            editRecord: function (record) {
                var me = this,
                    id = record ? record.data.Id : null,
                    model;

                model = me.controller.getModel(me.modelName);

                if (id) {
                    if (me.controllerEditName) {
                        me.controller.application.redirectTo(Ext.String.format('deliveryagentedit/{0}', id));
                    }
                    else {
                        model.load(id, {
                            success: function (rec) {
                                me.setFormData(rec);
                            },
                            scope: me
                        });
                    }
                }
                else {
                    me.setFormData(new model({ Id: 0 }));
                }
            }
        }
    ],
    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('deliveryagentgrid');
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});