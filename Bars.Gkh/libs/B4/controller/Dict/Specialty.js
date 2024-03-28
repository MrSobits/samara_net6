Ext.define('B4.controller.dict.Specialty', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: '#specialtyGrid',
            permissionPrefix: 'Gkh.Dictionaries.Specialty'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'specialtyGridAspect',
            storeName: 'dict.Specialty',
            modelName: 'dict.Specialty',
            gridSelector: 'specialtyGrid'
        }
    ],
    
    models: ['dict.Specialty'],
    stores: ['dict.Specialty'],
    views: ['dict.specialty.Grid'],

    mainView: 'dict.specialty.Grid',
    mainViewSelector: 'specialtyGrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'specialtyGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('specialtyGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.Specialty').load();
    }
});