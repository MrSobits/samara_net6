Ext.define('B4.view.cmnestateobj.Grid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.cmnestateobjgrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',

        'B4.store.CommonEstateObject'
    ],

    title: 'Объекты общего имущества',
    closable: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.CommonEstateObject'),
            nameRenderer = function(val) {
                return val && val.Name ? val.Name : '';
            },
            yesNoRenderer = function(val) {
                return val ? 'Да' : 'Нет';
            };

        Ext.applyIf(me, {
            store: store,
            cls:'x-large-head',
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 2,
                    text: 'Наименование'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'GroupType',
                    flex: 1,
                    text: 'Тип группы',
                    renderer: nameRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    flex: 1,
                    text: 'Код'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ReformCode',
                    flex: 1,
                    text: 'Код реформы'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'GisCode',
                    flex: 1,
                    text: 'Код ГИС ЖКХ'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Weight',
                    flex: 1,
                    text: 'Вес'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IncludedInSubjectProgramm',
                    width: 100,
                    text: 'Включен в программу субъекта',
                    renderer: yesNoRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsEngineeringNetwork',
                    width: 100,
                    text: 'Является инженерной сетью',
                    renderer: yesNoRenderer

                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsMatchHc',
                    width: 100,
                    text: 'Соответствует ЖК РФ',
                    renderer: yesNoRenderer

                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsMain',
                    width: 100,
                    text: 'Является основным',
                    renderer: yesNoRenderer

                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    action: 'export'
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
    }
});