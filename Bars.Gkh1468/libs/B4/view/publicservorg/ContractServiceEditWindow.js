Ext.define('B4.view.publicservorg.ContractServiceEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.publicservorgcontractserviceeditwindow',
    mixins: ['B4.mixins.window.ModalMask'],

    width: 700,
    height: 500,
    bodyPadding: 5,
    title: 'Предоставляемая услуга',
    trackResetOnLoad: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.view.publicservorg.ContractServiceMainInfo',
        'B4.view.publicservorg.contractqualitylevel.Panel',
        'B4.view.publicservorg.ContractServicePlanVolume'
    ],

    initComponent: function() {
        var me = this,
            panels;

        panels = [
            {
                xtype: 'contractservicemaininfopanel'
            },
            {
                xtype: 'contractservicecontractqualitylevelpanel'
            },
            {
                xtype: 'contractserviceplanvolumepanel'
            }
        ];

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    border: false,
                    activeTab: 0,
                    items: panels
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [{ xtype: 'b4savebutton' }]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [{ xtype: 'b4closebutton' }]
                        }
                    ]
                }
            ]
        });
        me.callParent(arguments);
    }
});