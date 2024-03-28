Ext.define('B4.controller.dict.WarmingLayersAttics', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],
    
    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'warminglayersatticsgrid',
            permissionPrefix: 'Gkh.Dictionaries.WarmingLayersAttics'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'baseHouseEmergencyGridAspect',
            modelName: 'dict.BaseDict',
            gridSelector: 'warminglayersatticsgrid'
        }
    ],
    
    models: ['dict.BaseDict'],
    views: ['dict.warminglayersattics.Grid'],

    mainView: 'dict.warminglayersattics.Grid',
    mainViewSelector: 'warminglayersatticsgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'warminglayersatticsgrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('warminglayersatticsgrid');
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore().load();
    }
});