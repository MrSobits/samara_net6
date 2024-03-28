Ext.define('B4.view.regop.personal_account.action.SetSignsWin', {
    extend: 'B4.view.regop.personal_account.action.BaseAccountWindow',

    alias: 'widget.setsignswin',

    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
    ],

    modal: true,
    closable: false,
    width: 320,
    height: 160,
    title: 'Установить параметры по ЛС',
    closeAction: 'destroy',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    
    accountOperationCode: 'SetSignOperation',
    accIds: null,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'form',
                    bodyStyle: Gkh.bodyStyle,
                    border: 0,
                    layout: {
                        type: 'vbox',
                        align: 'stretch',
                    },
                    items: [
                        {
                            xtype: 'numberfield',
                            hideTrigger: true,
                            labelWidth: 200,
                            fieldLabel: 'Колличество изменяемых ЛС',
                            name: 'CountLS',
                            readOnly: true
                        },
                        {
                            xtype: 'checkbox',
                            boxLabel: 'Не считать должником',
                            fieldStyle: 'vertical-align: middle;',
                            style: 'font-size: 11px !important;',
                            margin: '-2 0 0 10',
                            width: 130,
                            name: 'IsNotDebtor'
                        },
                        {
                            xtype: 'checkbox',
                            boxLabel: 'Заключен договор рассрочки',
                            fieldStyle: 'vertical-align: middle;',
                            style: 'font-size: 11px !important;',
                            margin: '-2 0 0 10',
                            width: 130,
                            name: 'InstallmentPlan'
                        }
                    ]
                }                
            ]
        });

        me.callParent(arguments);
    }
});