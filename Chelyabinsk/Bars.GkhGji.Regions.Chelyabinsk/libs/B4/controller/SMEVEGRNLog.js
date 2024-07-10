Ext.define('B4.controller.SMEVEGRNLog', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['smev.SMEVEGRNLog'],
    stores: ['smev.SMEVEGRNLog'],

    views: ['smevegrn.LogGrid'],

    mainView: 'smevegrn.LogGrid',
    mainViewSelector: 'smevegrnloggrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'smevegrnloggrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            name: 'smevegrnloggridAspect',
            storeName: 'smev.SMEVEGRNLog',
            modelName: 'smev.SMEVEGRNLog',
            gridSelector: 'smevegrnloggrid'
        }
    ],

    index: function () {

        var view = this.getMainView() || Ext.widget('smevegrnloggrid');

        this.bindContext(view);
        this.application.deployView(view);
        
        this.getStore('smev.SMEVEGRNLog').load();
    }
});
