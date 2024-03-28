/*
    Проверочный расчет Grid
*/
Ext.define('B4.view.delta.DeltaOfChargesOverride', {
    extend: 'Ext.grid.Panel',

    alias: 'widget.deltaofchargesoverride',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.store.delta.DeltaOfChargesOverride'
    ],

    title: 'Проверочный расчет',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.delta.DeltaOfChargesOverride', {
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'Host', //'Delta',
                    listAction: 'Listing',
                    timeout: 999999
                }
            });

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            enableColumnHide: true,
            columns: [
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ChargeDate',
                    hideable: false,
                    hidden: true,
                    minWidth: 120,
                    format: 'm.Y',
                    align: 'center',
                    flex: 1,
                    text: 'Месяц перерасчета'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ServiceName',
                    flex: 2,
                    text: 'Услуга',
                    groupByColumn: true,
                    minWidth: 150,
                    listeners: {
                        //Если колонка была скрыта, а теперь видна, то перегружаем стор
                        beforeshow: function (column) {
                            if (column.hidden) {
                                column.groupByColumn = true;
                                column.up('gridpanel').getStore().load();
                            }
                            column.up('panel').columns[2].setVisible(true);
                        },
                        //Аналогично со скрытием колонки
                        beforehide: function (column) {
                            if (!column.hidden) {
                                column.groupByColumn = false;
                                column.up('gridpanel').getStore().load();
                            }
                            column.up('panel').columns[2].setVisible(false);
                        }
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Measure',
                    hideable: false,
                    width: 70,
                    text: 'Единица измерения'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SupplierName',
                    flex: 1,
                    text: 'Договор ЖКУ',
                    groupByColumn: true,
                    minWidth: 150,
                    hidden: false,
                    listeners: {
                        //Если колонка была скрыта, а теперь видна, то перегружаем стор
                        beforeshow: function (column) {
                            if (column.hidden) {
                                column.groupByColumn = true;
                                column.up('gridpanel').getStore().load();
                            }
                        },
                        //Аналогично со скрытием колонки
                        beforehide: function (column) {
                            if (!column.hidden) {
                                column.groupByColumn = false;
                                column.up('gridpanel').getStore().load();
                            }
                        }
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FormulaName',
                    flex: 1,
                    text: 'Формула',
                    groupByColumn: false,
                    minWidth: 150,
                    hidden: true,
                    listeners: {
                        //Если колонка была скрыта, а теперь видна, то перегружаем стор
                        beforeshow: function (column) {
                            if (column.hidden) {
                                column.groupByColumn = true;
                                column.up('gridpanel').getStore().load();
                            }
                        },
                        //Аналогично со скрытием колонки
                        beforehide: function (column) {
                            if (!column.hidden) {
                                column.groupByColumn = false;
                                column.up('gridpanel').getStore().load();
                            }
                        }
                    }
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'Tariff',
                    hideable: false,
                    minWidth: 60,
                    align: 'right',
                    flex: 1,
                    text: 'Тариф',
                    renderer: me.numberRenderer
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'TariffPrev',
                    hideable: false,
                    hidden: true,
                    minWidth: 60,
                    align: 'right',
                    flex: 1,
                    text: 'Тариф пр',
                    renderer: me.numberRenderer
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'Norm',
                    hideable: false,
                    minWidth: 60,
                    align: 'right',
                    flex: 1,
                    text: 'Норматив по услугам',
                    renderer: me.numberRenderer4
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'NormConsumption',
                    hideable: false,
                    minWidth: 60,
                    align: 'right',
                    flex: 1,
                    text: 'Норматив расход',
                    renderer: me.numberRenderer4
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CalculationSign',
                    hideable: false,
                    minWidth: 20,
                    align: 'right',
                    flex: 1,
                    text: 'П*'
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'Consumption',
                    hideable: false,
                    minWidth: 60,
                    align: 'right',
                    flex: 1,
                    text: 'Расход',
                    renderer: me.numberRenderer
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'ConsumptionPrev',
                    hideable: false,
                    hidden: true,
                    minWidth: 60,
                    align: 'right',
                    flex: 1,
                    text: 'Расход пр',
                    renderer: me.numberRenderer
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'FullCalculation',
                    hideable: false,
                    minWidth: 60,
                    align: 'right',
                    flex: 1,
                    text: 'Полный расчет',

                    renderer: function (value, metadata, record, rowIdx, colIdx, st, view) {
                        var cm = record.get('CurMonth');

                        if (cm > 0 && cm < 13 && record.get('ServiceId') > 1)
                            metadata.style = 'text-decoration:underline; text-align: right;';

                        var vl = me.metaRender(value, metadata, record, rowIdx, colIdx, st, view);

                        return Ext.util.Format.number(parseFloat(vl), '0,000.00').replace(',', '.');
                    }
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'FullCalculationPrev',
                    hideable: false,
                    hidden: true,
                    minWidth: 60,
                    align: 'right',
                    flex: 1,
                    text: 'Полный расчет пр',
                    renderer: me.numberRenderer
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'CalculationDaily',
                    hideable: false,
                    minWidth: 60,
                    align: 'right',
                    flex: 1,
                    text: 'Подневной расчет',
                    renderer: me.numberRenderer
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'CalculationDailyPrev',
                    hideable: false,
                    hidden: true,
                    minWidth: 60,
                    align: 'right',
                    flex: 1,
                    text: 'Подневной расчет пр',
                    renderer: me.numberRenderer
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'ShortDelivery',
                    hideable: false,
                    minWidth: 60,
                    align: 'right',
                    flex: 1,
                    text: 'Недоп',
                    renderer: me.numberRenderer
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'ShortDeliveryPrev',
                    hideable: false,
                    hidden: true,
                    minWidth: 60,
                    align: 'right',
                    flex: 1,
                    text: 'Недоп пр',
                    renderer: me.numberRenderer
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'Credited',
                    hideable: false,
                    minWidth: 60,
                    align: 'right',
                    flex: 1,
                    text: 'Начисления',
                    renderer: me.numberRenderer
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'RecalculationPositive',
                    hideable: false,
                    minWidth: 60,
                    align: 'right',
                    flex: 1,
                    text: 'Перерасчет +',
                    renderer: me.numberRenderer
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'RecalculationNegative',
                    hideable: false,
                    minWidth: 60,
                    align: 'right',
                    flex: 1,
                    text: 'Перерасчет -',
                    renderer: me.numberRenderer
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'IncomingSaldo',
                    hideable: false,
                    minWidth: 60,
                    align: 'right',
                    flex: 1,
                    text: 'Входящее сальдо',
                    renderer: me.numberRenderer
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'ChangePositive',
                    hideable: false,
                    minWidth: 60,
                    align: 'right',
                    flex: 1,
                    text: 'Изменение +',
                    renderer: me.numberRenderer
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'ChangeNegative',
                    hideable: false,
                    minWidth: 60,
                    align: 'right',
                    flex: 1,
                    text: 'Изменение -',
                    renderer: me.numberRenderer
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'Paid',
                    hideable: false,
                    minWidth: 60,
                    align: 'right',
                    flex: 1,
                    text: 'Оплата',
                    renderer: me.numberRenderer
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'Payable',
                    hideable: false,
                    minWidth: 60,
                    align: 'right',
                    flex: 1,
                    text: 'Начислено к оплате',
                    renderer: me.numberRenderer
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'OutcomingSaldo',
                    hideable: false,
                    minWidth: 120,
                    align: 'right',
                    flex: 1,
                    text: 'Исходящее сальдо',
                    renderer: me.numberRenderer
                }
            ],

            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            defaults: {
                                margin: '0 2'
                            },
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                }

                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 4,
                            defaults: {
                                margin: '0 2'
                            },
                            items: [
                                {
                                    xtype: 'button',
                                    name: 'calcAccount',
                                    text: 'Расчет',
                                    tooltip: 'Рассчитать лицевой счет',
                                    iconCls: 'icon-calculator'
                                },
                                {
                                    xtype: 'button',
                                    name: 'calcHouse',
                                    text: 'Расчет дома',
                                    tooltip: 'Рассчитать дом',
                                    iconCls: 'icon-calculator'
                                },
                                {
                                    xtype: 'checkbox',
                                    style: "font-size:11px;padding:10px 0px 0 15px",
                                    name: 'showNulls',
                                    hidden: true,
                                    boxLabel: 'Показать нулевые записи',
                                    tooltip: 'Показать нулевые записи'
                                }
                                //,
                                //{
                                //    xtype: 'checkbox',
                                //    name: 'showPrev',
                                //    boxLabel: 'Показать перерасчеты',
                                //    tooltip: 'Показать перерасчеты'
                                //}
                            ]
                        }
                    ]
                },
                {
                    xtype: 'panel',
                    height: 100,
                    dock: 'bottom',
                    frame: true,
                    html: '*П (Признак расчета):' +
                        '<br />1 - норматив' +
                        '<br />2 - показание ПУ' +
                        //'<br />3 - среднемесячное потребление по ПУ' +
                        '<br />3 - исходя из показаний ОДПУ'
                    //'<br />5 - расчетный способ для нежилых помещений'
                }
            ],
            viewConfig: {
                loadMask: true,
                getRowClass: function (record) {

                    var delta = record.get('Delta');

                    if (delta != null) {

                        if (delta[0].Type == 1) //нет в проверочном расчете
                        {
                            return 'font-color1-noimportant';
                        } else {
                            if (delta[0].Type == 2) //нет в исходном расчете
                                return 'font-color2-noimportant';
                        }

                        return '';
                    }

                    var curYear = record.get('CurYear'),
                        curMonth = record.get('CurMonth'),
                        chargeDate = record.get('ChargeDate'),
                        date = null;

                    if (curYear > 0 && curMonth > 0)
                        date = new Date(curYear, curMonth, 1);

                    if (date == null || !chargeDate) return '';


                    if (new Date(chargeDate) < date) {
                        return 'font-tomato-noimportant';
                    } else {
                        return 'font-blue-noimportant';
                    }

                }
            },

            features: [{
                ftype: 'grouping',
                groupHeaderTpl: '{[values.rows[0].data.TopicName]}',
                //startCollapsed: true,
                enableGroupingMenu: false
            }]

        });

        me.callParent(arguments);
    },

    numberRenderer: function (value, metadata, record, rowIdx, colIdx, store, view) {

        var vl = this.metaRender(value, metadata, record, rowIdx, colIdx, store, view);
        return Ext.util.Format.number(parseFloat(vl), '0,000.00').replace(',', '.');
    },

    numberRenderer4: function (value, metadata, record, rowIdx, colIdx, store, view) {

        var vl = this.metaRender(value, metadata, record, rowIdx, colIdx, store, view);
        return Ext.util.Format.number(parseFloat(vl), '0,000.0000').replace(',', '.');
    },

    metaRender: function (value, metadata, record, rowIdx, colIdx, store, view) {

        var vl = value,
            delta = record.get('Delta');

        if (delta == null)
            return value;

        if (delta[0].Type == 1 || delta[0].Type == 2)
            return value;

        var column = view.getHeaderAtIndex(colIdx);
        var dataIndex = column.dataIndex;

        //metadata.style += 'text-color: red; font-weight: bold;';

        Ext.each(delta, function (item) {

            if (item.Field === dataIndex) {

                metadata.style += 'color: red;';
                vl = item.Delta;
                return;
            }
        });

        return vl;
    }
});