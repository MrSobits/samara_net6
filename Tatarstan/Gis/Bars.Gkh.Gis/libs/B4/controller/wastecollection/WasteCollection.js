Ext.define('B4.controller.wastecollection.WasteCollection', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GkhGridEditForm',
        'B4.controller.wastecollection.Navi',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'wastecollection.WasteCollectionPlace'
    ],

    stores: [
        'wastecollection.WasteCollectionPlace'
    ],

    views: [
        'wastecollection.Grid',
        'wastecollection.AddWindow'
    ],

    refs: [
       {
           ref: 'mainView',
           selector: 'wastecollectionplacegrid'
       }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.WasteCollectionPlaces.Create', applyTo: 'b4addbutton', selector: 'wastecollectionplacegrid' },
                {
                    name: 'Gkh.WasteCollectionPlaces.Delete', applyTo: 'b4deletecolumn', selector: 'wastecollectionplacegrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'wasteCollectionButtonExportAspect',
            gridSelector: 'wastecollectionplacegrid',
            buttonSelector: 'wastecollectionplacegrid #btnExport',
            controllerName: 'WasteCollectionPlace',
            actionName: 'Export',
            usePost: true
        },
        {
            xtype: 'gkhgrideditformaspect',
            gridSelector: 'wastecollectionplacegrid',
            editFormSelector: 'wastecollectionplaceaddwindow',
            storeName: 'wastecollection.WasteCollectionPlace',
            modelName: 'wastecollection.WasteCollectionPlace',
            editWindowView: 'wastecollection.AddWindow',
            controllerEditName: 'B4.controller.wastecollection.Navi',
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
                        me.controller.application.redirectTo(Ext.String.format('wastecollectionplaceedit/{0}', id));
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

    init: function () {
        var me = this;

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('wastecollectionplacegrid');
        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    }
});