Ext.define('B4.view.terminatecontract.EditPanel', {
    extend: 'Ext.form.Panel',
    closable: true,
    bodyPadding: 2,
    trackResetOnLoad: true,
    autoScroll: true,
    title: 'Сведения о расторгнутых договорах',
    itemId: 'terminateContractEditPanel',
    layout: 'border',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.button.Save',
        'B4.view.terminatecontract.GridPanel',
        
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
                    padding: '3px 0 0 0',
                    bodyPadding: 2,
                    bodyStyle: Gkh.bodyStyle,
                    items: [
                        {
                            xtype: 'combobox', editable: false,
                            fieldLabel: 'Случаи расторжения договоров управления в предыдущем календарном году были',
                            labelStyle: 'font-weight:bold; color: #0440A5; font-size: 11px;',
                            store: B4.enums.YesNoNotSet.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'TerminateContract',
                            itemId: 'cbTerminateContract',
                            labelWidth: 470
                        }
                    ]
                },
                {
                    xtype: 'terminatecontractgridpanel',
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    idProperty: 'helpButton',
                                    text: 'Справка',
                                    tooltip: 'Справка',
                                    iconCls: 'icon-help'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'button',
                                    itemId: 'realityObjButton',
                                    text: 'Управление домами',
                                    tooltip: 'Управление домами',
                                    iconCls: 'icon-pencil-go'
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
