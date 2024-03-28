Ext.define('B4.view.planworkservicerepair.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'border',
    width: 900,
    height: 500,
    bodyPadding: 5,
    itemId: 'planWorkServiceRepairEditWindow',
    title: 'План работ по содержанию и ремонту',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.view.planworkservicerepair.WorksGrid'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'anchor',
                    region: 'north',
                    height: 25,
                    defaults: {
                        anchor: '100%',
                        labelWidth: 190,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Наименование услуги',
                            readOnly: true
                        }
                    ]
                },
                {
                    xtype: 'planworkservrepairworksgrid',
                    region: 'center'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'tbfill'
                        },
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