Ext.define('B4.view.actsurvey.PhotoEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {type: 'vbox', align: 'stretch'},
    width: 500,
    bodyPadding: 5,
    itemId: 'actSurveyPhotoEditWindow',
    title: 'Форма изображения',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        
        'B4.enums.ImageGroupSurveyGji'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 130,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    maxLength: 300
                },
                {
                    xtype: 'datefield',
                    name: 'ImageDate',
                    fieldLabel: 'Дата изображения',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'combobox', editable: false,
                    floating: false,
                    name: 'Group',
                    fieldLabel: 'Группа',
                    displayField: 'Display',
                    store: B4.enums.ImageGroupSurveyGji.getStore(),
                    valueField: 'Value',
                    editable: false
                },
                {
                    xtype: 'checkboxfield',
                    name: 'IsPrint',
                    fieldLabel: 'Выводить на печать'
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл'
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