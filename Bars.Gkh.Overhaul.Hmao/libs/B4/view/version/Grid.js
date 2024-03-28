Ext.define('B4.view.version.Grid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.programversiongrid',

    requires: [
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.GridStateColumn',
        'B4.form.ComboBox',
        'B4.store.version.ProgramVersion'
    ],

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.version.ProgramVersion');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'actioncolumn',
                    align: 'center',
                    type: 'copy',
                    width: 20,
                    icon: B4.Url.content('content/img/icons/page_copy.png'),
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var me = this;
                        var scope = me.origScope;
                        scope.fireEvent('rowaction', scope, 'copy', rec);
                    },
                    scope: me
                },
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    menuText: 'Статус',
                    width: 175,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        editable: false,
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'ovrhl_program_version';
                            },
                            storeloaded: {
                                fn: function (me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                    me.select(me.getStore().data.items[0]);
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
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'VersionDate',
                    flex: 1,
                    text: 'Дата',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsMain',
                    flex: 1,
                    text: 'Основная',
                    renderer: function (v) {
                        return v ? 'Да' : 'Нет';
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'combobox',
                            name: 'Municipality',
                            padding: '0 0 0 5',
                            store: Ext.create('B4.store.dict.municipality.ByParam', { remoteFilter: false }),
                            fieldLabel: 'Муниципальное образование',
                            labelAlign: 'right',
                            labelWidth: 200,
                            width: 600,
                            valueField: 'Id',
                            displayField: 'Name',
                            listeners: {
                                change: function (cmp, newValue) {
                                    if (cmp.store.isLoading()) {
                                        return;
                                    }

                                    cmp.store.clearFilter();
                                    if (!Ext.isEmpty(newValue)) {
                                        cmp.store.filter({
                                            property: 'Name',
                                            anyMatch: true,
                                            exactMatch: false,
                                            caseSensitive: false,
                                            value: newValue
                                        });
                                    }
                                }
                            }
                        },
                        {
                            xtype: 'checkbox',
                            itemId: 'cbShowNotActualProgramm',
                            boxLabel: 'Показать не основные версии программ',
                            labelWidth: 150
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