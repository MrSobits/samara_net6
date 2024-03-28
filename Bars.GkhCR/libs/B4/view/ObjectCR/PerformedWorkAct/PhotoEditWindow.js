Ext.define('B4.view.objectcr.performedworkact.PhotoEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 500,
    bodyPadding: 5,
    itemId: 'photoEditWindow',
    title: 'Фотография',
    alias: 'widget.perfworkactphotowin',

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.PerfWorkActPhotoType'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 140,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Название',
                    maxLength: 100,
                    allowBlank: false
                },
                {
                    xtype: 'b4filefield',
                    editable: false,
                    name: 'Photo',
                    fieldLabel: 'Файл фото',
                    allowBlank: false
                },
                {
                    xtype: 'combobox',
                    editable: false,
                    fieldLabel: 'Тип фото',
                    name: 'PhotoType',
                    store: B4.enums.PerfWorkActPhotoType.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    allowBlank: false
                },
                {
                    xtype: 'textarea',
                    name: 'Discription',
                    fieldLabel: 'Описание',
                    maxLength: 2000,
                    height: 40
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