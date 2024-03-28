Ext.define('B4.controller.dict.MKDLicTypeRequest', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhInlineGrid'],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.MKDLicTypeRequest'],
    stores: ['dict.MKDLicTypeRequest'],

    views: ['dict.mkdlictyperequest.Grid'],

    mainView: 'dict.mkdlictyperequest.Grid',
    mainViewSelector: 'mkdlictyperequestGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'mkdlictyperequestGrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            name: 'mkdlictyperequestGridAspect',
            storeName: 'dict.MKDLicTypeRequest',
            modelName: 'dict.MKDLicTypeRequest',
            gridSelector: 'mkdlictyperequestGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('mkdlictyperequestGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.MKDLicTypeRequest').load();
    }
});