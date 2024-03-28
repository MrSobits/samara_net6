Ext.define('B4.controller.dict.SocialStatus', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.SocialStatus'],
    stores: ['dict.SocialStatus'],

    views: ['dict.socialstatus.Grid'],

    mainView: 'dict.socialstatus.Grid',
    mainViewSelector: 'socialstatusgrid',
    
    refs: [
        {
            ref: 'mainView',
            selector: 'socialstatusgrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            name: 'socialStatusGridAspect',
            storeName: 'dict.SocialStatus',
            modelName: 'dict.SocialStatus',
            gridSelector: 'socialstatusgrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('socialstatusgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.SocialStatus').load();
    }
});