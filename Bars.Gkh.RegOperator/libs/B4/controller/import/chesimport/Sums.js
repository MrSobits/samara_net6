Ext.define('B4.controller.import.chesimport.Sums', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhButtonPrintAspect'
    ],

    mixins: { context: 'B4.mixins.Context' },

    mainView: 'import.chesimport.SumsPanel',
    mainViewSelector: 'chesimportsumspanel',

    aspects: [
        {
            xtype: 'gkhbuttonprintaspect',
            buttonSelector: 'chesimportsumspanel button[action=Export]',
            codeForm: '',
            name: 'chesImportSumsPrintAspect',
            printController: 'ChesImport',
            printAction: 'Export',
            getUserParams: function () {
                var me = this,
                    periodId = me.controller.getContextValue(me.controller.getMainView(), 'periodId');

                me.params.periodId = periodId;
            }
        }
    ],

    init: function() {
        var me = this;

        me.control({
            'chesimportsumspanel': {
                'afterrender': {
                    fn: function(view) {
                        view.on('chargeStore.beforeload', me.onStoreBeforeLoad, me);
                        view.on('paymentStore.beforeload', me.onStoreBeforeLoad, me);
                        view.on('saldoStore.beforeload', me.onStoreBeforeLoad, me);
                        view.on('recalcStore.beforeload', me.onStoreBeforeLoad, me);
                    },
                    scope: me
                }
            },
            'chesimportsumspanel b4updatebutton': { 'click': { fn: this.loadStores, scope: this } },
            'chesimportsumspanel button[action=Export]': { 'click': { fn: this.onPrintButtonClick, scope: this } }
        });

        this.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);

        me.bindContext(view);
        me.application.deployView(view, 'chesPeriodId_Info');
        me.setContextValue(view, 'periodId', id);

        me.loadStores();
    },

    onStoreBeforeLoad: function(store, operation) {
        var view = this.getMainView();
        (operation.params || (operation.params = {})).periodId = this.getContextValue(view, 'periodId');
    },

    loadStores: function () {
        var me = this,
            view = me.getMainView();

        view.chargeStore.load();
        view.paymentStore.load();
        view.saldoStore.load();
        view.recalcStore.load();
    },

    onPrintButtonClick: function(btn) {
        var me = this;
        
        // вызываем сами экспорт, т.к. у нас просто кнопка печати
        me.getAspect('chesImportSumsPrintAspect').printReport(btn);
    }
});