Ext.define('B4.view.dict.tariffbyperiodforclaimwork.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.grid.SelectFieldEditor',
        'B4.form.SelectField'
    ],

    title: 'Тарифы',
    store: 'dict.TariffByPeriodForClaimWork',
    alias: 'widget.tariffByPeriodForClaimWorkGrid',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ChargePeriod',
                    flex: 1,
                    text: 'Период',
                    renderer: function (val) {
                        if (val && val.Name) {
                            return val.Name;
                        }
                    },
                    editor: {
                        xtype: 'b4selectfieldeditor',
                        store: 'B4.store.regop.ChargePeriod',
                        windowCfg: { modal: true },
                        textProperty: 'Name',
                        labelWidth: 175,
                        isGetOnlyIdProperty: false,
                        width: 500,
                        allowBlank: false,
                        editable: false,
                        columns: [
                            {
                                text: 'Наименование',
                                dataIndex: 'Name',
                                flex: 1,
                                filter: { xtype: 'textfield' }
                            },
                            {
                                text: 'Дата открытия',
                                xtype: 'datecolumn',
                                format: 'd.m.Y',
                                dataIndex: 'StartDate',
                                flex: 1,
                                filter: { xtype: 'datefield' }
                            },
                            {
                                text: 'Дата закрытия',
                                xtype: 'datecolumn',
                                format: 'd.m.Y',
                                dataIndex: 'EndDate',
                                flex: 1,
                                filter: { xtype: 'datefield' }
                            },
                            {
                                text: 'Состояние',
                                dataIndex: 'IsClosed',
                                flex: 1,
                                renderer: function (value) {
                                    return value ? 'Закрыт' : 'Открыт';
                                }
                            }
                        ],
                        name: 'ChargePeriod'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Value',
                    flex: 1,
                    text: 'Значение',
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 500
                    },
                   
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
                                    xtype: 'b4savebutton'
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