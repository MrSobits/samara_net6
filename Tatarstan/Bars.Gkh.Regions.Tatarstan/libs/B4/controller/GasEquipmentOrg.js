Ext.define('B4.controller.GasEquipmentOrg', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.controller.gasequipmentorg.Navigation',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: ['GasEquipmentOrg'],
    stores: ['GasEquipmentOrg'],
    views: [
        'gasequipmentorg.Grid',
        'gasequipmentorg.AddWindow'
    ],

    mainView: 'gasequipmentorg.Grid',
    mainViewSelector: 'gasequipmentorggrid',

    refs: [
        {
            ref: 'AddWindow',
            selector: 'gasequipmentorgaddwindow'
        },
        {
            ref: 'mainView',
            selector: 'gasequipmentorggrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Orgs.GasEquipmentOrg.Create', applyTo: 'b4addbutton', selector: 'gasequipmentorgmkdaddwindow' },
                { name: 'Gkh.Orgs.GasEquipmentOrg.Edit', applyTo: 'b4savebutton', selector: 'gasequipmentorggrid' },
                { name: 'Gkh.Orgs.GasEquipmentOrg.Delete', applyTo: 'b4deletecolumn', selector: 'gasequipmentorggrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'gkhgrideditformaspect',
            gridSelector: 'gasequipmentorggrid',
            editFormSelector: 'gasequipmentorgaddwindow',
            modelName: 'GasEquipmentOrg',
            editWindowView: 'gasequipmentorg.AddWindow',
            deleteWithRelatedEntities: true,
            controllerEditName: 'B4.controller.gasequipmentorg.Navigation',
            rowAction: function (grid, action, record) {
                if (this.fireEvent('beforerowaction', this, grid, action, record) !== false) {
                    switch (action.toLowerCase()) {
                        case 'edit':
                            this.editRecord(record);
                            break;
                        case 'delete':
                            this.deleteRecord(record);
                            break;
                    }
                }
            },
            editRecord: function (record) {
                var me = this,
                    id = record ? record.get('Id') : null,
                    model;

                model = me.controller.getModel(me.modelName);

                if (id) {
                    if (me.controllerEditName) {
                        me.controller.application.redirectTo(Ext.String.format('gasequipmentorgedit/{0}', id));
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
        },
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('gasequipmentorggrid');
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore('GasEquipmentOrg').load();
    },
});