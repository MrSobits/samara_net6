Ext.define('B4.view.al.DataSourceEdit', {
    extend: 'B4.form.Window',
    alias: 'widget.datasourceedit',
    width: 600,
    layout: 'form',
    bodyPadding: 5,
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.ComboBox',
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.enums.al.OwnerType'
    ],

    mixins: ['B4.mixins.window.ModalMask'],
    title: 'Редактирование источника данных',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.base.Store', {
                model: 'B4.model.al.DataSource',
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'DataSource'
                },
                autoLoad: true
            });
        Ext.apply(me, {
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Название',
                    allowBlank: false
                },
                {
                    xtype: 'hidden',
                    name: 'Id',
                    margin: '0 0 0 0'
                },
                {
                    xtype: 'b4enumcombo',
                    hideTrigger: true,
                    readOnly: true,
                    enumName: 'B4.enums.al.OwnerType',
                    name: 'OwnerType',
                    fieldLabel: 'Тип'
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Parent',
                    hidden: true,
                    flex: 1,
                    fieldLabel: 'Источник-родитель',
                    store: store,
                    allowBlank: false,
                    editable: false,
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Name',
                            text: 'Название',
                            flex: 1
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Description',
                            text: 'Описание',
                            flex: 1
                        }
                    ],
                    selectionMode: "SINGLE"
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Описание'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
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
                            columns: 1,
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