Ext.define('B4.controller.bankstatement.Edit', {
/*
* Контроллер панели редактирования банковской выписки
*/
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhEditPanel'],

    models: ['BankStatement'],

    views: ['bankstatement.EditPanel'],
    
    mainView: 'bankstatement.EditPanel',
    mainViewSelector: '#bankStatementEditPanel',

    aspects: [
        {
            /*
            * Аспект отвечающий за редактирование данных грида банковской выписки
            */
            xtype: 'gkheditpanel',
            name: 'bankStatementEditPanelAspect',
            editPanelSelector: '#bankStatementEditPanel',
            modelName: 'BankStatement',
            otherActions: function (actions) {
                actions['#bankStatementEditPanel b4selectfield[name=Contragent]'] = { 'beforeload': { fn: this.onBeforeLoadContragent, scope: this } };
            },
            onBeforeLoadContragent: function (store, operation) {
                operation.params = {};
                operation.params.showAll = true;
            }
        }
    ],

    onLaunch: function () {
        if (this.params) {
            this.getAspect('bankStatementEditPanelAspect').setData(this.params.get('Id'));
        }
    }
});