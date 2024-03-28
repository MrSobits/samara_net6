Ext.define('B4.view.contragent.AuditPurposeEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.contragentauditpurposeeditwin',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 400,
    height: 150,
    bodyPadding: 5,
    title: 'Редактирование даты проверки по цели',
    closable: false,
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'left'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'AuditPurposeName',
                    fieldLabel: 'Цель',
                    readOnly: true
                },
                {
                    xtype: 'datefield',
                    name: 'LastInspDate',
                    fieldLabel: 'Дата прошлой проверки',
                    format: 'd.m.Y'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
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