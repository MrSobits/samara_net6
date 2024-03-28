Ext.define('B4.view.administration.executionaction.actionwithparams.SubRequestPostSendAction', {
    extend: 'Ext.form.Panel',
    requires: [
        'B4.enums.DebtRequestMailNotificationType',
        'B4.form.EnumCombo'
    ],

    border: false,
    layout:{
        type:'fit',
        align:'stretch',
        pack:'start'
    },

    initComponent: function () {

        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4enumcombo',
                    name: 'NotificationType',
                    enumName: 'B4.enums.DebtRequestMailNotificationType',
                    fieldLabel: 'Тип оповещения поставщика ЖКУ',
                    labelWidth: 200,
                    value: B4.enums.DebtRequestMailNotificationType.RequestWithoutResponse,
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});