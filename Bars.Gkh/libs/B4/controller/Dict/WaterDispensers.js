Ext.define('B4.controller.dict.WaterDispensers', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],
    
    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'waterdispensersgrid',
            permissionPrefix: 'Gkh.Dictionaries.WaterDispensers'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'baseHouseEmergencyGridAspect',
            modelName: 'dict.BaseDict',
            gridSelector: 'waterdispensersgrid'
        }
    ],
    
    models: ['dict.BaseDict'],
    views: ['dict.waterdispensers.Grid'],

    mainView: 'dict.waterdispensers.Grid',
    mainViewSelector: 'waterdispensersgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'waterdispensersgrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('waterdispensersgrid');
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore().load();
    }
});