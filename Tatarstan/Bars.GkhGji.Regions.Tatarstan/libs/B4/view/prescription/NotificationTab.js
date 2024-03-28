Ext.define('B4.view.prescription.NotificationTab', {
    extend: 'Ext.container.Container',
    
    requires: [
        'B4.enums.NotificationType',
        'B4.enums.YesNo'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    bodyPadding: 5,
    alias: 'widget.notificationtab',
    title: 'Уведомление',
    trackResetOnLoad: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                xtype: 'container',
                padding: 5,
                layout: { type: 'hbox' },
                labelAlign: 'right',
                labelWidth: 130
            },
            items: [
                {
                    xtype: 'datefield',
                    name: 'Date',
                    fieldLabel: 'Дата',
                    format: 'd.m.Y',
                    maxLength: 10
                },
                {
                    xtype: 'textfield',
                    name: 'DocumentNumber',
                    fieldLabel: 'Номер документа',
                    maxLength: 255
                },
                {
                    xtype: 'datefield',
                    name: 'OutMailDate',
                    fieldLabel: 'Дата исходящего письма',
                    format: 'd.m.Y',
                    maxLength: 10
                },
                {
                    xtype: 'textfield',
                    name: 'OutMailNumber',
                    fieldLabel: 'Номер исходящего письма',
                    maxLength: 255
                },
                {
                    xtype: 'b4enumcombo',
                    name: 'NotificationTransmission',
                    fieldLabel: 'Уведомление передано',
                    enumName: 'B4.enums.YesNo',
                    flex: 1
                },
                {
                    xtype: 'b4enumcombo',
                    name: 'NotificationReceive',
                    fieldLabel: 'Уведомление получено',
                    enumName: 'B4.enums.YesNo',
                    flex: 1
                },
                {
                    xtype: 'b4enumcombo',
                    name: 'NotificationType',
                    fieldLabel: 'Способ уведомления',
                    enumName: 'B4.enums.NotificationType',
                    flex: 1
                },
                {
                    xtype: 'datefield',
                    name: 'ProlongationDate',
                    fieldLabel: 'Срок продления',
                    format: 'd.m.Y',
                    maxLength: 10
                }
            ]
        });

        me.callParent(arguments);
    }
});