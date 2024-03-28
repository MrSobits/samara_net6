Ext.define('B4.view.admincase.RequirementEditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.admincaserequirementeditwin',
    
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
        'B4.view.Control.GkhTriggerField',
        'B4.enums.TypeRequirement',
        'B4.store.requirement.Type',
        'B4.store.dict.ArticleLawGji'
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