Ext.define('B4.controller.dict.TypeNpa', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],
    
    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'typeNpaGridAspect',
            permissionPrefix: 'Gkh.Dictionaries.TypeNpa'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'typeNpaGridAspect',
            modelName: 'dict.BaseDict',
            gridSelector: 'typenpagrid'
        }
    ],
    
    models: ['dict.BaseDict'],
    views: ['dict.typenpa.Grid'],

    mainView: 'dict.typenpa.Grid',
    mainViewSelector: 'typenpagrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'typenpagrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('typenpagrid');
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore().load();
    }
});