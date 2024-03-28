Ext.define('B4.view.validitydocperiod.EditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.validitydocperiodeditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 700,
    bodyPadding: 5,
    itemId: 'validitydocperiodeditwindow',
    title: 'Период действия документа ГЖИ',
    closeAction: 'hide',

    requires: [
        'B4.form.EnumCombo',
        'B4.enums.TypeDocumentGji',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4enumcombo',
                    name: 'TypeDocument',
                    fieldLabel: 'Тип документа',
                    enumName: B4.enums.TypeDocumentGji,
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'StartDate',
                    fieldLabel: 'Дата начала действия',
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'EndDate',
                    fieldLabel: 'Дата окончания действия'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4savebutton' }]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4closebutton' }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});