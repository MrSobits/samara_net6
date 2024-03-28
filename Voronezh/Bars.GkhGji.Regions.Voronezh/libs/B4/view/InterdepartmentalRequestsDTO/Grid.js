Ext.define('B4.view.InterdepartmentalRequestsDTO.Grid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.interdepartmentalrequestsdtoGrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'B4.enums.RequestState',
        'B4.enums.TypeComplainsRequest',
        'B4.enums.NameOfInterdepartmentalDepartment'
    ],

    title: 'Реестр межведомственных запросов',
    store: 'InterdepartmentalRequestsDTO',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Number',
                    flex: 0.5,
                    text: 'Номер запроса',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Department',
                    flex: 1,
                    text: 'Ведомство',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'Date',
                    flex: 0.5,
                    text: 'Дата запроса',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Inspector',
                    flex: 1,
                    text: 'Инспектор',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.RequestState',
                    dataIndex: 'RequestState',
                    flex: 1,
                    text: 'Статус запроса',
                    filter: true
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.NameOfInterdepartmentalDepartment',
                    dataIndex: 'NameOfInterdepartmentalDepartment',
                    flex: 1,
                    text: 'Вид запроса',
                    filter: true
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true,
                getRowClass: function (record) {
                    var stateCode = record.get('RequestState');
                
                    if (stateCode == B4.enums.RequestState.ResponseReceived) {
                        return 'back-coralgreen';
                    }
                    return '';

                }
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
                                    xtype: 'datefield',
                                    labelWidth: 60,
                                    fieldLabel: 'Дата с',
                                    width: 160,
                                    name: 'DateStart',
                                    itemId: 'dfDateStart',
                                    value: new Date(new Date().getFullYear(), 0, 1)
                                },
                                {
                                    xtype: 'datefield',
                                    labelWidth: 30,
                                    fieldLabel: 'по',
                                    name: 'DateEnd',
                                    width: 130,
                                    itemId: 'dfDateEnd',
                                    value: new Date(new Date().getFullYear(), 11, 31)
                                },
                                {
                                    xtype: 'b4updatebutton'
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