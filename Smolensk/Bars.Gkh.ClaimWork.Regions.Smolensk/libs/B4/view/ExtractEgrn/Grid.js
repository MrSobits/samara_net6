Ext.define('B4.view.ExtractEgrn.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.ux.grid.filter.YesNo',
        'B4.enums.YesNoNotSet',
        'B4.enums.YesNo',
        'B4.ux.grid.column.Enum',
        'Ext.ux.grid.FilterBar'
    ],

    title: 'ЕГРН - Помещения',
    store: 'ExtractEgrn',
    alias: 'widget.extractegrngrid',
    closable: true,
    enableColumnHide: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.ExtractEgrn');
        yesNoStore = Ext.create('Ext.data.Store', {
            fields: ['Display', 'Value'],
            data: [
                { "Display": 'Нет', "Value": false },
                { "Display": 'Да', "Value": true }
            ]
        });
        Ext.applyIf(me, {
            columnLines: true,
            //selModel: Ext.create('Ext.selection.CheckboxModel'),
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'actioncolumn',
                    action: 'getExtract',
                    width: 20,
                    align: 'center',
                    icon: B4.Url.content('content/img/icons/book_go.png'),
                    tooltip: 'Скачать выписку'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Id',
                    flex: 0.3,
                    text: 'Id',
                    hidden: false,
                    filter: { xtype: 'numberfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    text: 'Адрес',
                    flex: 3,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Area',
                    text: 'Площадь',
                    flex: 0.5,
                    filter: { xtype: 'numberfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CadastralNumber',
                    text: 'Кадастровый номер',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ExtractDate',
                    text: 'Дата выписки',
                    format: 'd.m.Y',
                    flex: 1,
                    filter: { xtype: 'datefield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Type',
                    text: 'Тип',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Purpose',
                    text: 'Назначение',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RoomAdr',
                    text: 'Адрес помещения в ИС',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RightsCount',
                    text: 'Кол-во собственников',
                    flex: 1,
                    filter: { xtype: 'numberfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RoomId',
                    flex: 1,
                    text: 'RoomId',
                    hidden: true,
                    filter: { xtype: 'numberfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ExtractId',
                    flex: 1,
                    text: 'ExtractId',
                    hidden: true,
                    filter: { xtype: 'numberfield' }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'IsMerged',
                    text: 'Сопоставлено',
                    enumName: 'B4.enums.YesNoNotSet',
                    flex: 0.5,
                    filter: true
                },           
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                {
                    ptype: 'filterbar',
                    renderHidden: false,
                    showShowHideButton: false,
                    showClearAllButton: false
                }
            ],
            viewConfig:
            {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'b4updatebutton'
                        },
                        {
                            xtype: 'button',
                            iconCls: 'icon-table-go',
                            text: 'Массовое сопоставление',
                            textAlign: 'left',
                            action: 'mergeRooms',
                            tip: 'Сопоставить несопоставленные выписки с помещениями',
                            listeners: {
                                render: function (c) {
                                    Ext.create('Ext.tip.ToolTip', {
                                        target: c.getEl(),
                                        html: c.tip
                                    });
                                }
                            }
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