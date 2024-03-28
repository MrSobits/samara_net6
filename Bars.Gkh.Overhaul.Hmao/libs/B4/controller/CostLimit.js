Ext.define('B4.controller.CostLimit', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow'
    ],
    stores: [
        'CostLimit',
        'CostLimitTypeWorkCr',
        'TypeWorksForSelect',
        'TypeWorksForSelected'
    ],
    models: [
        'CostLimit',
        'CostLimitTypeWorkCr'
    ],
    views: [
        'costlimit.Grid',
        'costlimit.Panel',
        'costlimit.EditWindow',
        'costlimit.ParamsWindow',
        'costlimit.CostLimitTypeWorkCrGrid'
    ],
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },
    refs: [
        {
            ref: 'mainView',
            selector: 'costlimitPanel'
        }
    ],
    mainView: 'costlimit.Panel',
    mainViewSelector: 'costlimitPanel',
    year: null,
    workId: null,
    capGroup: null,
    costLimId: null,
    //codeParam: null,
    init: function () {
        var me = this,
            actions = {
            };
        me.control(actions);
        me.callParent(arguments);
    },
    index: function () {
        var view = this.getMainView() || Ext.widget('costlimitPanel');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('CostLimit').load();
    },
    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'costlimitGridAspect',
            gridSelector: 'costlimitgrid',
            editFormSelector: '#costlimitEditWindow',
            storeName: 'CostLimit',
            modelName: 'CostLimit',
            editWindowView: 'costlimit.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions[this.gridSelector + ' button[action=ProcessCalculation]'] = { 'click': { fn: this.showParametersWindow, scope: this } };
                actions['costlimitparamswindow b4savebutton'] = { 'click': { fn: this.processCalculation, scope: this } };
            },
            showParametersWindow: function () {
                var me = this;
                win = Ext.widget('costlimitparamswindow');
                win.show();
            },
            processCalculation: function (btn) {
                debugger;
                var me = this,
                    win = btn.up('costlimitparamswindow');
                var nfCalcYear = win.down('#nfCalcYear');
                var nfCalcIndex = win.down('#nfCalcIndex');
                var calcYear = nfCalcYear.getValue();
                var calcIndex = nfCalcIndex.getValue();

                if (!calcYear) {
                    Ext.Msg.alert('Ошибка!', 'Не заполнен год фактически проведенных работ в краткосрочной программе');
                }
                if (!calcIndex) {
                    Ext.Msg.alert('Ошибка!', 'Не заполнен индекс потребительских цен');
                }
                if (calcYear && calcIndex) {
                    debugger;
                    me.mask('Рассчитываем предельные стоимости работ', win);
                    B4.Ajax.request(B4.Url.action('CalculateCostLimit', 'CostLimit', {
                        calcYear: calcYear,
                        calcIndex: calcIndex
                    })).next(function (response) {
                        debugger;
                        me.unmask();
                        Ext.Msg.alert('Результаты расчета', response.message);
                    }).error(function (response) {
                        me.unmask();
                        Ext.Msg.alert('Результаты расчета', response.message);
                    });

                }


            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    this.controller.costLimId = record.get('Id');
                    this.controller.year = record.get('Year');
                    this.controller.workId = record.get('Work');
                    this.controller.capGroup = record.get('CapitalGroup');
                    var store = form.down('costLimitTypeWorkCrGrid').getStore();

                    store.clearFilter(true);
                    store.filter([
                        { property: 'costLimId', value: this.controller.costLimId }
                    ]);
                }
            },
            //otherActions: function (actions) {
            //actions['#actualisedpkrEditWindow #cbNumberApartments'] = { 'change': { fn: this.onChangeNumberApartments, scope: this } },
            //},
            //onChangeNumberApartments: function (field, newValue) {
            //    var form = this.getForm(),
            //        cbNumberApartmentsCondition = form.down('#cbNumberApartmentsCondition'),
            //        nfNumberApartments = form.down('#nfNumberApartments');
            //    if (newValue == true) {
            //        cbNumberApartmentsCondition.setDisabled(false);
            //        nfNumberApartments.setDisabled(false);
            //    }
            //    else {
            //        cbNumberApartmentsCondition.setDisabled(true);
            //        nfNumberApartments.setDisabled(true);
            //    }
            //},
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'costLimitTypeWorkCrMultiselectWindowAspect',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#costLimitTypeWorkCrSelectWindow',
            storeSelect: 'TypeWorksForSelect',
            storeSelected: 'TypeWorksForSelected',
            gridSelector: 'costLimitTypeWorkCrGrid',
            columnsGridSelect: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    header: 'Название',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 1,
                    header: 'Адрес',
                    filter: { xtype: 'textfield' }
                }
            ],
            columnsGridSelected: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    header: 'Название',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 1,
                    header: 'Адрес',
                    filter: { xtype: 'textfield' }
                }
            ],
            titleSelectWindow: 'Выбор работ',
            titleGridSelect: 'Работы для выбора',
            titleGridSelected: 'Выбранные работы',
            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        workIds = [],
                        panel = me.getGrid().up('#costlimitEditWindow');

                    records.each(function (rec, index) { workIds.push(rec.get('Id')); });
                    if (workIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddWorks', 'CostLimit', {
                            workIds: Ext.encode(workIds),
                            costLimId: this.controller.costLimId
                        })).next(function (response) {
                            me.getGrid().getStore().load();
                            var obj = Ext.JSON.decode(response.responseText);
                            asp.controller.unmask();
                            Ext.Msg.alert('Сохранение!', 'Предельная стоимость пересчитана');
                            var debtCalcMethod = panel.down('#costlimitCost');
                            debtCalcMethod.setValue(obj.Cost);
                            
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать работы');
                        return false;
                    }
                    return true;
                },
            },
            onBeforeLoad: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.year = this.controller.year;
                operation.params.workId = this.controller.workId;
                operation.params.capGroup = this.controller.capGroup;
            },
        }
    ]
});