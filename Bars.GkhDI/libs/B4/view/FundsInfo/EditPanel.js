Ext.define('B4.view.fundsinfo.EditPanel', {
    extend: 'Ext.form.Panel',
    closable: true,
    bodyPadding: 2,
    trackResetOnLoad: true,
    autoScroll: true,
    title: 'Сведения о фондах',
    itemId: 'fundsInfoEditPanel',
    layout: 'border',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.view.fundsinfo.GridPanel',
        'B4.form.FileField',
        'B4.view.Control.GkhDecimalField',
        
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
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'vbox',
                                pack: 'start'
                            },
                            items: [
                                {
                                    xtype: 'combobox', editable: false,
                                    fieldLabel: 'Фонды товарищества/кооператива существуют',
                                    labelStyle: 'font-weight:bold; color: #0440A5; font-size: 11px;',
                                    store: B4.enums.YesNoNotSet.getStore(),
                                    displayField: 'Display',
                                    valueField: 'Value',
                                    name: 'FundsInfo',
                                    itemId: 'cbFundsInfo',
                                    labelWidth: 300,
                                    flex: 1
                                },
                                {
                                    xtype: 'gkhdecimalfield',
                                    fieldLabel: 'Размер обязательных платежей и взносов',
                                    labelStyle: 'font-weight:bold; color: #0440A5; font-size: 11px;',
                                    name: 'SizePayments',
                                    labelWidth: 300,
                                    labelAlign: 'right',
                                    flex: 1
                                },
                                {
                                    xtype: 'b4filefield',
                                    fieldLabel: 'Документ, подтверждающий отсутствие фондов у организации',
                                    labelStyle: 'font-weight:bold; color: #0440A5; font-size: 11px;',
                                    name: 'DocumentWithoutFunds',
                                    itemId: 'ffDocumentWithoutFunds',
                                    labelWidth: 300,
                                    labelAlign: 'right',
                                    flex: 1
                                }
                            ]
                        }
                    ]
                },
                Ext.create('B4.view.fundsinfo.GridPanel', { region: 'center' })
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
                                    itemId: 'addFundsButton'
                                },
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    idProperty: 'helpFundsButton',
                                    text: 'Справка',
                                    tooltip: 'Справка',
                                    iconCls: 'icon-help'
                                },
                                {
                                    xtype: 'b4updatebutton'
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
