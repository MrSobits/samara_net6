Ext.define('B4.controller.administration.LogOperation', {
    /*
    * Контроллер логов импортов в систему
    */
    extend: 'B4.base.Controller',
   
    requires: [],
    mixins: {
        context: 'B4.mixins.Context'
    },
    models: ['administration.LogOperation'],
    stores: ['administration.LogOperation'],
    views: ['administration.LogOperationGrid'],

    mainView: 'administration.LogOperationGrid',
    mainViewSelector: 'operationloggrid',

    refs: [{
        ref: 'mainView',
        selector: 'operationloggrid'
    }],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('operationloggrid');
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});