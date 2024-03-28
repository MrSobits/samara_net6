Ext.define('B4.view.constructionobject.photo.EditWindow', {
    extend: 'B4.form.Window',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    minWidth: 500,
    minHeight: 100,
    bodyPadding: 5,
    alias: 'widget.constructobjphotoeditwindow',
    title: 'Фото-архив',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [        
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.enums.ConstructionObjectPhotoGroup'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 120
            },
            items: [
                {
                    xtype: 'datefield',
                    name: 'Date',
                    fieldLabel: 'Дата изображения',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    maxLength: 150,
                    allowBlank: false
                },
                {
                    xtype: 'combobox',
                    name: 'Group',
                    fieldLabel: 'Группа',
                    store: B4.enums.ConstructionObjectPhotoGroup.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    allowBlank: false
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Описание',
                    maxLength: 500,
                    flex: 1
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
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
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