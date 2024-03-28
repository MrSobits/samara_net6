Ext.define('B4.controller.dict.BaseHouseEmergency', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],
    
    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'basehouseemergencygrid',
            permissionPrefix: 'Gkh.Dictionaries.BaseHouseEmergency'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'baseHouseEmergencyGridAspect',
            modelName: 'dict.BaseDict',
            gridSelector: 'basehouseemergencygrid'
        }
    ],
    
    models: ['dict.BaseDict'],
    views: ['dict.basehouseemergency.Grid'],

    mainView: 'dict.basehouseemergency.Grid',
    mainViewSelector: 'basehouseemergencygrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'basehouseemergencygrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('basehouseemergencygrid');
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore().load();
    }
});