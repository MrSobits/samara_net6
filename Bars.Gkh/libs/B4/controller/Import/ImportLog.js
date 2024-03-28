Ext.define('B4.controller.Import.ImportLog', {
    /*
    * Контроллер логов импортов в систему
    */
    extend: 'B4.base.Controller',
   
    mixins: {
        context: 'B4.mixins.Context'
    },
    
    models: ['Import.Log'],
    stores: ['Import.Log'],
    views: ['Import.LogGrid'],

    mainView: 'Import.LogGrid',
    mainViewSelector: 'importLogGrid',

    refs: [{
        ref: 'mainView',
        selector: 'importLogGrid'
    }],
    
    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('importLogGrid'),
            store = view.getStore();
        me.bindContext(view);
        me.application.deployView(view);
        
        store.load();
    },
});