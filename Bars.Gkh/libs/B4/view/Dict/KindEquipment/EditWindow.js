Ext.define('B4.view.dict.kindequipment.EditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 500,
    bodyPadding: 5,
    itemId: 'kindEquipmentEditWindow',
    title: 'Вид оснащения',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.UnitMeasure',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'b4selectfield',
                    name: 'UnitMeasure',
                    fieldLabel: 'Ед. измерения',
                    store: 'B4.store.dict.UnitMeasure',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Описание',
                    flex: 1,
                    maxLength: 500
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