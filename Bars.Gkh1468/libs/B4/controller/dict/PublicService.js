Ext.define('B4.controller.dict.PublicService', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            name: 'publicserviceGridAspect',
            storeName: 'dict.PublicService',
            modelName: 'dict.PublicService',
            gridSelector: 'publicServiceGrid'
        }
    ],
    
    models: ['dict.PublicService'],
    stores: ['dict.PublicService'],
    views: ['dict.publicservice.Grid'],

    mainView: 'dict.publicservice.Grid',
    mainViewSelector: 'publicServiceGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'publicServiceGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('publicServiceGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.PublicService').load();
    }
});