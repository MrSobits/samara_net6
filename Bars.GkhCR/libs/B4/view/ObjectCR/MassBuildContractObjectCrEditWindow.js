Ext.define('B4.view.objectcr.MassBuildContractObjectCrEditWindow', {
    extend: 'B4.form.Window',
    itemId: 'massBuildContractObjectCrEditWindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 650,
    minWidth: 380,
    minHeight: 300,
    maxHeight: 400,
    bodyPadding: 5,
    title: 'Объект КР',
    closeAction: 'destroy',
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.objectcr.MassBuildContractObjectCrWorkGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 100
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'ObjectCrName',
                    fieldLabel: 'Адрес МКД',
                    allowBlank: false,
                    itemId: 'tfObjectCrName',
                    disabled: true,
                    maxLength: 300
                },
                {
                    xtype: 'gkhdecimalfield',
                    name: 'Sum',
                    fieldLabel: 'Сумма работ',
                    itemId: 'dcfSum'
                },
                {
                    xtype: 'tabpanel',
                    items: [
                        {
                            xtype: 'massbuildcontractobjectcrworkgrid'
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
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