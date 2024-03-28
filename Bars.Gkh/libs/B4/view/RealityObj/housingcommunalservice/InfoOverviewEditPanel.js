Ext.define('B4.view.realityobj.housingcommunalservice.InfoOverviewEditPanel', {
    extend: 'Ext.form.Panel',
    
    alias: 'widget.hseinfoovervieweditpanel',

    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 800,
    minWidth: 800,
    bodyPadding: 5,
    bodyStyle: Gkh.bodyStyle,
    title: 'Сведения по дому: общие сведения',
    trackResetOnLoad: true,
    autoScroll: true,
    requires: [
        'B4.ux.button.Save'
    ],

    initComponent: function() {
        var me = this;

        me.initialConfig = Ext.apply({
            trackResetOnLoad: true
        }, me.initialConfig);

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    title: 'Количество лицевых счетов физических лиц',
                    layout: 'hbox',
                    defaults: {
                        labelAlign: 'right',
                        flex: 1,
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        allowDecimals: false,
                        minValue: 0,
                        labelWidth: 200
                    },
                    items: [
                        {
                            name: 'IndividualAccountsCount',
                            fieldLabel: 'Общее',
                            labelWidth: 50,
                            flex: 0.5
                        },
                        {
                            name: 'IndividualTenantAccountsCount',
                            fieldLabel: 'физических лиц-собственников'
                        },
                        {
                            name: 'IndividualOwnerAccountsCount',
                            fieldLabel: 'физических лиц-нанимателей'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Количество лицевых счетов юридических лиц',
                    layout: 'hbox',
                    defaults: {
                        labelAlign: 'right',
                        flex: 1,
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        allowDecimals: false,
                        minValue: 0,
                        labelWidth: 200
                    },
                    items: [
                        {
                            name: 'LegalAccountsCount',
                            fieldLabel: 'Общее',
                            labelWidth: 50,
                            flex: 0.5
                        },
                        {
                            name: 'LegalTenantAccountsCount',
                            fieldLabel: 'физических лиц-собственников'
                        },
                        {
                            name: 'LegalOwnerAccountsCount',
                            fieldLabel: 'физических лиц-нанимателей'
                        }
                    ]
                },
                {
                    xtype: 'hseoverallbalancegrid',
                    flex: 1
                }
            ],
            dockedItems: [{
                xtype: 'toolbar',
                dock: 'top',
                items: [
                    {
                        xtype: 'buttongroup',
                        items: [{ xtype: 'b4savebutton' }]
                    }
                ]
            }]
        });

        me.callParent(arguments);
    }
});