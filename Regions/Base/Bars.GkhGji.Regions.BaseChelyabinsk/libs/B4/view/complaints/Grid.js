Ext.define('B4.view.complaints.Grid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.complaintsgrid',

    requires: [
        'B4.ux.button.Add',
        'B4.form.GridStateColumn',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'Ext.ux.grid.FilterBar',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.ux.grid.column.Enum',
        'B4.enums.RequesterRole'
    ],

    store: 'complaints.SMEVComplaints',
    title: 'Реестр заявок на досудебное обжалование',
    closable: true,
    enableColumnHide: true,

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
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    width: 100,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'gji_smev_complaints';
                            },
                            storeloaded: {
                                fn: function (me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                }
                            }
                        }
                    },
                    processEvent: function (type, view, cell, recordIndex, cellIndex, e) {
                        if (type == 'click' && e.target.localName == 'img') {
                            var record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    },
                    scope: this
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.RequesterRole',
                    dataIndex: 'RequesterRole',
                    text: 'Тип заявителя',
                    flex: 0.5,
                    filter: true,
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RequesterName',
                    text: 'Заявитель',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AppealNumber',
                    text: 'Номер проверки',
                    flex: 0.5,
                    filter: { xtype: 'textfield' }
                },
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'DocNumber',
                //    text: 'Номер жалобы',
                //    flex: 0.5,
                //    filter: { xtype: 'textfield' }
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ComplaintState',
                    text: 'Статус жалобы',
                    flex: 0.5,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ComplaintId',
                    text: 'Ид жалобы',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ComplaintDate',
                    text: 'Дата жалобы',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
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
                            itemId: 'btnExport'
                        },      
                        {
                            xtype: 'datefield',
                            labelWidth: 60,
                            fieldLabel: 'Период с',
                            width: 160,
                            itemId: 'dfDateStart',
                            value: new Date(new Date().getFullYear(), 0, 1)
                        },
                        {
                            xtype: 'datefield',
                            labelWidth: 30,
                            fieldLabel: 'по',
                            width: 130,
                            itemId: 'dfDateEnd',
                            value: new Date(new Date().getFullYear(), 11, 31)
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