Ext.define('B4.view.service.communal.ConsumptionNormsNpaGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.consumptionnormsnpagrid',
    requires: [
        'B4.view.Control.GkhDecimalField',
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.TariffIsSetForDi'
    ],

    itemId: 'consumptionNormsNpaGrid',
    store: 'service.ConsumptionNormsNpa',
    title: 'НПА нормативов потребления',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'NpaDate',
                    width: 125,
                    text: 'Дата',
                    editor: 'datefield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NpaNumber',
                    flex: 1,
                    text: 'Номер нормативного правового акта',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NpaAcceptor',
                    flex: 1,
                    text: 'Наименование принявшего акт органа',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    }
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
                            title: 'Нормативно-правовой акт, устанавливающий норматив потребления коммунальной услуги (заполняется по каждому нормативному правому акту)',
                            width: 800,
                            enableOverflow: true,
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
                                    typeaction: 'consumptionNormsNpaSave'
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