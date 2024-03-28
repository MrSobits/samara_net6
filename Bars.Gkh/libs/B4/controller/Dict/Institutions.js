Ext.define('B4.controller.dict.Institutions', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.dict.Institution'
    ],

    models: ['dict.Institutions'],
    stores: ['dict.Institutions'],
    views: [
        'dict.institutions.Grid',
        'dict.institutions.EditWindow'
    ],

    mainView: 'dict.institutions.Grid',
    mainViewSelector: 'institutionsGrid',

    refs: [{
        ref: 'mainView',
        selector: 'institutionsGrid'
    }],

    mixins: {
        context: 'B4.mixins.Context'
    },

    aspects: [
        {
            xtype: 'institutiondictperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'institutionsGridWindowAspect',
            gridSelector: 'institutionsGrid',
            editFormSelector: '#institutionsEditWindow',
            storeName: 'dict.Institutions',
            modelName: 'dict.Institutions',
            editWindowView: 'dict.institutions.EditWindow'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('institutionsGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.Institutions').load();
    }
});