Ext.define('B4.controller.GisDataBank', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },
    models: [
        'GisDataBank'
    ],

    stores: [
        'GisDataBank'
    ],
    views: [
        'gisdatabank.Grid',
        'gisdatabank.Window'
    ],
    mainView: 'gisdatabank.Grid',
    mainViewSelector: 'gisdatabankgrid',
    
    refs: [
        {
            ref: 'mainView',
            selector: 'gisdatabankgrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Gis.KpSettings.GisDataBank.Create',
                    applyTo: 'b4addbutton',
                    selector: 'gisdatabankgrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'Gis.KpSettings.GisDataBank.Edit',
                    applyTo: 'b4editcolumn',
                    selector: 'gisdatabankgrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'Gis.KpSettings.GisDataBank.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'gisdatabankgrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'gisDataBankGridEditWindowAspect',
            gridSelector: 'gisdatabankgrid',
            editFormSelector: 'gisdatabankwindow',
            modelName: 'GisDataBank',
            editWindowView: 'gisdatabank.Window'
        }
    ],

    init: function () {
        this.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('gisdatabankgrid');

        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    }
});