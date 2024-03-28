Ext.define('B4.controller.dict.TypeOfFeedback', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhInlineGrid'],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.TypeOfFeedback'],
    stores: ['dict.TypeOfFeedback'],

    views: ['dict.typeoffeedback.Grid'],

    mainView: 'dict.typeoffeedback.Grid',
    mainViewSelector: 'typeoffeedbackgrid',

    aspects: [

        {
            xtype: 'gkhinlinegridaspect',
            name: 'typeoffeedbackGridAspect',
            storeName: 'dict.TypeOfFeedback',
            modelName: 'dict.TypeOfFeedback',
            gridSelector: 'typeoffeedbackgrid'
        }
],

    index: function () {
        var view = this.getMainView() || Ext.widget('typeoffeedbackgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.TypeOfFeedback').load();
    }
});