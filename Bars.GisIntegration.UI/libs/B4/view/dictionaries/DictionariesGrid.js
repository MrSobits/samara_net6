Ext.define('B4.view.dictionaries.DictionariesGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.dictionariesgrid',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.DictionaryState',
        'B4.enums.DictionaryGroup'
    ],

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.Dictionary');

        me.addEvents('showRecords', 'compareDictionary', 'compareRecords', 'updateStates');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'actioncolumn',
                    width: 20,
                    tooltip: 'Просмотр записей',
                    icon: B4.Url.content('content/img/icons/magnifier.png'),
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var grid = this.up('dictionariesgrid');
                        grid.fireEvent('showRecords', gridView, rowIndex, colIndex, el, e, rec);
                    },
                    sortable: false
                },
                {
                    text: 'Наименование',
                    flex: 1,
                    dataIndex: 'Name',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    text: 'Реестровый номер в ГИС',
                    width: 150,
                    dataIndex: 'GisRegistryNumber',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.DictionaryGroup',
                    text: 'Группа',
                    dataIndex: 'Group',
                    width: 250,
                    filter: true
                },
                {
                    xtype: 'actioncolumn',
                    width: 20,
                    tooltip: 'Связать со справочником ГИС',
                    icon: B4.Url.content('content/img/icons/table_link.png'),
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        if (!el.isDisabled(gridView, rowIndex, colIndex, el, rec)) {
                            var grid = this.up('dictionariesgrid');
                            grid.fireEvent('compareDictionary', gridView, rowIndex, colIndex, el, e, rec);
                        }
                    },
                    isDisabled: function(view, rowIndex, colIndex, item, record) {
                        return !record.get('CompareDictionaryEnabled');
                    },
                    defaultRenderer: me.actionColumnRenderer,
                    sortable: false
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.DictionaryState',
                    dataIndex: 'State',
                    text: 'Статус',
                    width: 150,
                    filter: true
                },
                {
                    xtype: 'actioncolumn',
                    width: 20,
                    tooltip: 'Сопоставить записи',
                    icon: B4.Url.content('content/icon/compare_records.png'),
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        if (!el.isDisabled(gridView, rowIndex, colIndex, el, rec)) {
                            var grid = this.up('dictionariesgrid');
                            grid.fireEvent('compareRecords', gridView, rowIndex, colIndex, el, e, rec);
                        }                               
                    },
                    isDisabled: function (view, rowIndex, colIndex, item, record) {
                        return !record.get('CompareRecordsEnabled');
                    },
                    defaultRenderer: me.actionColumnRenderer,
                    sortable: false
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y H:i:s',
                    text: 'Время последнего сопоставления записей',
                    width: 230,
                    dataIndex: 'LastRecordsCompareDate'
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
                            items: [
                                {
                                    xtype: 'b4updatebutton',
                                    handler: function(button, e) {
                                        button.up('dictionariesgrid').getStore().load();
                                    }
                                },
                                {
                                    xtype: 'button',
                                    icon: B4.Url.content('content/icon/update_state.png'),
                                    text: 'Актуализировать статусы',
                                    tooltip: 'Актуализировать статусы c учетом изменений в ГИС',
                                    handler: function(button, e) {
                                        var grid = button.up('dictionariesgrid');
                                        grid.fireEvent('updateStates', button, e);
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
    },

    actionColumnRenderer: function (v, meta, record, rowIdx, colIdx, store, view) {
        var me = this,
            prefix = Ext.baseCSSPrefix,
            scope = me.origScope || me,
            items = me.items,
            len = items.length,
            i = 0,
            item,
            disabled,
            tooltip;

        // Allow a configured renderer to create initial value (And set the other values in the "metadata" argument!)
        v = Ext.isFunction(me.origRenderer) ? me.origRenderer.apply(scope, arguments) || '' : '';

        meta.tdCls += ' ' + Ext.baseCSSPrefix + 'action-col-cell';
        for (; i < len; i++) {
            item = items[i];

            disabled = item.disabled || (item.isDisabled ? item.isDisabled.call(item.scope || scope, view, rowIdx, colIdx, item, record) : false);
            tooltip = disabled ? null : (item.tooltip || (item.getTip ? item.getTip.apply(item.scope || scope, arguments) : null));

            // Only process the item action setup once.
            if (!item.hasActionConfiguration) {

                // Apply our documented default to all items
                item.stopSelection = me.stopSelection;
                item.disable = Ext.Function.bind(me.disableAction, me, [i], 0);
                item.enable = Ext.Function.bind(me.enableAction, me, [i], 0);
                item.hasActionConfiguration = true;
            }

            v += '<img alt="' + (item.altText || me.altText) + '" src="' + (item.icon || Ext.BLANK_IMAGE_URL) +
                '" class="' + prefix + 'action-col-icon ' + prefix + 'action-col-' + String(i) + ' ' + (disabled ? prefix + 'item-disabled' : ' ') +
                ' ' + (Ext.isFunction(item.getClass) ? item.getClass.apply(item.scope || scope, arguments) : (item.iconCls || me.iconCls || '')) + '"' +
                (tooltip ? ' data-qtip="' + tooltip + '"' : '') + ' />';
        }
        return v;
    }
});