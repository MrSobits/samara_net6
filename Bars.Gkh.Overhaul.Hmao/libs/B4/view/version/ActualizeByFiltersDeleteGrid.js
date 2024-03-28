Ext.define('B4.view.version.ActualizeByFiltersDeleteGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.actualizedbyfilterdeletegrid',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.version.ActualizeByFiltersDelete',
    ],

    //closable: true,
    title: 'Дома на удаление',
    store: 'version.ActualizeByFiltersDelete',
    //layout: 'form',
    //bodyPadding: 5,
    //closable: true,    
    initComponent: function ()
    {
        var me = this;

        Ext.applyIf(me, {
            store: this.store,
            selModel: Ext.create('B4.ux.grid.selection.CheckboxModel', {}),
            columnLines: true,
            columns: 
            [
      
                {
                    xtype: 'actioncolumn',
                    action: 'remove',
                    width: 18,
                    align: 'center',
                    icon: B4.Url.content('content/images/btnRemove.png'),
                    tooltip: 'Исключить',
                },
                {
                    xtype: 'gridcolumn',
                    header: 'Id',
                    width: 100,
                    dataIndex: 'Id',
                    text: 'Id',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    header: 'Адрес',
                    width: 100,
                    dataIndex: 'Address',
                    text: 'Address',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    header: 'ООИ',
                    width: 100,
                    dataIndex: 'CommonEstateObject',
                    text: 'CommonEstateObject',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    header: 'КЭ',
                    width: 100,
                    dataIndex: 'StructuralElement',
                    text: 'StructuralElement',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    header: 'Год',
                    width: 50,
                    dataIndex: 'Year',
                    text: 'Year',
                    filter: {
                        xtype: 'numberfield',
                    },
                }, 
                {
                    xtype: 'gridcolumn',
                    width: 200,
                    header: 'Причина',
                    dataIndex: 'Reasons',
                    text: 'Reasons',
                    filter: {
                        xtype: 'textfield',
                    },
                },
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            features: [
                Ext.create('Ext.grid.feature.Grouping', { groupHeaderTpl: 'Адрес: {name}' })
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
                            xtype: 'button',
                            iconCls: 'icon-arrow-in',
                            text: 'Свернуть все',
                            handler: function () {
                                me.features[0].collapseAll();
                            }
                        },
                        {
                            xtype: 'button',
                            iconCls: 'icon-arrow-out',
                            text: 'Развернуть все',
                            handler: function () {
                                me.features[0].expandAll();
                            }
                        },
                    {
                        xtype: 'button',
                        text: 'Удалить из ДПКР',
                        textAlign: 'left',
                        action: 'Actualize',
                        iconCls: 'icon-accept',
                        tooltip: {
                            //title: 'Заголовок',
                            width: 200,
                            text: 'Удалить отмеченные галочками элементы из программы'
                            }
                    },
                    //{
                    //    xtype: 'button',
                    //    text: 'Свернуть все',
                    //    textAlign: 'left',
                    //    action: 'CollapseAll',
                    //    iconCls: 'icon-collapse-all',
                    //},
                    //{
                    //    xtype: 'button',
                    //    text: 'Развернуть все',
                    //    textAlign: 'left',
                    //    action: 'ExpandAll',
                    //    iconCls: 'icon-collapse-all',
                    //}
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