Ext.define('B4.view.protocolgji.RequirementEditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.protocolgjirequirementeditwin',

    itemId: 'protocolgjirequirementEditWindow',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 500,
    bodyPadding: 5,

    title: 'Требование',

    requires: [
        'B4.form.ComboBox',
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.Control.GkhButtonPrint',
        'B4.enums.TypeRequirement',
        'B4.store.requirement.Type',
        'B4.store.dict.ArticleLawGji',
        'B4.view.Control.GkhTriggerField'
    ],

    initComponent: function() {
        var me = this,
            requirementStore = Ext.create('B4.store.requirement.Type');

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 120,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4combobox',
                    editable: false,
                    fieldLabel: 'Тип требования',
                    store: requirementStore,
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'TypeRequirement',
                    allowBlank: false,
                    listeners: {
                        storeloaded: {
                            fn: function () {
                                if (this.getValue() == '') {
                                    this.setValue(this.getStore().first().data);
                                }
                            }
                        }
                    }
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        labelWidth: 120,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentNumber',
                            fieldLabel: 'Номер документа'
                        },
                        {
                            xtype: 'datefield',
                            name: 'DocumentDate',
                            fieldLabel: 'Дата документа',
                            format: 'd.m.Y',
                            allowBlank: false
                        }
                    ]
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'ArticleLaw',
                    fieldLabel: 'Статьи закона',
                    textProperty: 'Name',
                    editable: false
                },
                {
                    xtype: 'textfield',
                    name: 'Destination',
                    fieldLabel: 'Адресат'
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'InspectionDate',
                            fieldLabel: 'Дата и время рассмотрения дела',
                            format: 'd.m.Y',
                            labelAlign: 'right',
                            labelWidth: 120,
                            width: 250
                        },
                        {
                            xtype: 'numberfield',
                            name: 'InspectionHour',
                            padding: '5 5 0 20',
                            allowDecimals: false,
                            minValue: 0,
                            maxValue: 23,
                            width: 60
                        },
                        {
                            xtype: 'label',
                            text: 'час',
                            padding: '5 5 0 0'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'InspectionMinute',
                            padding: '5 5 0 20',                               
                            allowDecimals: false,
                            minValue: 0,
                            maxValue: 59,
                            width: 60
                        },
                        {
                            xtype: 'label',
                            text: 'мин',
                            padding: '5 5 0 0',
                        }
                    ]
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'gkhbuttonprint'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnCreateProtocol',
                                    text: 'Сформировать протокол'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    menu: []
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