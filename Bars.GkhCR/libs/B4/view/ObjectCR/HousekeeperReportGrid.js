Ext.define('B4.view.objectcr.HousekeeperReportGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.housekeeperreportgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.form.GridStateColumn'
    ],

    title: 'Отчет старшего по дому',
    closable: true,
    store: 'objectcr.HousekeeperReport',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    width: 160,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'housekeeper_report';
                            },
                            storeloaded: {
                                fn: function (me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
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
                    xtype: 'gridcolumn',
                    dataIndex: 'ReportNumber',
                    flex: 0.4,
                    text: 'Номер отчета'
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'ReportDate',
                    flex: 0.5,
                    text: 'Дата отчета'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FIO',
                    flex: 1,
                    text: 'Старший по дому'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PhoneNumber',
                    flex: 1,
                    text: 'Телефон'
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'CheckDate',
                    flex: 0.5,
                    text: 'Дата проверки'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CheckTime',
                    flex: 0.5,
                    text: 'Время проверки'
                },
                {
                    xtype: 'booleancolumn',
                    falseText: '',
                    trueText: 'Да',
                    dataIndex: 'IsArranged',
                    flex: 1,
                    text: 'Замечания устранены',
                    filter:
                    {
                        xtype: 'b4combobox',
                        items: [[null, '-'], [false, 'Нет'], [true, 'Да']],
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
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
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});