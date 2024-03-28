/*
    Delta начислений с группировкой - Controller
*/
Ext.define('B4.controller.delta.DeltaOfChargesOverride', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.QuickMsg',
        'B4.utils.KP6Utils'
    ],

    views: [
        'B4.view.delta.DeltaOfChargesOverride'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'deltaofchargesoverride'
        }
    ],

    RootNode: null,

    init: function () {
        var me = this;

        me.callParent(arguments);
        me.control({
            'deltaofchargesoverride b4updatebutton': {
                click: me.loadData
            },
            'deltaofchargesoverride [name=calcAccount]': {
                click: me.calcAccount
            },
            'deltaofchargesoverride [name=calcHouse]': {
                click: me.calcHouse
            },
            'deltaofchargesoverride [name=calcBank]': {
                click: me.calcBank
            },
            'deltaofchargesoverride checkbox[name=showPrev]': {
                change: me.showPrev
            },
            'deltaofchargesoverride checkbox[name=showNulls]': {
                change: me.showNulls
            },
            'deltaofchargesoverride': {
                afterrender: me.loadData,
                celldblclick: me.onShowProtocol
            }
        });
    },

    index: function (realityObjectId, personalAccountId) {
        var me = this,
            view = me.getMainView() || Ext.widget('deltaofchargesoverride'),
            storeMain = view.getStore();

        me.setContextValue(view, 'personalAccountId', personalAccountId);
        me.setContextValue(view, 'realityObjectId', realityObjectId);

        storeMain.getProxy().setExtraParam('personalAccountId', personalAccountId);
        storeMain.getProxy().setExtraParam('realityObjectId', realityObjectId);

        storeMain.on({

            beforeload: function () {
                me.prepareRequestData(storeMain, view);
            },
            load: function (store, records) {

                if (records == null || records.length === 0) {
                    B4.QuickMsg.msg(
                      'Внимание',
                      'Данные за указанный месяц отсутствуют',
                      'warning'
                  );
                }
            }
        });

        me.application.deployView(view, 'personalAccount_info');

    },

    showPrev: function (checkbox, value) {
        var me = this,
            view = me.getMainView();

        if (value) {
            view.down('datecolumn[dataIndex=ChargeDate]').show();
            view.down('numbercolumn[dataIndex=TariffPrev]').show();
            view.down('numbercolumn[dataIndex=ConsumptionPrev]').show();
            view.down('numbercolumn[dataIndex=FullCalculationPrev]').show();
            view.down('numbercolumn[dataIndex=CalculationDailyPrev]').show();
            view.down('numbercolumn[dataIndex=ShortDeliveryPrev]').show();
        } else {
            view.down('datecolumn[dataIndex=ChargeDate]').hide();
            view.down('numbercolumn[dataIndex=TariffPrev]').hide();
            view.down('numbercolumn[dataIndex=ConsumptionPrev]').hide();
            view.down('numbercolumn[dataIndex=FullCalculationPrev]').hide();
            view.down('numbercolumn[dataIndex=CalculationDailyPrev]').hide();
            view.down('numbercolumn[dataIndex=ShortDeliveryPrev]').hide();
        }

        view.getStore().getProxy().setExtraParam('showPrev', value);
        me.loadData();
    },


    showNulls: function (checkbox, value) {
        var me = this,
         view = me.getMainView();
        view.getStore().getProxy().setExtraParam('showNulls', value);
        me.loadData(checkbox);
    },
    //подготовка данных для запроса списка начислений
    prepareRequestData: function (store, view) {
        //Настройка колонок для отображения

        //Настраиваем группировку по услугам
        if (view.down('gridcolumn[dataIndex=ServiceName]').groupByColumn) {
            store.getProxy().setExtraParam('groupByService', true);
        } else {
            store.getProxy().setExtraParam('groupByService', false);
        }

        //Группировка по поставщикам
        if (view.down('gridcolumn[dataIndex=SupplierName]').groupByColumn) {
            store.getProxy().setExtraParam('groupBySupplier', true);
        } else {
            store.getProxy().setExtraParam('groupBySupplier', false);
        }

        //Группировка по формулам
        if (view.down('gridcolumn[dataIndex=FormulaName]').groupByColumn) {
            store.getProxy().setExtraParam('groupByFormula', true);
        } else {
            store.getProxy().setExtraParam('groupByFormula', false);
        }
    },

    //загрузить store с данными
    loadData: function (view) {
        view = view.xtype == "b4updatebutton" || view.xtype == "checkbox" ? view.up('deltaofchargesoverride') : view;
        var me = this,
            personalAccountId = me.getContextValue(view, 'personalAccountId'),
            realityObjectId = me.getContextValue(view, 'realityObjectId'),
            store = view.getStore();

        store.getProxy().setExtraParam('month', view.up('personalAccount_info_panel').down('combobox[name=month]').getValue());
        store.getProxy().setExtraParam('year', view.up('personalAccount_info_panel').down('combobox[name=year]').getValue());

        store.on('beforeload', function (st, operation) {
            operation.params = operation.params || {};
            operation.params.personalAccountId = personalAccountId;
            operation.params.realityObjectId = realityObjectId;
            operation.params.month = view.up('personalAccount_info_panel').down('combobox[name=month]').getValue();
            operation.params.year = view.up('personalAccount_info_panel').down('combobox[name=year]').getValue();
        }, me);
        store.load();
    },

    //Расчет лицевого счета
    calcAccount: function (component) {
        var me = this,
            view = component.up('deltaofchargesoverride'),
            personalAccountId = me.getContextValue(view, 'personalAccountId'),
            calcAccounts = Ext.decode(B4.Variables.getValue('KP60CalcAccounts')) || [],
            realityObjectId = me.getContextValue(view, 'realityObjectId');


        if (calcAccounts.indexOf(personalAccountId) != -1) {
            B4.QuickMsg.msg('Внимание', 'Расчет данного лицевого счета уже выполняется', 'warning');
            return;
        }

        Ext.Msg.confirm('Расчет счета', 'Вы хотите выполнить проверочный расчет счета?', function (result) {
            if (result == 'yes') {
                view.getEl().mask('Расчет...');

                calcAccounts.push(personalAccountId);
                B4.Variables.set('KP60CalcAccounts', Ext.encode(calcAccounts));

                B4.Ajax.request({
                    url: 'Host/Calculate',
                    method: 'POST',
                    params: {
                        personalAccount: personalAccountId,
                        realityObjectId: realityObjectId,
                        month: view.up('personalAccount_info_panel').down('combobox[name=month]').getValue(),
                        year: view.up('personalAccount_info_panel').down('combobox[name=year]').getValue()
                    },
                    timeout: 600000
                }).next(function (jsonResp) {
                    calcAccounts = Ext.decode(B4.Variables.getValue('KP60CalcAccounts'));
                    var index = calcAccounts.indexOf(personalAccountId);
                    calcAccounts.splice(index, 1);
                    B4.Variables.set('KP60CalcAccounts', Ext.encode(calcAccounts));

                    if (view.getEl()) {
                        view.getEl().unmask();
                        view.getStore().reload();
                    }

                    var response = Ext.decode(jsonResp.responseText);
                    B4.QuickMsg.msg(
                        response.success ? 'Выполнено' : 'Внимание',
                        response.data,
                        response.success ? 'success' : 'error'
                    );

                }).error(function (resp) {
                    if (view.getEl()) {
                        view.getEl().unmask();
                    }
                    B4.QuickMsg.msg(
                        'Внимание',
                        'При выполнении операции произошла ошибка',
                        'warning'
                    );

                    calcAccounts = Ext.decode(B4.Variables.getValue('KP60CalcAccounts'));
                    var index = calcAccounts.indexOf(personalAccountId);
                    calcAccounts.splice(index, 1);
                    B4.Variables.set('KP60CalcAccounts', Ext.encode(calcAccounts));
                });
            }
        }, me);
    },

    //Расчет дома
    calcHouse: function (component) {
        var me = this,
            view = component.up('deltaofchargesoverride'),
            personalAccountId = me.getContextValue(view, 'personalAccountId'),
            realityObjectId = me.getContextValue(view, 'realityObjectId');

        Ext.Msg.confirm('Расчет дома', 'Вы действительно хотите рассчитать</br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;лицевые счета дома?', function (result) {
            if (result == 'yes') {

                view.getEl().mask('Расчет...');

                B4.Ajax.request({
                    url: 'Host/CalculateHouse',
                    method: 'POST',
                    params: {
                        personalAccount: personalAccountId,
                        realityObjectId: realityObjectId,
                        month: view.up('personalAccount_info_panel').down('combobox[name=month]').getValue(),
                        year: view.up('personalAccount_info_panel').down('combobox[name=year]').getValue()
                    },
                    timeout: 600000
                }).next(function (jsonResp) {
                    view.getEl().unmask();
                    view.getStore().reload();

                    var response = Ext.decode(jsonResp.responseText);
                    B4.QuickMsg.msg(
                        'Внимание',
                        response.data,
                        response.success ? 'info' : 'error'
                    );

                }).error(function (resp) {
                    if (view.getEl()) {
                        view.getEl().unmask();
                    }
                    B4.QuickMsg.msg(
                        'Внимание',
                        'При выполнении операции произошла ошибка',
                        'warning'
                    );
                });
            }
        }, me);
    },

    //--------------------------------------------------------------
    //  показать протокол расчета
    //--------------------------------------------------------------
    onShowProtocol: function (cmp, td, cellIndex, record, tr, rowIndex, e, eOpts) {

        var me = this,
            view = me.getMainView(),
            personalAccountId = me.getContextValue(view, 'personalAccountId'),
            realityObjectId = me.getContextValue(view, 'realityObjectId'),
            yy = record.get('CurYear'),
            mm = record.get('CurMonth'),
            serviceId = record.get('ServiceId'),
            supplierId = record.get('SupplierId'),
            isGis = record.get('IsGis'),
            ServiceName = record.get('ServiceName');

        if (mm < 1 || mm > 12 || serviceId == 1)
            return;

        if (serviceId > 0) {
        } else {
            B4.QuickMsg.msg(
                'Внимание',
                'Необходимо показать столбец услуги',
                'error'
            );

            return;
        }

        //Протокол 5.0
        if (supplierId > 0) {
        } else {
            B4.QuickMsg.msg(
                'Внимание',
                'Необходимо показать столбец договора ЖКУ',
                'error'
            );

            return;
        }
        
        B4.utils.KP6Utils.onShowTreeProtocol(view, personalAccountId, realityObjectId, yy, mm, serviceId, supplierId, isGis, ServiceName);
        return;
    }

});