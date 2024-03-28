Ext.define('B4.view.regop.personal_account.action.SetPenalty', {
    extend: 'B4.view.regop.personal_account.action.BaseAccountWindow',

    alias: 'widget.setpenaltywin',

    requires: [
        'B4.form.FileField',
        'B4.view.regop.personal_account.action.BaseAccountWindow'
    ],

    modal: true,

    width: 500,

    title: 'Установка и изменение пени',
    closeAction: 'destroy',

    initComponent: function() {
        var me = this;
        Ext.apply(me, {
            items: [
                {
                    xtype: 'form',
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    defaults: {
                        labelWidth: 200,
                        anchor: '100%'
                    },
                    items: [
                        {
                            xtype: 'hidden',
                            name: 'AccountId'
                        },
                        {
                            xtype: 'numberfield',
                            hideTrigger: true,
                            fieldLabel: 'Текущее пени',
                            readOnly: true,
                            allowDecimals: true,
                            name: 'DebtPenalty'
                        },
                        {
                            xtype: 'numberfield',
                            hideTrigger: true,
                            fieldLabel: 'Новое значение',
                            allowDecimals: true,
                            name: 'NewPenalty',
                            allowBlank: false,
                            decimalSeparator: ','
                        },
                        {
                            xtype: 'b4filefield',
                            name: 'Document',
                            fieldLabel: 'Документ основание'
                        },
                        {
                            xtype: 'textarea',
                            fieldLabel: 'Причина изменения пени',
                            name: 'Reason',
                            allowBlank: false
                        }
                    ]
                }
            ]
        });
        me.callParent(arguments);
    }
});