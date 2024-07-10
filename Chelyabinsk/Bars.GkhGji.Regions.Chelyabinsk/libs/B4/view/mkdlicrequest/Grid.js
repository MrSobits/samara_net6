Ext.define('B4.view.mkdlicrequest.Grid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.mkdLicRequestGrid',

    requires: [
        'B4.ux.button.Add',
        'B4.form.GridStateColumn',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'Ext.ux.grid.FilterBar',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.ux.grid.column.Enum',
        'B4.enums.LicStatementResult',
        'B4.enums.DisputeResult',
        'B4.form.SelectField',
        'B4.view.realityobj.Grid',
        'B4.store.RealityObject'
    ],

    store: 'mkdlicrequest.MKDLicRequest',
    itemId: 'mkdLicRequestGrid',
    closable: false,
    cls: 'x-grid-header',
    enableColumnHide: true,

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
                    text: 'Статус заявления',
                    width: 100,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'mkdlicrequest';
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
                    dataIndex: 'StatementNumber',
                    text: 'Номер заявления',
                    flex: 0.5,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ExecutantDocGji',
                    text: 'Статус заявителя',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MKDLicTypeRequest',
                    text: 'Содержание заявления',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'StatementDate',
                    text: 'Дата регистрация заявления',
                    format: 'd.m.Y',
                    flex: 1,
                    hideable: false,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'CheckTime',
                    text: 'Контрольный срок',
                    format: 'd.m.Y',
                    flex: 1,
                    hideable: false,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.LicStatementResult',
                    dataIndex: 'LicStatementResult',
                    text: 'Результат рассмотрения',
                    flex: 1,
                    filter: true,
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Contragent',
                    text: 'Заявитель',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Executant',
                    text: 'Исполнитель',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Inspector',
                    text: 'Инспектор',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObjects',
                    text: 'Адрес МКД',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true,
                getRowClass: function (record) {
                    
                    var state = record.get('State');

                    if (state && state.FinalState) {
                        return 'back-coralgreen';
                    }

                    if (record.get('ExtensTime')) {
                        var extDate = record.get('ExtensTime'),
                            extDateDate = new Date(extDate);
                        var yellowDate = Ext.Date.add(extDateDate, Ext.Date.DAY, -3);
                        var redDate = Ext.Date.add(extDateDate, Ext.Date.DAY, -1);

                        if (redDate <= new Date()) {
                            return 'back-red';
                        }
                        if (yellowDate <= new Date()) {
                            return 'back-yellow';
                        }
                    }

                    if (!record.get('ExtensTime') && record.get('CheckTime')) {
                        var checkDate = record.get('CheckTime'),
                            checkDateDate = new Date(checkDate);
                        var yellowDate = Ext.Date.add(checkDateDate, Ext.Date.DAY, -3);
                        var redDate = Ext.Date.add(checkDateDate, Ext.Date.DAY, -1);

                        if (redDate <= new Date()) {
                            return 'back-red';
                        }
                        if (yellowDate <= new Date()) {
                            return 'back-yellow';
                        }
                    }

                    return '';
                }
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'b4addbutton'
                        },
                        //{
                        //    xtype: 'button',
                        //    iconCls: 'icon-table-go',
                        //    text: 'Экспорт',
                        //    textAlign: 'left',
                        //    itemId: 'btnExport'
                        //},
                        {
                            xtype: 'checkbox',
                            itemId: 'cbShowCloseAppeals',
                            boxLabel: 'Показать закрытые',
                            labelAlign: 'right',
                            checked: false,
                            margin: '10px 10px 0 0'
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