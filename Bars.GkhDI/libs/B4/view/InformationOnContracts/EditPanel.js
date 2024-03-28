Ext.define('B4.view.informationoncontracts.EditPanel', {
    extend: 'Ext.form.Panel',
    closable: true,
    bodyPadding: 2,
    trackResetOnLoad: true,
    autoScroll: true,
    title: 'Сведения о договорах',
    itemId: 'informationOnContractsEditPanel',
    layout: 'border',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.view.informationoncontracts.GridPanel',
        
        'B4.enums.YesNoNotSet'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'form',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyPadding: 2,
                    bodyStyle: Gkh.bodyStyle,
                    layout: {
                        type: 'vbox',
                        pack: 'start',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'combobox', editable: false,
                            fieldLabel: 'Действующие договоры за отчетный период имеются',
                            labelStyle: 'font-weight:bold; color: #0440A5; font-size: 11px;',
                            store: B4.enums.YesNoNotSet.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'ContractsAvailability',
                            itemId: 'cbContractsAvailability',
                            labelWidth: 331,
                            maxWidth: 700,
                            margin: '0 0 8px 0',
                            labelAlign: 'right'
                        },
                        {
                            xtype: 'numberfield',
                            hideTrigger: true,
                            allowDecimals: false,
                            keyNavEnabled: false,
                            mouseWheelEnabled: false,
                            name: 'NumberContracts',
                            fieldLabel: 'Количество заключенных договоров',
                            labelStyle: 'font-weight:bold; color: #0440A5; font-size: 11px;',
                            labelWidth: 331,
                            maxWidth: 700,
                            labelAlign: 'right'
                        }
                    ]
                },
                {
                    xtype: 'infcontractgridpanel',
                    region: 'center'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 4,
                            items: [
                                {
                                    xtype: 'b4addbutton',
                                    itemId: 'addInformationOnContractsButton'
                                },
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'helpButton',
                                    text: 'Справка',
                                    tooltip: 'Справка',
                                    iconCls: 'icon-help'
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
