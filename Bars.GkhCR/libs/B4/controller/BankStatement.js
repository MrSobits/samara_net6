Ext.define('B4.controller.BankStatement', {
/*
* Контроллер раздела банковских выписок
*/
    extend: 'B4.base.Controller',
    requires:
    [
       'B4.aspects.GkhGridEditForm',
       'B4.aspects.permission.BankStatement'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },
    
    models: ['BankStatement'],
    stores: ['BankStatement'],
    views: ['bankstatement.Grid', 'bankstatement.AddWindow'],

    mainView: 'bankstatement.Grid',
    mainViewSelector: 'bankStatementGrid',
    
    refs: [
        {
            ref: 'mainView',
            selector: 'bankStatementGrid'
        }
    ],

    aspects: [
        {
            xtype: 'bankstatementperm'
        },
        {
        /*
        * Аспект взаимодействия таблицы банковских выписок и формы редактирования
        */
            xtype: 'gkhgrideditformaspect',
            name: 'bankStatementGridWindowAspect',
            gridSelector: 'bankStatementGrid',
            editFormSelector: '#bankStatementAddWindow',
            storeName: 'BankStatement',
            modelName: 'BankStatement',
            editWindowView: 'bankstatement.AddWindow',
            controllerEditName: 'B4.controller.bankstatement.Navigation'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('bankStatementGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('BankStatement').load();
    }
});