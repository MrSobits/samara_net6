Ext.define('B4.controller.ContragentClw', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.controller.contragentclw.Navigation',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['ContragentClw'],
    stores: ['ContragentClw'],
    views: [
        'contragentclw.Grid',
        'contragentclw.AddWindow'
    ],

    refs: [
        {
            ref: 'AddWindow',
            selector: 'contragentclwaddwindow'
        },
        {
            ref: 'mainView',
            selector: 'contragentclwgrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Orgs.ContragentClw.Create', applyTo: 'b4addbutton', selector: 'contragentclwgrid' },
                {
                    name: 'Gkh.Orgs.ContragentClw.Delete', applyTo: 'b4deletecolumn', selector: 'contragentclwgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'gkhgrideditformaspect',
            gridSelector: 'contragentclwgrid',
            editFormSelector: 'contragentclwaddwindow',
            storeName: 'ContragentClw',
            modelName: 'ContragentClw',
            editWindowView: 'contragentclw.AddWindow',
            controllerEditName: 'B4.controller.contragentclw.Navigation',
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
                        me.controller.application.redirectTo(Ext.String.format('contragentclwedit/{0}', id));
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
            view = me.getMainView() || Ext.widget('contragentclwgrid');
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});