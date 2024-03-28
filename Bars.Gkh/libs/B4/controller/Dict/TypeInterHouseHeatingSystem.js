Ext.define('B4.controller.dict.TypeInterHouseHeatingSystem', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],
    
    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'typeinterhouseheatingsystemgrid',
            permissionPrefix: 'Gkh.Dictionaries.TypeInterHouseHeatingSystem'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'baseHouseEmergencyGridAspect',
            modelName: 'dict.BaseDict',
            gridSelector: 'typeinterhouseheatingsystemgrid'
        }
    ],
    
    models: ['dict.BaseDict'],
    views: ['dict.typeinterhouseheatingsystem.Grid'],

    mainView: 'dict.typeinterhouseheatingsystem.Grid',
    mainViewSelector: 'typeinterhouseheatingsystemgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'typeinterhouseheatingsystemgrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('typeinterhouseheatingsystemgrid');
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore().load();
    }
});