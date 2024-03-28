Ext.define('B4.view.appealcits.SoprInformationGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.appealcitssoprinformationgrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',

        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',

        'B4.form.ComboBox',
    ],

    itemId: 'appealCitsSoprInformationGrid',
    title: 'Сведения об обращениях в СОПР',
    closable: false,
    autoScroll: true,

    store: 'appealcits.SoprInformation',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentName',
                    text: 'Наименование организации',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ReceiptDate',
                    text: 'Дата поступления в СОПР',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    },
                    format: 'd.m.Y',
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ControlPeriod',
                    text: 'Контрольный срок',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    },
                    format: 'd.m.Y',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'State',
                    text: 'Статус обращения',
                    width: 200,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'gji_rapid_response_system_appeal_details';
                            },
                            storeloaded: {
                                fn: function (field) {
                                    field.getStore().insert(0, { Id: null, Name: '-' });
                                }
                            }
                        }
                    },
                    flex: 1,
                    renderer: function (v) {
                        return v.Name ? v.Name : '';
                    }
                },
                {
                    xtype: 'actioncolumn',
                    align: 'center',
                    width: 170,
                    text: 'Открыть в СОПР',
                    icon: B4.Url.content('content/img/icons/arrow_right.png'),
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var me = this,
                            scope = me.origScope;
                        scope.fireEvent('rowaction', scope, 'goToSopr', rec);
                    },
                    scope: me
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
                    store: me.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});