Ext.define('B4.controller.RequestState', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['RequestState'],
    stores: ['RequestState'],

    views: ['requeststateperson.RSGrid'],

    mainView: 'requeststateperson.RSGrid',
    mainViewSelector: 'requeststategrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'requeststategrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            name: 'requestStateGridAspect',
            storeName: 'RequestState',
            modelName: 'RequestState',
            gridSelector: 'requeststategrid'
        }
    ],

    index: function () {

        var view = this.getMainView() || Ext.widget('requeststategrid');

        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('RequestState').load();
    }
});
