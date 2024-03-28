Ext.define('B4.view.dict.categorycsmkd.EditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 800,
    bodyPadding: 5,
    itemId: 'categorycsmkdEditWindow',
    title: 'Категория МКД',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.store.cscalculation.TypeCategoryCS',
        'B4.model.Fias'
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
                    xtype: 'b4selectfield',
                    name: 'TypeCategoryCS',
                    fieldLabel: 'Тип категории',
                    store: 'B4.store.cscalculation.TypeCategoryCS',
                    editable: false,
                    allowBlank: false,
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                    ]
                },
                {
                    xtype: 'textfield',
                    name: 'Code',
                    allowBlank: false,
                    hidden: false,
                    fieldLabel: 'Код'
                },
                {
                    xtype: 'textfield',
                    name: 'Name',
                    allowBlank: false,
                    hidden: false,
                    fieldLabel: 'Наименование'
                },
                {
                    xtype: 'component'
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