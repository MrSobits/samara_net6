Ext.define('B4.view.appealcits.PrFondObjectCrEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 800,
    minWidth: 520,
    height: 100,
    bodyPadding: 5,
    itemId: 'prFondObjectCrEditWindow',
    title: 'Форма редактирования объекта КР',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.appealcits.ObjectCrList'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 170,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'ObjectCr',
                    fieldLabel: 'Объект КР',
                    textProperty: 'Address',
                    store: 'B4.store.appealcits.ObjectCrList',
                    editable: false,
                    itemId: 'sfObjCr',
                    columns: [
                        {
                            text: 'Программа',
                            dataIndex: 'Program',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Адрес',
                            dataIndex: 'Address',
                            flex: 1,
                            filter: { xtype: 'textfield' }
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