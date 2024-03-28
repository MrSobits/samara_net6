Ext.define('B4.view.baseprosclaim.MainInfoTabPanel', {
    extend: 'Ext.form.Panel',
    layout: { type: 'vbox', align: 'stretch' },
    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.form.SelectField',
        'B4.store.ManagingOrganization',
        'B4.form.FileField',
        'B4.form.ComboBox',
        
        'B4.enums.TypeBaseProsClaim',
        'B4.enums.TypeFormInspection'
    ],
    alias: 'widget.baseprosclaimmaininfotabpanel',
    itemId: 'mainInfoTabPanel',
    title: 'Основная информация',
    bodyStyle: Gkh.bodyStyle,
    bodyPadding: 5,
    border: false,
    defaults: {
        labelWidth: 170,
        labelAlign: 'right'
    },
    
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            items: [
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    defaults: {
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
                    },
                    layout: {
                        pack: 'start',
                        type: 'hbox'
                    },
                    items: [
                        {
                            xtype: 'combobox',
                            name: 'TypeBaseProsClaim',
                            itemId: 'cbTypeBaseProsClaim',
                            fieldLabel: 'Тип требования',
                            displayField: 'Display',
                            store: B4.enums.TypeBaseProsClaim.getStore(),
                            valueField: 'Value',
                            editable: false
                        },
                        {
                            xtype: 'datefield',
                            itemId: 'dfProsClaimDateCheck',
                            name: 'ProsClaimDateCheck',
                            fieldLabel: 'Дата проверки',
                            format: 'd.m.Y',
                            allowBlank: false
                        }
                    ]
                },
                {
                    xtype: 'textfield',
                    name: 'IssuedClaim',
                    itemId: 'tfIssuedClaim',
                    fieldLabel: 'ДЛ, вынесшее требование',
                    maxLength: 300
                },
                {
                    xtype: 'combobox', editable: false,
                    name: 'TypeForm',
                    itemId: 'cbTypeForm',
                    fieldLabel: 'Форма проверки',
                    displayField: 'Display',
                    store: B4.enums.TypeFormInspection.getStore(),
                    valueField: 'Value',
                    editable: false
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Inspectors',
                    itemId: 'prosClaimInspectorsTrigerField',
                    fieldLabel: 'Инспекторы',
                    editable: false
                },
                {
                    xtype: 'fieldset',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    title: 'Документ',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 100
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentName',
                            itemId: 'tfDocumentName',
                            fieldLabel: 'Наименование',
                            maxLength: 300
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: {
                                pack: 'start',
                                type: 'hbox'
                            },
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 100,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNumber',
                                    itemId: 'tfDocumentNumber',
                                    fieldLabel: 'Номер',
                                    allowBlank: false,
                                    maxLength: 50
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentDate',
                                    itemId: 'dfDocumentDate',
                                    fieldLabel: 'Дата',
                                    format: 'd.m.Y'
                                }
                            ]
                        },
                        {
                            xtype: 'textarea',
                            height: 50,
                            name: 'DocumentDescription',
                            itemId: 'taDocumentDescription',
                            fieldLabel: 'Описание',
                            maxLength: 500
                        },
                        {
                            xtype: 'b4filefield',
                            name: 'File',
                            fieldLabel: 'Файл',
                            editable: false
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});