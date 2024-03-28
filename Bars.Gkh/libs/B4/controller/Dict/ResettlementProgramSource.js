Ext.define('B4.controller.dict.ResettlementProgramSource', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'resettlementProgramSourceGrid',
            permissionPrefix: 'Gkh.Dictionaries.ResettlementProgramSource'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'resettlementProgramSourceGridAspect',
            storeName: 'dict.ResettlementProgramSource',
            modelName: 'dict.ResettlementProgramSource',
            gridSelector: 'resettlementProgramSourceGrid'
        }
    ],

    models: ['dict.ResettlementProgramSource'],
    stores: ['dict.ResettlementProgramSource'],
    views: ['dict.resettlementprogramsource.Grid'],

    mainView: 'dict.resettlementprogramsource.Grid',
    mainViewSelector: 'resettlementProgramSourceGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'resettlementProgramSourceGrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('resettlementProgramSourceGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.ResettlementProgramSource').load();
    }
});