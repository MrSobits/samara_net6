Ext.define('B4.controller.dict.RevenueSourceEdoInteg', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.view.dict.revenuesourcegji.Grid'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.RevenueSourceEdoInteg'],
    stores: ['dict.RevenueSourceEdoInteg'],

    views: [
        'dict.revsourceedointeg.Grid',
        'dict.revsourceedointeg.EditWindow'
    ],

    mainView: 'dict.revsourceedointeg.Grid',
    mainViewSelector: 'revSourceEdoIntegGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'revSourceEdoIntegGrid'
        }
    ],

    aspects: [
         {
             xtype: 'grideditwindowaspect',
             name: 'kindEquipmentGridWindowAspect',
             gridSelector: 'revSourceEdoIntegGrid',
             editFormSelector: '#revSourceEdoIntegEditWindow',
             storeName: 'dict.RevenueSourceEdoInteg',
             modelName: 'dict.RevenueSourceEdoInteg',
             editWindowView: 'dict.revsourceedointeg.EditWindow'
         }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('revSourceEdoIntegGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.RevenueSourceEdoInteg').load();
    }
});