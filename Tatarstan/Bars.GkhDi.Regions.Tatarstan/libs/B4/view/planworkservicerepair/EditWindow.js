Ext.define('B4.view.planworkservicerepair.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'border',
    width: 1000,
    height: 600,
    bodyPadding: 5,
    itemId: 'planWorkServiceRepairEditWindow',
    title: 'План работ по содержанию и ремонту',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.view.planworkservicerepair.WorksGrid',
        'B4.view.planworkservicerepair.RepairServicesGrid'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'textfield',
                    region: 'north',
                    anchor: '100%',
                    labelWidth: 190,
                    labelAlign: 'right',
                    name: 'Name',
                    fieldLabel: 'Наименование услуги',
                    readOnly: true
                },
                {
                    xtype: 'panel',
                    padding: '5 0 0 0',
                    title: 'Работы по содержанию и ремонту',
                    region: 'center',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    flex: 1,
                    defaults: {
                        flex: 1,
                        border: false
                    },
                    items: [
                        {
                            xtype: 'planworkservrepairworksgrid'
                        },
                        {
                            xtype: 'planworkservrepairrepairservicesgrid'
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