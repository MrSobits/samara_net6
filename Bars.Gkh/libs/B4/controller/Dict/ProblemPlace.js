Ext.define('B4.controller.dict.ProblemPlace', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: ['dict.ProblemPlace'],
    stores: ['dict.ProblemPlace'],
    views: ['dict.problemplace.Grid'],

    mainView: 'dict.problemplace.Grid',
    mainViewSelector: 'ProblemPlace',

    refs: [{
        ref: 'mainView',
        selector: 'problemplacegrid'
    }],

    mixins: {
        context: 'B4.mixins.Context'
    },

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: '#problemplacegrid',
            permissionPrefix: 'Gkh.Dictionaries.ProblemPlace'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'problemPlaceGridAspect',
            storeName: 'dict.ProblemPlace',
            modelName: 'dict.ProblemPlace',
            gridSelector: 'problemplacegrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('problemplacegrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.ProblemPlace').load();
    }
});