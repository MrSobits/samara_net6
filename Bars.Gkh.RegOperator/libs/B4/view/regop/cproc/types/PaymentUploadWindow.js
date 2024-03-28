Ext.define('B4.view.regop.cproc.types.PaymentUploadWindow', {
    extend: 'B4.view.regop.cproc.types.ComPrBaseWindow',

    alias: 'widget.comprpaymentuploadwindow',

    title: 'Загрузка оплат',


    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [{
                xtype: 'grid',
                border: false,
                columns: [
                    {
                        text: 'Личевой счет',
                        dataIndex: '',
                        flex: 1
                    },
                    {
                        xtype: 'datecolumn',
                        format: 'd.m.Y',
                        text: 'Дата оплаты',
                        dataIndex: '',
                        flex: 1
                    },
                    {
                        xtype: 'numbercolumn',
                        text: 'Уплачено взносов',
                        dataIndex: '',
                        flex: 1
                    },
                    {
                        xtype: 'numbercolumn',
                        text: 'Уплачено пени',
                        dataIndex: '',
                        flex: 1
                    }
                ]
            }]
        });
        me.callParent(arguments);
    }
});