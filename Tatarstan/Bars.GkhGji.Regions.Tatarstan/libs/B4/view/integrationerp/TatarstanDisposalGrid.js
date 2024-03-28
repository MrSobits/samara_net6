Ext.define('B4.view.integrationerp.TatarstanDisposalGrid',
    {
        extend: 'B4.ux.grid.Panel',
        alias: 'widget.tatarstandisposalgrid',

        requires: [
            'B4.ux.button.Update',
            'B4.ux.grid.column.Edit',
            'B4.ux.grid.plugin.HeaderFilters',
            'B4.ux.grid.toolbar.Paging',
            'B4.store.integrationerp.TatarstanDisposal'
        ],

        title: 'Интеграция с ЕРП',
        closable: true,

        initComponent: function () {
            var me = this,
                store = Ext.create('B4.store.integrationerp.TatarstanDisposal');

            Ext.applyIf(me, {
                columnLines: true,
                store: store,
                columns: [
                    {
                        xtype: 'b4editcolumn',
                        name: 'showDocument',
                        width: 20,
                        icon: 'content/img/searchfield-icon.png'
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'DocumentNumber',
                        text: 'Номер распоряжения',
                        flex: 1,
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'datecolumn',
                        dataIndex: 'DocumentDate',
                        text: 'Дата распоряжения',
                        format: 'd.m.Y',
                        flex: 1,
                        filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                    },
                    {
                        xtype: 'datecolumn',
                        dataIndex: 'LastMethodStartTime',
                        text: 'Дата последнего запроса',
                        format: 'd.m.Y H:i:s',
                        flex: 1,
                        filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'ErpGuid',
                        text: 'Идентификатор в ЕРП',
                        flex: 2,
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'datecolumn',
                        dataIndex: 'ErpRegistrationDate',
                        text: 'Дата присвоения идентификатора ЕРП',
                        format: 'd.m.Y',
                        flex: 1,
                        filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                    },
                ],
                plugins: [
                    Ext.create('B4.ux.grid.plugin.HeaderFilters')
                ],
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
                                items: [
                                    {
                                        xtype: 'b4updatebutton',
                                        listeners: {
                                            click: function () {
                                                store.load();
                                            }
                                        }
                                    }
                                ]
                            }
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