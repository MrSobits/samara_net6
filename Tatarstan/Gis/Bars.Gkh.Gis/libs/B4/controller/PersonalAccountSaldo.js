Ext.define('B4.controller.PersonalAccountSaldo', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.aspects.PeriodFilterGrid',
        'B4.QuickMsg'
    ],

    mixins: { context: 'B4.mixins.Context' },
    views: [
        'personalAccount.SaldoGrid',
        'analysisreport.AnalysisReportForm'
    ],
    mainView: 'personalAccount.SaldoGrid',
    mainViewSelector: 'personalAccount_saldo_grid',

    init: function() {
        var me = this;

        this.callParent(arguments);

        this.control({
            'personalAccount_saldo_grid': {
                afterrender: me.onLoad,
                reconfigure: function (column) {
                    column.up('gridpanel').getStore().load();
                }
            },
            'personalAccount_saldo_grid b4updatebutton': {
                'click': function() {
                    this.getMainView().getStore().reload();
                },
                scope: this
            },
            'personalAccount_saldo_grid button[name=calculate]': {
                'click': me.calculate
            },
            'personalAccount_saldo_grid b4editcolumn': {
                'click': this.openProtocol
            },
            

            'personalAccount_info_panel combobox[name=month]': {
                'change': function(cmp) {
                    //this.getMainView().getStore().reload();
                    var grid = cmp.up('panel').down('personalAccount_saldo_grid');
                    grid.getStore().reload();
                },
                scope: this
            },
            'personalAccount_info_panel combobox[name=year]': {
                'change': function(cmp) {
                    //this.getMainView().getStore().reload();
                    cmp.up('panel').down('personalAccount_saldo_grid').getStore().reload();
                },
                scope: this
            }
        });
    },

    refs: [
        {
            ref: 'infoPanel',
            selector: 'personalAccount_info_panel'
        },
        {
            ref: 'comboMonth',
            selector: 'personalAccount_info_panel combobox[name=month]'
        },
        {
            ref: 'comboYear',
            selector: 'personalAccount_info_panel combobox[name=year]'
        }
    ],

    index: function(id) {
        var me = this,
            view = this.getMainView() || Ext.widget('personalAccount_saldo_grid');

        me.bindContext(view);
        me.setContextValue(view, 'apartmentId', id);
        me.application.deployView(view, 'personalAccount_info');
    },

    onLoad: function (view) {

        var me = this,
            store = view.getStore();

        //Подписка на события
        store.on('beforeload', me.onBeforeLoadSaldoList, me);
        store.load();
    },

    //подготовка данных для запроса списка сальдо
    onBeforeLoadSaldoList: function(store, operation) {

        var me = this,
            view = this.getMainView() || Ext.ComponentQuery.query('personalAccount_saldo_grid')[0], //cmp.up('panel').down('personalAccount_saldo_grid'),
            apartmentId = me.getContextValue(view, 'apartmentId'),
            comboMonth,
            comboYear;

        comboMonth = this.getComboMonth();
        comboYear = this.getComboYear();
        if (!view || !comboMonth.getValue() || !comboYear.getValue()) {
            return;
        }

        //Настройка колонок для отображения
        if (view.down('gridcolumn[dataIndex=Service]').groupByColumn) {
            operation.params.groupByService = true;
        }
        if (view.down('gridcolumn[dataIndex=Supplier]').groupByColumn) {
            operation.params.groupBySupplier = true;
        }

        operation.params.apartmentId = apartmentId;
        operation.params.month = comboMonth.getValue();
        operation.params.year = comboYear.getValue();
    },

    calculate: function() {
        var me = this,
            view = me.getMainView(),
            apartmentId = me.getContextValue(view, 'apartmentId'),
            comboMonth,
            comboYear;

        comboMonth = me.getComboMonth(); //comboMonth.getValue();
        comboYear = me.getComboYear(); //comboMonth.getValue();

        Ext.Msg.confirm('Расчет начислений',
            'Вы действительно хотите выполнить расчет начислений?', function (result) {

                if (result == 'yes') {

                    view.getEl().mask('Расчет...');

                    B4.Ajax.request({
                        url: B4.Url.action('Calculate', 'Host'),
                        method: 'POST',
                        params: {
                            personalAccount: apartmentId,
                            year: 0, //yy,
                            month: 0 //mm
                        },
                        timeout: 600000
                    }).next(function(jsonResp) {

                        view.getEl().unmask();

                        var response = Ext.decode(jsonResp.responseText);
                        B4.QuickMsg.msg(
                            response.success ? 'Выполнено' : 'Внимание',
                            response.data,
                            response.success ? 'success' : 'error'
                        );


                    }).error(function(response) {

                        view.getEl().unmask();
                        Ext.Msg.alert('Ошибка!', !Ext.isString(response.message) ? 'При выполнении операции произошла ошибка!' : response.message);

                    });

                }
            }, me);
    },

    //--------------------------------------------------------------
    //  показать протокол расчета в виде дерева
    //--------------------------------------------------------------
    openProtocol: function (object) {

        var me = this,
            sel = object.store.getRange()[object.highlightedItem.viewIndex].raw,
            mm = me.getComboMonth().getValue(),
            yy = me.getComboYear().getValue(),
            view = me.getMainView();

        B4.utils.KP6Utils.onShowTreeProtocol(view, sel.personalAccountId, 0, yy, mm, sel.serviceId, sel.supplierId, sel.serviceName, 0);
    }

});