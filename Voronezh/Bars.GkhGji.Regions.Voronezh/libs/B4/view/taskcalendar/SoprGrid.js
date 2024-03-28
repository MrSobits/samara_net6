/*
* Грид обращений граждан. При наличия модуля Интеграция с ЭДО перекрывается этим модулем
*/
Ext.define('B4.view.taskcalendar.SoprGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.taskcalendarsoprgrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Enum',
        'B4.enums.YesNoNotSet',
        'B4.form.GridStateColumn',
        'B4.ux.grid.toolbar.Paging'
    ],

    store: 'taskcalendar.ListSOPR',
    closable: false,
    enableColumnHide: true,
    title: 'СОПР',
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
                    width: 100,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'gji_appeal_citizens';
                            },
                            storeloaded: {
                                fn: function (me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                }
                            }
                        }
                    },
                    scope: this
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Executant',
                    flex: 1,
                    text: 'Контрагент',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentINN',
                    flex: 1,
                    text: 'ИНН',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    width: 80,
                    text: 'Номер обращения',
                    hideable: false,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateFrom',
                    text: 'Дата регистрации обращения',
                    format: 'd.m.Y',
                    width: 100,
                    flex: 1,
                    hideable: false,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq },
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'OrderDate',
                    flex: 0.5,
                    text: 'Дата размещения в СОПР',
                    format: 'd.m.Y',
                    renderer: function (v) {
                        if (Date.parse(v, 'd.m.Y') == Date.parse('01.01.0001', 'd.m.Y') || Date.parse(v, 'd.m.Y') == Date.parse('01.01.3000', 'd.m.Y')) {
                            v = undefined;
                        }
                        return Ext.util.Format.date(v, 'd.m.Y');
                    },
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'PerformanceDate',
                    flex: 0.5,
                    text: 'Дата исполнения',
                    format: 'd.m.Y',
                    renderer: function (v) {
                        if (Date.parse(v, 'd.m.Y') == Date.parse('01.01.0001', 'd.m.Y') || Date.parse(v, 'd.m.Y') == Date.parse('01.01.3000', 'd.m.Y')) {
                            v = undefined;
                        }
                        return Ext.util.Format.date(v, 'd.m.Y');
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.YesNoNotSet',
                    dataIndex: 'YesNoNotSet',
                    text: 'Отработано',
                    flex: 1,
                    filter: true,
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.YesNoNotSet',
                    dataIndex: 'Confirmed',
                    text: 'Принято инспектором',
                    flex: 1,
                    filter: true,
                }
            ],
            plugins: [{ ptype: 'filterbar'}],
            viewConfig: {
                loadMask: true,
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'b4updatebutton'
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