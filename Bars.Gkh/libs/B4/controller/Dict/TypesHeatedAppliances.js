Ext.define('B4.controller.dict.TypesHeatedAppliances', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],
    
    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'typesheatedappliancesgrid',
            permissionPrefix: 'Gkh.Dictionaries.TypesHeatedAppliances'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'baseHouseEmergencyGridAspect',
            modelName: 'dict.BaseDict',
            gridSelector: 'typesheatedappliancesgrid'
        }
    ],
    
    models: ['dict.BaseDict'],
    views: ['dict.typesheatedappliances.Grid'],

    mainView: 'dict.typesheatedappliances.Grid',
    mainViewSelector: 'typesheatedappliancesgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'typesheatedappliancesgrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('typesheatedappliancesgrid');
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore().load();
    }
});