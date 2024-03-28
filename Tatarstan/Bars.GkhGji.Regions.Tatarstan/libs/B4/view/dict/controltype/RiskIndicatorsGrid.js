Ext.define('B4.view.dict.ControlType.RiskIndicatorsGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'Ext.grid.plugin.CellEditing',
        'B4.store.dict.ControlTypeRiskIndicators'
    ],

    title: 'Индикаторы риска',
    alias: 'widget.controltyperiskindicatorsgrid',

    initComponent: function (){
        var me = this,
            store = Ext.create('B4.store.dict.ControlTypeRiskIndicators');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    text: 'Наименование',
                    flex: 1,
                    editor: {
                        xtype: 'textfield',
                        maxLength: 5000
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ErvkId',
                    text: 'Идентификатор записи',
                    flex: 1,
                    editor: {
                        xtype: 'textfield',
                        maxLength: 36
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
                    clicksToEdit: 2,
                    pluginId: 'cellEditing'
                })
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    layout: 'vbox',
                    dock: 'top',
                    padding: 5,
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
                                    actionName: 'save',
                                    name: 'RiskIndicatorsSaveButton'
                                },
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    dock: 'bottom',
                    displayInfo: true,
                    store: store
                }
            ]
        });
        me.callParent(arguments);
    }
});