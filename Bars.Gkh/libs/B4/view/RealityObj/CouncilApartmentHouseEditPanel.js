Ext.define('B4.view.realityobj.CouncilApartmentHouseEditPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.councilapartmenthouseeditpanel',
    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 800,
    minWidth: 800,
    bodyPadding: 5,
    bodyStyle: Gkh.bodyStyle,
    
    title: 'Совет МКД',
    trackResetOnLoad: true,
    autoScroll: true,
    requires: [
        'B4.form.FileField',
        'B4.view.realityobj.CouncillorsGrid',
        'B4.ux.button.Save',
        'B4.enums.CouncilResult'
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
                    itemId: 'fsCouncilProtocol',
                    title: 'Протокол собрания жильцов',
                    items: [
                        {
                            xtype: 'combobox',
                            name: 'CouncilResult',
                            labelWidth: 120,
                            width: 500,
                            fieldLabel: 'Результат собрания',
                            editable: false,
                            displayField: 'Display',
                            store: B4.enums.CouncilResult.getStore(),
                            valueField: 'Value',
                            itemId: 'cbCouncilResult'
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNum',
                                    padding: '0 0 5 0',
                                    labelWidth: 120,
                                    width: 350,
                                    fieldLabel: 'Номер протокола',
                                    maxLength: 50,
                                    itemId: 'documentNum'
                                },
                                {
                                    xtype: 'datefield',
                                    padding: '0 0 5 0',
                                    labelWidth: 50,
                                    width: 150,
                                    name: 'DateFrom',
                                    fieldLabel: 'От',
                                    format: 'd.m.Y',
                                    itemId: 'dateFrom'
                                }
                            ]
                        },
                        {
                            xtype: 'b4filefield',
                            name: 'File',
                            width: 500,
                            labelWidth: 120,
                            fieldLabel: 'Файл',
                            labelAlign: 'right',
                            itemId: 'file'
                        }
                    ]
                },
                {
                    xtype: 'realobjcouncillorsgrid',
                    flex: 1
                }
            ],
            dockedItems: [{
                xtype: 'toolbar',
                dock: 'top',
                items: [
                    {
                        xtype: 'buttongroup',
                        columns: 2,
                        itemId: 'councillorsGroupButton',
                        items: [{ xtype: 'b4savebutton' }]
                    },
                    {
                        xtype: 'component',
                        itemId: 'councillorsLabel',
                        padding: 2,
                        style: 'font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 3px; line-height: 16px;',
                        html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Данный раздел не предназначен для заполнения при текущем способе управления</span>'
                    },
                    {
                        xtype: 'component',
                        itemId: 'emptyResultLabel',
                        padding: 2,
                        hidden: true,
                        style: 'font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 1px; line-height: 16px;',
                        html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Перед внесением информации укажите результат собрания</span>'
                    }
                ]
            }]
        });

        me.callParent(arguments);
    }
});