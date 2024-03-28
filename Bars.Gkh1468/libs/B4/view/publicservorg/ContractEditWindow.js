Ext.define('B4.view.publicservorg.ContractEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.publicservorgcontracteditwindow',

    requires: [
        'B4.form.SelectField',
        'B4.form.FileField',
        'B4.view.realityobj.Grid',
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.store.publicservorg.ByPublicServOrg',
        'B4.view.publicservorg.ContractServiceGrid',
        'B4.view.publicservorg.ContractMainInfo',
        'B4.view.publicservorg.RealtyObjectInContractGrid',
        'B4.view.publicservorg.ContractTimingInformation',
        'B4.view.publicservorg.ContractStop',
        'B4.view.publicservorg.contractpart.MainPanel'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    closable: 'hide',
    layout: { type: 'vbox', align: 'stretch' },
    minWidth: 800,
    maxWidth: 800,
    width: 800,
    bodyPadding: 3,
    
    title: 'Договор на поставку услуг',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this,
            panels;

        panels = [
            {
                // Основная информация
                xtype: 'contractmaininfopanel'
            },
            {
                // Дома в договоре
                xtype: 'realtyobjectincontractgrid',
                height: 500
            },
            {
                // Сведения о сроках
                xtype: 'contracttiminginformationpanel'
            },
            {
                // Стороны договора
                xtype: 'contractpartymainpanelpanel'
            },
            {
                // Информация о температурном графике
                xtype: 'publicservorgcontracttepgraphpanel'
            },
            {
                // Расторжение
                xtype: 'contractstoppanel'
            }
        ];

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 160
            },
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton',
                                    name: 'pubServOrg'
                                }
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