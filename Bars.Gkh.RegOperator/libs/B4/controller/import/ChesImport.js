Ext.define('B4.controller.import.ChesImport', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhButtonImportAspect',
        'B4.aspects.GridEditForm',
        'B4.enums.ChesImportState',
        'B4.enums.ChesAnalysisState',
        'B4.aspects.GridEditWindow'
    ],

    mixins: { context: 'B4.mixins.Context' },

    views: [
        'import.chesimport.ChesImportGrid',
        'import.chesimport.ImportWindow'
    ],

    mainView: 'import.chesimport.ChesImportGrid',
    mainViewSelector: 'chesimportgrid',

    aspects: [
        {
            /*
            *аспект для импорта
            */
            xtype: 'gkhbuttonimportaspect',
            name: 'chesImportButtonAspect',
            buttonSelector: 'chesimportgrid #btnImport',
            codeImport: 'ChesImport',
            windowImportView: 'import.chesimport.ImportWindow',
            windowImportSelector: 'chesimportimportwindow'
        },
        {
            xtype: 'grideditformaspect',
            name: 'chesImportEditFormAspect',
            gridSelector: 'chesimportgrid',
            modelName: 'import.ChesImport',
            editRecord: function (record) {
                var me = this,
                    id = record.getId();

                me.redirectToEdit(id);
            },
            redirectToEdit: function(objectId) {
                Ext.History.add('chesimport_detail/' + objectId);
            }
            
        }
        //{
        //    xtype: 'gkhbuttonprintaspect',
        //    buttonSelector: 'chesimportanalyzewindow button[action=Export]',
        //    codeForm: '',
        //    name: 'chesImportanalyzePrintAspect',
        //    printController: 'ChesImport',
        //    printAction: 'Export',
        //    getUserParams: function () {
        //        var me = this,
        //            view = me.controller.getAnalyzeWindow();

        //        me.params.periodId = view.down('[name=ChargePeriod]').getValue();
        //    }
        //}
    ],

    init: function() {
        this.control({
            //'chesimportpanel button[action=showAnalyze]': { 'click': { fn: this.onClickAnalyze, scope: this } },
            //'chesimportanalyzewindow button[action=Export]': { 'click': { fn: this.onPrintButtonClick, scope: this } }
        });

        this.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);

        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
        me.getAspect('chesImportButtonAspect').loadImportStore();
    },

    onStoreBeforeLoad: function(store, operation) {
        (operation.params || (operation.params = {})).periodId = this.getAnalyzeWindow().down('[name=ChargePeriod]').getValue();
    },

    loadStores: function (window) {
        window.chargeStore.load();
        window.paymentStore.load();
        window.saldoStore.load();
        window.recalcStore.load();
    },

    onPrintButtonClick: function(btn) {
        var me = this;
        
        // вызываем сами экспорт, т.к. у нас просто кнопка печати
        me.getAspect('chesImportanalyzePrintAspect').printReport(btn);
    },

    onClickAnalyze: function() {
        var me = this,
            window = Ext.widget('chesimportanalyzewindow');

        // подписываемся перед загрузкой
        window.on('chargeStore.beforeload', me.onStoreBeforeLoad, me);
        window.on('paymentStore.beforeload', me.onStoreBeforeLoad, me);
        window.on('saldoStore.beforeload', me.onStoreBeforeLoad, me);
        window.on('recalcStore.beforeload', me.onStoreBeforeLoad, me);

        // при изменении периода грузим данные
        window.down('[name=ChargePeriod]').on('change', function () { me.loadStores(window); }, window);

        window.show();
    }
});