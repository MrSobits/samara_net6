Ext.define('B4.controller.dict.TypesExteriorWalls', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],
    
    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'typesexteriorwallsgrid',
            permissionPrefix: 'Gkh.Dictionaries.TypesExteriorWalls'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'baseHouseEmergencyGridAspect',
            modelName: 'dict.BaseDict',
            gridSelector: 'typesexteriorwallsgrid'
        }
    ],
    
    models: ['dict.BaseDict'],
    views: ['dict.typesexteriorwalls.Grid'],

    mainView: 'dict.typesexteriorwalls.Grid',
    mainViewSelector: 'typesexteriorwallsgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'typesexteriorwallsgrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('typesexteriorwallsgrid');
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore().load();
    }
});