Ext.define('B4.controller.EmailLists', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['EmailLists'],
    stores: ['EmailLists'],

    views: ['emaillist.Grid'],

    mainView: 'emaillist.Grid',
    mainViewSelector: 'emaillistgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'emaillistgrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            name: 'emailListGridAspect',
            storeName: 'EmailLists',
            modelName: 'EmailLists',
            gridSelector: 'emaillistgrid'
        }
    ],

    index: function () {

        var view = this.getMainView() || Ext.widget('emaillistgrid');

        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('EmailLists').load();
    }
});
