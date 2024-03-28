Ext.define('B4.controller.HousingInspection', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.controller.housinginspection.Navigation',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: ['HousingInspection'],
    stores: ['HousingInspection'],
    views: [
        'housinginspection.Grid',
        'housinginspection.AddWindow'
    ],

    mainView: 'housinginspection.Grid',
    mainViewSelector: 'housinginspectiongrid',

    refs: [
        {
            ref: 'AddWindow',
            selector: 'housinginspectionaddwindow'
        },
        {
            ref: 'mainView',
            selector: 'housinginspectiongrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Orgs.HousingInspection.Create', applyTo: 'b4addbutton', selector: 'housinginspectiongrid' },
                {
                    name: 'Gkh.Orgs.HousingInspection.Delete', applyTo: 'b4deletecolumn', selector: 'housinginspectiongrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'gkhgrideditformaspect',
            gridSelector: 'housinginspectiongrid',
            editFormSelector: 'housinginspectionaddwindow',
            modelName: 'HousingInspection',
            editWindowView: 'housinginspection.AddWindow',
            deleteWithRelatedEntities: true,
            controllerEditName: 'B4.controller.housinginspection.Navigation',
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
                        me.controller.application.redirectTo(Ext.String.format('housinginspectionedit/{0}', id));
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
        var view = this.getMainView() || Ext.widget('housinginspectiongrid');
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore('ManagingOrganization').load();
    },
});