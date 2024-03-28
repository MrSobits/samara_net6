Ext.define('B4.view.contractrf.ObjectGridIn', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',

        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',

        'B4.view.Control.GkhDecimalField'
    ],
    alias: 'widget.objectcontractrfgridin',
    itemId: 'objectGridIn',
    store: 'contractrf.ObjectIn',
    title: 'Дома, включенные в договор',
    cls: 'x-large-head',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'MunicipalityName',
                        width: 100,
                        text: 'Муниципальное образование',
                        filter: {
                            xtype: 'b4combobox',
                            operand: CondExpr.operands.eq,
                            storeAutoLoad: false,
                            hideLabel: true,
                            editable: false,
                            valueField: 'Name',
                            emptyItem: { Name: '-' },
                            url: '/Municipality/ListWithoutPaging'
                        }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'RealityObjectName',
                        width: 150,
                        text: 'Жилой дом',
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'GkhCode',
                        width: 100,
                        text: 'Код дома',
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'ManOrgName',
                        text: 'Управляющая организация',
                        width: 100,
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'TotalArea',
                        text: 'Общая площадь жилых и нежилых помещений в доме (кв. м.)',
                        filter: { xtype: 'gkhdecimalfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'AreaLiving',
                        text: 'В т.ч. общая площадь жилых помещений в доме  (кв. м.)',
                        filter: { xtype: 'gkhdecimalfield' },
                        editor: { xtype: 'gkhdecimalfield', minValue: 0 }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'AreaLivingOwned',
                        text: 'В т.ч. общая площадь жилых помещений находящихся в собственности граждан  (кв. м.)',
                        filter: { xtype: 'gkhdecimalfield' },
                        editor: { xtype: 'gkhdecimalfield', minValue: 0 }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'AreaNotLiving',
                        text: 'В т.ч. общая площадь нежилых помещений в доме  (кв. м.)',
                        filter: { xtype: 'gkhdecimalfield' },
                        editor: { xtype: 'gkhdecimalfield', minValue: 0 }
                    },
                    {
                        xtype: 'datecolumn',
                        format: 'd.m.Y',
                        editor: 'datefield',
                        dataIndex: 'IncludeDate',
                        width: 80,
                        text: 'Дата включения в договор',
                        filter: {
                            xtype: 'datefield',
                            operand: CondExpr.operands.eq
                        }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'RealityObjectAreaMkd',
                        width: 60,
                        text: 'Общая площадь МКД',
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'RealityObjectAreaLivingOwned',
                        width: 60,
                        text: 'Площадь в собственности граждан',
                        filter: { xtype: 'textfield' }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'Note',
                        width: 150,
                        text: 'Примечание',
                        filter: { xtype: 'textfield' },
                        editor: {
                            xtype: 'textfield',
                            maxLength: 255
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
                            columns: 4,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'contractRfInSaveButton',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    itemId: 'btnIncludeContractRfObjExport'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: 'contractrf.ObjectIn',
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});