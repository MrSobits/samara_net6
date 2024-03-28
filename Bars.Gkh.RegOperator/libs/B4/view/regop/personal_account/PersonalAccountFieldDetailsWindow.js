Ext.define('B4.view.regop.personal_account.PersonalAccountFieldDetailsWindow', {
    extend: 'Ext.window.Window',

    alias: 'widget.personalaccountfielddetailswindow',

    requires: [
        'B4.store.regop.personal_account.PersonalAccountFieldDetails'
    ],

    modal: true,
    closable: false,
    width: 700,
    height: 380,
    title: 'Детализация',
    layout: 'fit',
    closeAction: 'destroy',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.regop.personal_account.PersonalAccountFieldDetails');
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'gridpanel',
                    border: false,
                    header: false,
                    store: store,
                    columnLines: true,
                    enableColumnHide: false,
                    columns: [
                        {
                            text: 'Период',
                            dataIndex: 'Period',
                            flex: 1
                        },
                        {
                            xtype: 'numbercolumn',
                            text: 'Сумма',
                            dataIndex: 'Amount',
                            format: '0.00',
                            flex: 1
                        }
                    ]
                }
            ],
            dockedItems: [
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