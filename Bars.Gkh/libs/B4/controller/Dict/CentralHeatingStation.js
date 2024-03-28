Ext.define('B4.controller.dict.CentralHeatingStation', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.dict.CentralHeatingStation'
    ],

    models: ['dict.CentralHeatingStation'],
    stores: ['dict.CentralHeatingStation'],
    views: [
        'dict.institutions.Grid',
        'dict.institutions.EditWindow'
    ],

    mainView: 'dict.CentralHeatingStation.Grid',
    mainViewSelector: 'centralheatingstationGrid',

    refs: [{
        ref: 'mainView',
        selector: 'centralheatingstationGrid'
    }],

    mixins: {
        context: 'B4.mixins.Context'
    },

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'centralheatingstationGridWindowAspect',
            gridSelector: 'centralheatingstationGrid',
            editFormSelector: '#centralheatingstationEditWindow',
            storeName: 'dict.CentralHeatingStation',
            modelName: 'dict.CentralHeatingStation',
            editWindowView: 'dict.CentralHeatingStation.EditWindow'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('centralheatingstationGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.CentralHeatingStation').load();
    }
});