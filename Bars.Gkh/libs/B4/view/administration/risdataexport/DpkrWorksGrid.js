Ext.define('B4.view.administration.risdataexport.DpkrWorksGrid',
    {
        extend: 'B4.ux.grid.Panel',
        requires: [
            'B4.ux.button.Update',
            'B4.ux.grid.toolbar.Paging',
            'B4.ux.grid.plugin.HeaderFilters',
            'B4.ux.grid.column.Enum',
            'B4.enums.FormatDataExportEntityState',
            'B4.enums.EntityType'
        ],

        alias: 'widget.risdataexportdpkrworksgrid',
        title: 'Работы по домам',

        initComponent: function () {
            var me = this,
                store = Ext.create('B4.store.administration.risdataexport.FormatDataExportEntity');

            me.relayEvents(store, ['beforeload'], 'store.');

            store.on('beforeload', function (store, operation) {
                operation.params.EntityType = B4.enums.EntityType.CrProgramHousePlanWork;
            });

            Ext.applyIf(me,
                {
                    columnLines: true,
                    store: store,
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            flex: 1,
                            dataIndex: 'Locality',
                            text: 'Муниципальное образование',
                            filter: {
                                xtype: 'b4combobox',
                                operand: CondExpr.operands.eq,
                                storeAutoLoad: false,
                                hideLabel: true,
                                editable: false,
                                valueField: 'Name',
                                emptyItem: { Name: '-' },
                                url: '/Municipality/ListSettlementWithoutPaging'
                            }
                        },
                        {
                            xtype: 'gridcolumn',
                            flex: 1,
                            dataIndex: 'Address',
                            text: 'Адрес',
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            flex: 1,
                            dataIndex: 'StructElName',
                            text: 'Конструктивный элемент',
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            flex: 1,
                            dataIndex: 'PublishedYear',
                            text: 'Год публикации',
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            flex: 1,
                            dataIndex: 'ErrorMessage',
                            text: 'Сообщение',
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'datecolumn',
                            format: 'd.m.Y H:i:s',
                            flex: 1,
                            dataIndex: 'ExportDate',
                            text: 'Дата передачи',
                            filter: {
                                xtype: 'datefield',
                                format: 'd.m.Y'
                            },
                        },
                        {
                            xtype: 'b4enumcolumn',
                            enumName: 'B4.enums.FormatDataExportEntityState',
                            flex: 1,
                            dataIndex: 'ExportEntityState',
                            text: 'Статус',
                            filter: true
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
                            name: 'buttons',
                            items: [
                                {
                                    xtype: 'buttongroup',
                                    columns: 1,
                                    items: [
                                        {
                                            xtype: 'b4updatebutton'
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