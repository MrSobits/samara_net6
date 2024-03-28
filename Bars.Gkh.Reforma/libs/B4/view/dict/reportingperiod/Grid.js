Ext.define('B4.view.dict.reportingperiod.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.dict.ReportingPeriodState',
        'B4.ux.grid.column.Enum',
        'B4.model.dict.PeriodDi',
        'B4.form.ComboBox',
        'B4.grid.SelectFieldEditor',
        'Ext.ux.CheckColumn'
    ],

    title: 'Отчетные периоды',
    store: 'dict.ReportingPeriod',
    alias: 'widget.reportingPeriodGrid',
    closable: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    text: 'Название',
                    flex: 1,
                    dataIndex: 'Name',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    flex: 1,
                    text: 'Дата начала',
                    dataIndex: 'DateStart',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', format: 'd.m.Y' }
                },
                {
                    xtype: 'datecolumn',
                    flex: 1,
                    text: 'Дата окончания',
                    dataIndex: 'DateEnd',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', format: 'd.m.Y' }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.dict.ReportingPeriodState',
                    flex: 1,
                    text: 'Статус',
                    dataIndex: 'State',
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PeriodDi',
                    flex: 1,
                    text: 'Период раскрытия',
                    renderer: function(val) {
                        if (val && val.Name) {
                            return val.Name;
                        }
                    },
                    editor: {
                        xtype: 'b4selectfieldeditor',
                        store: 'B4.store.dict.PeriodDi',
                        name: 'PeriodDi',
                        modalWindow: true,
                        isGetOnlyIdProperty: false,
                        columns: [
                            {
                                text: 'Название',
                                flex: 1,
                                dataIndex: 'Name',
                                filter: { xtype: 'textfield' }
                            },
                            {
                                xtype: 'datecolumn',
                                flex: 1,
                                text: 'Дата начала',
                                dataIndex: 'DateStart',
                                format: 'd.m.Y',
                                filter: { xtype: 'datefield', format: 'd.m.Y' }
                            },
                            {
                                xtype: 'datecolumn',
                                flex: 1,
                                text: 'Дата окончания',
                                dataIndex: 'DateEnd',
                                format: 'd.m.Y',
                                filter: { xtype: 'datefield', format: 'd.m.Y' }
                            }
                        ]
                    },
                    filter: {
                        xtype: 'textfield',
                        filterName: 'PeriodDi.Name'
                    }
                },
                {
                    xtype: 'checkcolumn',
                    text: 'Синхронизация',
                    dataIndex: 'Synchronizing',
                    width: 100,
                    name: 'Synchronizing'
                },
                {
                    xtype: 'checkcolumn',
                    text: 'Относится к новым формам',
                    dataIndex: 'Is_988',
                    width: 160,
                    name: 'Is_988',
                    disabled: true
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