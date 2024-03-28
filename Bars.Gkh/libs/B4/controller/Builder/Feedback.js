Ext.define('B4.controller.builder.Feedback', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow'
    ],

    models: ['builder.Feedback'],
    stores: ['builder.Feedback'],
    views: [
        'builder.FeedbackEditWindow',
        'builder.FeedbackGrid'
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'builderFeedbackGridWindowAspect',
            gridSelector: '#builderFeedbackGrid',
            editFormSelector: '#builderFeedbackEditWindow',
            storeName: 'builder.Feedback',
            modelName: 'builder.Feedback',
            editWindowView: 'builder.FeedbackEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (this.controller.params && !record.data.Id) {
                        record.data.Builder = this.controller.params.get('Id');
                    }
                }
            }
        }
    ],

    params: null,
    mainView: 'builder.FeedbackGrid',
    mainViewSelector: '#builderFeedbackGrid',

    init: function () {
        this.getStore('builder.Feedback').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('builder.Feedback').load();
    },
    
    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.builderId = this.params.get('Id');
        }
    }
});