Ext.define('B4.view.emergencyobj.DocumentsPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: 'anchor',
    width: 500,
    bodyPadding: 5,
    itemId: 'emergencyObjDocumentsPanel',
    title: 'Документы',
    trackResetOnLoad: true,
    autoScroll: true,
    frame: true,
    requires: [
        'B4.form.FileField',
        'B4.ux.button.Save'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                anchor: '100%',
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 250,
                        labelAlign: 'right',
                        width: 400
                    },
                    title: 'Требование о сносе/реконструкции аварийного МКД',
                    items: [
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Дата издания',
                            name: 'RequirementPublicationDate',
                            format: 'd.m.Y',
                            allowBlank: false
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Номер документа',
                            name: 'RequirementDocumentNumber',
                            width: 600
                        },
                        {
                            xtype: 'b4filefield',
                            fieldLabel: 'Файл',
                            name: 'RequirementFile',
                            width: 600
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 250,
                        labelAlign: 'right',
                        width: 400
                    },
                    title: 'Постановление об изъятии жилого помещения',
                    items: [
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Дата издания',
                            name: 'DecreePublicationDate',
                            format: 'd.m.Y',
                            allowBlank: false
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Реквизиты МПА',
                            name: 'DecreeRequisitesMpa',
                            width: 600,
                            allowBlank: false
                        },
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Дата опубликования МПА',
                            name: 'DecreeMpaPublicationDate',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Дата регистрации МПА, изданного в Управлении Росреестра по РТ',
                            name: 'DecreeMpaRegistrationDate',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Дата уведомления Управления Росреестра по РТ об изданном МПА',
                            name: 'DecreeMpaNotificationDate',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'b4filefield',
                            fieldLabel: 'Файл',
                            name: 'DecreeFile',
                            width: 600
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 250,
                        labelAlign: 'right',
                        width: 400
                    },
                    title: 'Соглашение об изъятии жилого помещения',
                    items: [
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Дата издания',
                            name: 'AgreementPublicationDate',
                            format: 'd.m.Y',
                            allowBlank: false
                        },
                        {
                            xtype: 'b4filefield',
                            fieldLabel: 'Файл',
                            name: 'AgreementFile',
                            width: 600
                        }
                    ]
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
                                    xtype: 'b4savebutton'
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