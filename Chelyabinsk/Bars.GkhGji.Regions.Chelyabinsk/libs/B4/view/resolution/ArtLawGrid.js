Ext.define('B4.view.resolution.ArtLawGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
         'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    alias: 'widget.resolutionArtLawGrid',
    title: 'Статьи закона',
    store: 'resolution.ArtLaw',
    itemId: 'resolutionArtLawGrid',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ArticleLawGji',
                    flex: 1,
                    text: 'Наиманование'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    flex: 1,
                    text: 'Код'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: 'Дата решения',
                    format: 'd.m.Y',
                    editor: {
                        xtype: 'datefield',
                    },
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Description',
                    flex: 1,
                    editor: {
                        xtype: 'textfield',
                        maxLength: 500
                    },
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
                                    itemId: 'resolutionSaveButton'
                                
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