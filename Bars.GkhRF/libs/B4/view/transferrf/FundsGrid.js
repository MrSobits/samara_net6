Ext.define('B4.view.transferrf.FundsGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',

        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.Panel',
        'B4.model.dict.ProgramCr',
        'B4.store.transferrf.PersonalAccount'
    ],
    alias: 'widget.transferfundsrfgrid',
    title: 'Дома, включенные в заявку',
    itemId: 'transferFundsRfGrid',
    store: 'transferrf.Funds',

    initComponent: function () {
        var me = this;
        var personalAccountStore = Ext.create('B4.store.transferrf.PersonalAccount');
        
        Ext.applyIf(me, {
            columnLines: true,
            features: [{
                ftype: 'summary'
            }],
            columns: [
                {
                    xtype: 'gridcolumn',
                    itemId: 'gcRealityObjectName',
                    dataIndex: 'RealityObjectName',
                    flex: 1,
                    text: 'Объект недвижимости',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    itemId: 'gcPersonalAccount',
                    dataIndex: 'PersonalAccount',
                    flex: 1,
                    text: 'Лицевой счет/Группа финансирования',
                    filter: { xtype: 'textfield' },
                    renderer: function (val) {
                        if (val) {
                            if (typeof val == 'object') {
                                return val.Account;
                            } else {
                                return val;
                            }
                        }
                        return '';
                    },
                    editor: {
                        xtype: 'b4combobox',
                        itemId: 'editorPersonalAccount',
                        store: personalAccountStore,
                        storeAutoLoad: false,
                        valueField: 'Account',
                        displayField: 'Account',
                        lastQuery: '',
                        listConfig: {
                            itemTpl: new Ext.XTemplate('<tpl for="."><div class="x-combo-list-item" > {Account}/{FinGroupDisplay}</div></tpl>')
                        }
                    }
                },
                {
                    xtype: 'gridcolumn',
                    itemId: 'gcPayAllocate',
                    dataIndex: 'PayAllocate',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    },
                    flex: 2,
                    text: 'Назначение платежа',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    itemId: 'gcWorkKind',
                    dataIndex: 'WorkKind',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    },
                    flex: 2,
                    text: 'Разновидность работы',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'numbercolumn',
                    itemId: 'gcSum',
                    dataIndex: 'Sum',
                    text: 'Сумма',
                    editor: 'gkhdecimalfield',
                    flex: 1,
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.String.format('Итого: {0}', Ext.util.Format.currency(value));
                    },
                    filter:
                   {
                       xtype: 'gkhdecimalfield',
                       operand: CondExpr.operands.eq
                   }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me,
                    itemId: 'requestTransferDeleteColumn'
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing',
                    onSpecialKey: function (ed, field, e) {
                        var grid = this.grid, sm;
                        if (e.getKey() === e.TAB) {
                            e.stopEvent();
                            sm = grid.getSelectionModel();
                            if (sm.onEditorTab) sm.onEditorTab(this, e);
                        } else if (e.getKey() === e.ENTER) {
                            e.stopEvent();
                            sm = grid.getSelectionModel();
                            if (sm.onEditorEnter) sm.onEditorEnter(this, e);
                        }
                    }
                })
            ],
            selModel: Ext.create('Ext.selection.RowModel', {
                lastId: null,
                onEditorTab: function (ep, e) {
                    var me = this,
                        view = me.view,
                        record = ep.getActiveRecord(),
                        header = ep.getActiveColumn(),
                        position = view.getPosition(record, header),
                        direction = e.shiftKey ? 'left' : 'right',
                        newPosition = view.walkCells(position, direction, e, false),
                        newId = newPosition.row,
                        grid = view.up('gridpanel');

                    if (me.lastId != newId && me.lastId != null) {
                        var deltaX = me.lastId < newId ? -Infinity : Infinity;
                        header = grid.headerCt.getHeaderAtIndex(newPosition.column);
                        if (header) {
                            while (!header.getEditor()) {
                                newPosition = view.walkCells(newPosition, direction, e, false);
                                header = grid.headerCt.getHeaderAtIndex(newPosition.column);
                            }
                        }
                    } else {
                        deltaX = ep.context.column.width * (direction == 'right' ? 1 : -1);
                    }
                    grid.scrollByDeltaX(deltaX);
                    me.lastId = newPosition.row;
                    Ext.defer(function () {
                        if (newPosition) ep.startEditByPosition(newPosition);
                        else ep.completeEdit();
                    }, 100);
                },
                onEditorEnter: function (ep, e) {
                    var me = this,
                        view = me.view,
                        record = ep.getActiveRecord(),
                        header = ep.getActiveColumn(),
                        position = view.getPosition(record, header),
                        direction = e.shiftKey ? 'up' : 'down',
                        newPosition = view.walkCells(position, direction, e, false),
                        newId = newPosition.row,
                        grid = view.up('gridpanel');
                    
                    //deltaY = 20 * (direction == 'down' ? 1 : -1);
                    //grid.scrollByDeltaY(deltaY);
                    me.lastId = newPosition.row;
                    Ext.defer(function () {
                        if (newPosition) ep.startEditByPosition(newPosition);
                        else ep.completeEdit();
                    }, 100);
                }
            }),
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton',
                                    itemId: 'requestTransferAddButton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'transferFundsRfSaveButton',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить'
                                }
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