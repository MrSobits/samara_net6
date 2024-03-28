Ext.define('B4.controller.dict.VideoOverwatchType', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid'
    ],

    models: ['dict.VideoOverwatchType'],
    stores: ['dict.VideoOverwatchType'],
    views: ['dict.videooverwatchtype.Grid'],

    mainView: 'dict.videooverwatchtype.Grid',
    mainViewSelector: 'videooverwatchtypeGrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'videooverwatchtypeGrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            name: 'videooverwatchtypeGridAspect',
            storeName: 'dict.VideoOverwatchType',
            modelName: 'dict.VideoOverwatchType',
            gridSelector: 'videooverwatchtypeGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('videooverwatchtypeGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.VideoOverwatchType').load();
    }
});