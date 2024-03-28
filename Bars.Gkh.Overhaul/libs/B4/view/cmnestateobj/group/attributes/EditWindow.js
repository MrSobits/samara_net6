Ext.define('B4.view.cmnestateobj.group.attributes.EditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.groupattributeseditwindow',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',

        'B4.enums.AttributeType'
    ],

    modal: true,
    layout: { type: 'vbox', align: 'stretch' },
    width: 500,
    bodyPadding: 5,
    title: 'Добавление/редактирование группы конструктивного элемента',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 100
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    anchor: '100%',
                    allowBlank: false
                },
                {
                    xtype: 'combobox',
                    editable: false,
                    fieldLabel: 'Тип атрибута',
                    store: B4.enums.AttributeType.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'AttributeType'
                },
                {
                    xtype: 'checkbox',
                    name: 'IsNeeded',
                    fieldLabel: 'Обязательность',
                    anchor: '100%',
                    allowBlank: false
                },
                {
                    xtype: 'textarea',
                    fieldLabel: 'Подсказка',
                    name: 'Hint',
                    maxLength: 3000,
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