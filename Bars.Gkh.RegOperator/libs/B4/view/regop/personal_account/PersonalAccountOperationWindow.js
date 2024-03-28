Ext.define('B4.view.regop.personal_account.PersonalAccountOperationWindow', {
    extend: 'Ext.window.Window',

    alias: 'widget.paoperationwin',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.base.Store'
    ],

    modal: true,
    width: 700,
    height: 350,
    title: 'Операции за период',
    layout: 'fit',
   
    initComponent: function() {
        var me = this,
            store = Ext.create('B4.base.Store', {
                fields: [
                    { name: 'Name' },
                    { name: 'SaldoChange' },
                    { name: 'Period' },
                    { name: 'Name' },
                    { name: 'Document' },
                    { name: 'Date' }
                ],
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'PersonAccountDetalization',
                    listAction: 'ListOperationDetails'
                },
                autoLoad: false
            });

        Ext.applyIf(me, {
            items: [{
                xtype: 'gridpanel',
                border: false,
                header: false,
                store: store,
                plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
                columnLines: true,
                enableColumnHide: false,
                columns: [
                    {
                        xtype: 'datecolumn',
                        dataIndex: 'Date',
                        format: 'd.m.Y',
                        text: 'Дата операции',
                        flex: 1,
                        filter: {
                            xtype: 'datefield',
                            hideTrigger: true,
                            operand: CondExpr.operands.eq
                        }
                    },
                    {
                        text: 'Название операции',
                        dataIndex: 'Name',
                        flex: 2,
                        filter: {
                            xtype: 'textfield',
                            operand: CondExpr.operands.icontains
                        }
                    },
                    {
                        xtype: 'numbercolumn',
                        text: 'Изменение сальдо',
                        dataIndex: 'SaldoChange',
                        format: '0.00',
                        flex: 1,
                        filter: {
                            xtype: 'numberfield',
                            hideTrigger: true,
                            operand: CondExpr.operands.eq
                        }
                    },
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'Document',
                        width: 100,
                        text: 'Документ',
                        renderer: function (v) {
                            return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                        }
                    }
                    //{
                    //    xtype: 'actioncolumn',
                    //    text: 'Документ',
                    //    scope: me,
                    //    width: 60,
                    //    icon: 'content/img/icons/page_copy.png',
                    //    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                    //        gridView.ownerCt.fireEvent('rowaction', gridView.ownerCt, 'edit', rec);
                    //    }
                    //}
                ]
            }],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        '->',
                        {
                            xtype: 'b4closebutton',
                            listeners: {
                                click: function(btn) {
                                    btn.up('window').close();
                                }
                            }
                        }
                    ]
                }
            ]
        });
        me.callParent(arguments);
    }
});