Ext.define('B4.view.objectcr.FileInfoEditWindow', {
    extend: 'B4.form.Window',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 450,
    minWidth: 400,
    minHeight: 250,
    height: 250,
    bodyPadding: 5,
    itemId: 'objectcrFileInfoEditWindow',
    title: 'Добавление файла',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField'

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
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Примечание',
                    maxLength: 1500
                },
                {
                    xtype: 'b4filefield',
                    name: 'FileInfo',
                    allowblank:false,
                    fieldLabel: 'Файл',
                    editable: false
                },
                {
                    xtype: 'container',
                    margin: '5 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        allowBlank: false,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'VideoLink',
                            itemId: 'tfDistanceCheckDate',
                            fieldLabel: 'Ccылка',
                            flex: 1,
                            allowBlank: true,
                        },
                        {
                            xtype: 'button',
                            text: 'Просмотр',
                            name: 'viewButton',
                            tooltip: 'Посмотреть видео мериприятия',
                            action: 'ViewVideo',
                            iconCls: 'icon-accept',
                            itemId: 'viewButton'
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