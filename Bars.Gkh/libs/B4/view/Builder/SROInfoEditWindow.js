Ext.define('B4.view.builder.SroInfoEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.store.dict.Work',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 500,
    minWidth: 500,
    minHeight: 125,
    maxHeight: 125,
    bodyPadding: 5,
    itemId: 'builderSroInfoEditWindow',
    title: 'Сведения об участии в СРО',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 80,
                labelAlign: 'right',
                editable: false
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'Work',
                    fieldLabel: 'Работа',
                    store: 'B4.store.dict.Work',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'b4filefield',
                    name: 'DescriptionWork',
                    fieldLabel: 'Файл'
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