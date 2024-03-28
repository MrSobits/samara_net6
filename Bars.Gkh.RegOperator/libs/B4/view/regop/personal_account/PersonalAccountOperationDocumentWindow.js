Ext.define('B4.view.regop.personal_account.PersonalAccountOperationDocumentWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.paoperationdocwin',

    requires: [
        'B4.ux.button.Close'
    ],

    modal: true,
    layout: 'form',
    width: 420,
    bodyPadding: 5,
    title: 'Документ операции',
    closeAction: 'hide',
    
    initComponent: function() {
        var me = this;
        Ext.applyIf(me, {
            defaults: {
                xtype: 'textfield',
                readOnly: true,
                labelAlign: 'right',
                labelWidth: 120
            },
            items: [
                /*{
                    name: 'Guid',
                    fieldLabel: 'Номер'
                },*/
                {
                    xtype: 'datefield',
                    name: 'Date',
                    fieldLabel: 'Дата документа'
                },
                {
                    name: 'Period',
                    fieldLabel: 'Расчетный период'
                },
                {
                    name: 'Name',
                    fieldLabel: 'Вид документа'
                },
                {
                    name: 'SaldoChange',
                    fieldLabel: 'Сумма'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        '->',
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4closebutton'
                                }
                            ]
                        }
                    ]
                }
            ]
        });
        me.callParent(arguments);
    }
});