Ext.define('B4.controller.dict.ModelLift', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'modelLiftGrid',
            permissionPrefix: 'Gkh.Dictionaries.ModelLift'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'modelLiftGridAspect',
            storeName: 'dict.ModelLift',
            modelName: 'dict.ModelLift',
            gridSelector: 'modelLiftGrid'
        }
    ],

    models: ['dict.ModelLift'],
    stores: ['dict.ModelLift'],
    views: ['dict.modellift.Grid'],

    mainView: 'dict.modellift.Grid',
    mainViewSelector: 'modelLiftGrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'modelLiftGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('modelLiftGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.ModelLift').load();
    }
});