Ext.define('B4.controller.GasuIndicator', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['GasuIndicator'],
    stores: ['GasuIndicator'],
    views: ['gasuindicator.EditWindow', 'gasuindicator.Grid'],

    refs: [
        {
            ref: 'mainView',
            selector: 'gasuindicatorgrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Administration.ExportData.GasuIndicator.Create', applyTo: 'b4addbutton', selector: 'gasuindicatorgrid' },
                { name: 'Administration.ExportData.GasuIndicator.Edit', applyTo: 'b4savebutton', selector: 'gasuindicatoreditwin' },
                { name: 'Administration.ExportData.GasuIndicator.Delete', applyTo: 'b4deletecolumn', selector: 'gasuindicatorgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'gasuindicatorGridWindowAspect',
            gridSelector: 'gasuindicatorgrid',
            editFormSelector: 'gasuindicatoreditwin',
            storeName: 'GasuIndicator',
            modelName: 'GasuIndicator',
            editWindowView: 'gasuindicator.EditWindow',
            updateGrid: function () {
                this.getGrid().getStore().load();
            }
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('gasuindicatorgrid');
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});