Ext.define('B4.view.version.Grid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.programversiongrid',

    requires: [
        'B4.ux.button.Update',
        'B4.form.GridStateColumn',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.store.version.ProgramVersion'
    ],

    title: 'Версии ДПКР',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.version.ProgramVersion'),
            nameRenderer = function (val) {
                return val && val.Name ? val.Name : '';
            };

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
                    text: 'Наименование',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'VersionDate',
                    flex: 1,
                    text: 'Дата',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 1900,
                        maxValue: 2200,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsMain',
                    flex: 1,
                    text: 'Основная',
                    renderer: function (v) {
                        return v ? 'Да' : 'Нет';
                    },
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
                    xtype: 'gridcolumn',
                    dataIndex: 'IsProgramPublished',
                    flex: 1,
                    text: 'Программа опубликована',
                    renderer: function (v) {
                        return v ? 'Да' : 'Нет';
                    },
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
                    xtype: 'gridcolumn',
                    dataIndex: 'CopyingState',
                    flex: 1,
                    text: 'Статус копирования'
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
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            flex: 1,
                            padding: '0 0 5 0',
                            items: [
                                {
                                    xtype: 'buttongroup',
                                    maxWidth: 86,
                                    columns: 2,
                                    items: [
                                        {
                                            xtype: 'b4updatebutton'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'component',
                                    width: 10
                                },
                                {
                                    xtype: 'b4combobox',
                                    url: '/Municipality/ListWithoutPaging',
                                    name: 'Municipality',
                                    fieldLabel: 'Муниципальное образование',
                                    labelWidth: 152,
                                    editable: false,
                                    flex: 1
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