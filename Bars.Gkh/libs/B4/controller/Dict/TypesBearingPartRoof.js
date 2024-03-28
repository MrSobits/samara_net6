Ext.define('B4.controller.dict.TypesBearingPartRoof', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'typesbearingpartroofgrid',
            permissionPrefix: 'Gkh.Dictionaries.TypesBearingPartRoof'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'baseHouseEmergencyGridAspect',
            modelName: 'dict.BaseDict',
            gridSelector: 'typesbearingpartroofgrid'
        }
    ],

    models: ['dict.BaseDict'],
    views: ['dict.typesbearingpartroof.Grid'],

    mainView: 'dict.typesbearingpartroof.Grid',
    mainViewSelector: 'typesbearingpartroofgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'typesbearingpartroofgrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('typesbearingpartroofgrid');
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore().load();
    }
});