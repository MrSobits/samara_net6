Ext.define('B4.view.activitytsj.ArticleGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.ComboBox',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.filter.YesNo',
        
        'B4.enums.TypeState'
    ],

    alias: 'widget.activityTsjArticleGrid',
    title: 'Статьи',
    store: 'activitytsj.Article',
    itemId: 'activityTsjArticleGrid',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Group',
                    flex: 2,
                    text: 'Группа',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 2,
                    text: 'Статья',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsNone',
                    width: 100,
                    text: 'Отсутствует',
                    editor: 'checkboxfield',
                    renderer: function (val) {
                        return val ? 'Да' : 'Нет';
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Paragraph',
                    flex: 1,
                    text: 'Пункт устава',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 1000
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    renderer: function (val) { return B4.enums.TypeState.displayRenderer(val); },
                    dataIndex: 'TypeState',
                    flex: 1,
                    text: 'Проверено',
                    editor: {
                        xtype: 'combobox',
                        store: B4.enums.TypeState.getStore(),
                        displayField: 'Display',
                        valueField: 'Value',
                        editable: false
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.TypeState.getItemsWithEmpty([null, '-']),
                        displayField: 'Display',
                        valueField: 'Value',
                        editable: false
                    }
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
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
                                    xtype: 'button',
                                    itemId: 'buttonSave',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить',
                                    tooltip: 'Сохранить'
                                },
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
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});