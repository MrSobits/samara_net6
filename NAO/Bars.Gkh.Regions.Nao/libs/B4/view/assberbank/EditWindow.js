Ext.define('B4.view.assberbank.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    bodyPadding: 10,
    width: 600,
    itemId: 'assberbankEditWindow',
    title: 'Настройки выгрузки в Клиент-Сбербанк',
    closeAction: 'destroy',
    trackResetOnLoad: true,
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {},
            items:
            [
                {
						xtype: 'container',
						layout: 'hbox',
						defaults:
						{
							labelWidth: 80,
							labelAlign: 'right',
						},
						items: [
						{
							xtype: 'textfield',
							name: 'ClientCode',
							fieldLabel: 'Код клиента',
							allowBlank: false,
							flex: 1,
                            maxLength: 5,
						}]
					},
            ],
            dockedItems: [{
                xtype: 'toolbar',
                dock: 'top',
                items: [{
                    xtype: 'buttongroup',
                    columns: 2,
                    items: [{
                        xtype: 'b4savebutton'
                    }]
                },
                {
                    xtype: 'tbfill'
                },
                {
                    xtype: 'buttongroup',
                    columns: 2,
                    items: [{
                        xtype: 'b4closebutton'
                    }]
                }
                ]
            }]
        });

        me.callParent(arguments);
    }
});