Ext.define('B4.controller.dict.FederalStandardFeeCr', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: ['dict.FederalStandardFeeCr'],
    stores: ['dict.FederalStandardFeeCr'],
    views: [
        'dict.federalstandardfeecr.Grid',
        'dict.federalstandardfeecr.EditWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    mainView: 'dict.federalstandardfeecr.Grid',
    mainViewSelector: 'federalstandardfeecrgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'federalstandardfeecrgrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhRegOp.Settings.FederalStandardFeeCr.Create', applyTo: 'b4addbutton', selector: 'federalstandardfeecrgrid' },
                { name: 'GkhRegOp.Settings.FederalStandardFeeCr.Edit', applyTo: 'b4savebutton', selector: 'federalstandardfeecreditwin' },
                { name: 'GkhRegOp.Settings.FederalStandardFeeCr.Delete', applyTo: 'b4deletecolumn', selector: 'federalstandardfeecrgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'federalstandardfeeCrWindowAspect',
            gridSelector: 'federalstandardfeecrgrid',
            editFormSelector: 'federalstandardfeecreditwin',
            storeName: 'dict.FederalStandardFeeCr',
            modelName: 'dict.FederalStandardFeeCr',
            editWindowView: 'dict.federalstandardfeecr.EditWindow',
            updateGrid: function () {
                this.getGrid().getStore().load();
            }
        }        
    ],

    init: function () {     
        this.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('federalstandardfeecrgrid');
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }    
});