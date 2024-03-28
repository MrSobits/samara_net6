Ext.define('B4.controller.dict.TechDecision', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhInlineGrid'
    ],

    models: [
        'dict.TechDecision'
    ],
    stores: [
        'dict.TechDecision'
    ],
    views: [
        'dict.techdecision.Grid'
    ],

    mainView: 'dict.techdecision.Grid',
    mainViewSelector: 'techdecisiongrid',

    refs: [{
        ref: 'mainView',
        selector: 'techdecisiongrid'
    }],

    mixins: {
        context: 'B4.mixins.Context'
    },

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            name: 'techDecisionGridAspect',
            storeName: 'dict.TechDecision',
            modelName: 'dict.TechDecision',
            gridSelector: 'techdecisiongrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('techdecisiongrid');
        this.bindContext(view);
        this.application.deployView(view);

        view.getStore().load();
    }
});