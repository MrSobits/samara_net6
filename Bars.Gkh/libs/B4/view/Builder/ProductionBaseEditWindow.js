Ext.define('B4.view.builder.ProductionBaseEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.store.dict.KindEquipment',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.Control.GkhDecimalField'
    ],
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 500,
    minWidth: 500,
    minHeight: 250,
    bodyPadding: 5,
    itemId: 'builderProductionBaseEditWindow',
    title: 'Производственная база',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'KindEquipment',
                    fieldLabel: 'Вид оснащения',
                    store: 'B4.store.dict.KindEquipment',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'gkhdecimalfield',
                    name: 'Volume',
                    fieldLabel: 'Объем'
                },
                {
                    xtype: 'b4filefield',
                    name: 'DocumentRight',
                    fieldLabel: 'Файл'
                },
                {
                    xtype: 'textarea',
                    name: 'Notation',
                    fieldLabel: 'Примечание',
                    maxLength: 300,
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
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});