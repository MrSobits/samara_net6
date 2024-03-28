Ext.define('B4.view.surveyplan.ContragentAttachmentEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.surveyPlanContragentAttachmentEditWindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    minWidth: 500,
    bodyPadding: 5,
    resizable: false,
    title: 'Редактирование',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField'
    ],
    defaults: {
        labelWidth: 120,
        labelAlign: 'right'
    },
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    allowBlank: false,
                    maxLength: 250
                },
                {
                    xtype: 'textfield',
                    name: 'Num',
                    fieldLabel: 'Номер документа',
                    allowBlank: false,
                    maxLength: 250
                },
                {
                    xtype: 'datefield',
                    format: 'd.m.Y',
                    name: 'Date',
                    fieldLabel: 'Дата документа',
                    allowBlank: false
                },
                {
                    xtype: 'textarea',
                    height: 100,
                    name: 'Description',
                    fieldLabel: 'Описание',
                    maxLength: 2000
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл',
                    allowBlank: false
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
                                    xtype: 'b4closebutton'
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