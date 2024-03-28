Ext.define('B4.view.protocolgji.ArticleLawGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    alias: 'widget.protocolgjiArticleLawGrid',
    title: 'Статьи закона',
    store: 'protocolgji.ArticleLaw',
    itemId: 'protocolgjiArticleLawGrid',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ArticleLaw',
                    flex: 1,
                    text: 'Статья закона'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Description',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 500
                    },
                    flex: 1,
                    text: 'Описание'
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Сохранить',
                                    tooltip: 'Сохранить',
                                    iconCls: 'icon-accept',
                                    itemId: 'protocolSaveButton'
                                }
                                
                            ]
                        },
                        {
                            xtype: 'textfield',
                            name: 'PersonFollowConversion',
                            fieldLabel: 'Лицо, выполнившее перепланировку/переустройство',
                            width: 500,
                            labelWidth: 290,
                            margin: '0 0 0 20'
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