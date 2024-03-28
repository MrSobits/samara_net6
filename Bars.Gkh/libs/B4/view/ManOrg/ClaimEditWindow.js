Ext.define('B4.view.manorg.ClaimEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 500,
    bodyPadding: 5,
    itemId: 'manorgClaimEditWindow',
    title: 'Претензия',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'datefield',
                    name: 'DateClaim',
                    fieldLabel: 'Дата претензии',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    name: 'AmountClaim',
                    fieldLabel: 'Сумма',
                    anchor: '100%'
                },
                {
                    xtype: 'textarea',
                    name: 'ContentClaim',
                    fieldLabel: 'Содержание',
                    anchor: '100%',
                    maxLength: 500
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
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});