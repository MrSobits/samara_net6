Ext.define('B4.view.regop.personal_account.action.SetBalance', {
    extend: 'B4.view.regop.personal_account.action.BaseAccountWindow',

    alias: 'widget.setbalancewin',

    requires: [
        'B4.form.FileField',
        'B4.view.regop.personal_account.action.BaseAccountWindow'
    ],

    modal: true,

    width: 500,

    title: 'Изменение основной задолженности',
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
                            xtype: 'textfield',
                            fieldLabel: 'Текущее сальдо',
                            readOnly: true,
                            name: 'CurrentSaldo'
                        },
                        {
                            xtype: 'numberfield',
                            hideTrigger: true,
                            fieldLabel: 'Новое значение',
                            allowDecimals: true,
                            name: 'NewValue',
                            allowBlank: false
                        },
                        {
                            xtype: 'b4filefield',
                            name: 'Document',
                            fieldLabel: 'Документ основание'
                        },
                        {
                            xtype: 'textarea',
                            fieldLabel: 'Причина изменение основной задолженности',
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