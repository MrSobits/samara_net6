Ext.define('B4.view.passport.oki.info.Grid', {    
    alias: 'widget.infookipassportgrid',

    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
    
        'B4.form.GridStateColumn',
        'B4.form.ComboBox',
        
        'B4.store.PeriodYear',
        'B4.store.passport.OkiProviderPassport'
    ],

    title: 'Паспорта по поставщикам',
    year: null,
    month: null,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.passport.OkiProviderPassport'),
            cbMonth = Ext.create('Ext.form.ComboBox', {
                index: 'Month',
                queryMode: 'local',
                valueField: 'num',
                displayField: 'name',
                store: Ext.create('Ext.data.Store', {
                    fields: ['num', 'name'],
                    data: [
                        { "num": 1, "name": "Январь" },
                        { "num": 2, "name": "Февраль" },
                        { "num": 3, "name": "Март" },
                        { "num": 4, "name": "Апрель" },
                        { "num": 5, "name": "Май" },
                        { "num": 6, "name": "Июнь" },
                        { "num": 7, "name": "Июль" },
                        { "num": 8, "name": "Август" },
                        { "num": 9, "name": "Сентябрь" },
                        { "num": 10, "name": "Октябрь" },
                        { "num": 11, "name": "Ноябрь" },
                        { "num": 12, "name": "Декабрь" }
                    ]
                })
            }),
            cbYear = Ext.create('Ext.form.ComboBox', {
                fieldLabel: 'Отчетный период',
                index: 'Year',
                queryMode: 'local',
                valueField: 'num',
                displayField: 'name',
                store: Ext.create('B4.store.PeriodYear')
            });
       
        cbMonth.setValue(me.month);
        cbYear.setValue(me.year);

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    menuText: 'Статус',
                    text: 'Статус',
                    width: 200,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'okiproviderpassport';
                            },
                            storeloaded: {
                                fn: function (field) {
                                    field.getStore().insert(0, { Id: null, Name: '-' });
                                }
                            }
                        }
                    },
                    processEvent: function (type, view, cell, recordIndex, cellIndex, e) {
                        if (type == 'click' && e.target.localName == 'img') {
                            var record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    },
                    scope: this
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'ObjectCreateDate',
                    width: 150,
                    text: 'Дата смены статуса',
                    filter: {
                        xtype: 'datefield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
                    text: 'Муниципальное образование',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Contragent',
                    width: 250,
                    text: 'Ответственный',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Signature',
                    flex: 1,
                    text: 'Сведения о подписанте',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Percent',
                    align: 'center',
                    text: '%',
                    tdCls: 'x-progress-cell',
                    renderer: function (value) {
                        return Math.round(value) + '%';
                    },
                    width: 100
                },
                {
                    xtype: 'actioncolumn',
                    text: 'Переход',
                    action: 'openpassport',
                    width: 174,
                    items: [{
                        tooltip: 'Перейти к паспорту',
                        icon: 'content/img/onepasptransfer.png'
                    }]
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true,
                getRowClass: function (record, index) {
                    var c = parseFloat(record.get('Percent'));
                    if (c == isNaN) {
                        return;
                    }

                    if (c == 100) {
                        return 'x-percent-100';
                    }

                    if (c <= 10) {
                        return 'x-percent-10';
                    } else if (c > 10 && c <= 20) {
                        return 'x-percent-20';
                    } else if (c > 20 && c <= 40) {
                        return 'x-percent-30';
                    } else if (c > 40 && c <= 70) {
                        return 'x-percent-70';
                    } else if (c > 70 && c <= 99) {
                        return 'x-percent-90';
                    };
                    return;
                }
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        }
                        //'->',
                        //cbYear,
                        //cbMonth
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});