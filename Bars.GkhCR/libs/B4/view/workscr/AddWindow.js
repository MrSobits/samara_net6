Ext.define('B4.view.workscr.AddWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    bodyPadding: 5,
    
    alias: 'widget.workscraddwin',
    itemId: 'workscraddwin',
    title: 'Объект капитального ремонта',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.form.ComboBox',

        'B4.store.RealityObject',
        'B4.store.dict.ProgramCr',
        'B4.store.dict.Work',

        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 140,
                labelAlign: 'right',
                allowBlank: false,
                editable: false
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'RealityObject',
                    fieldLabel: 'Объект недвижимости',
                    store: 'B4.store.RealityObject',
                    textProperty: 'Address',
                    columns: [
                        {
                            text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
                            filter: {
                                xtype: 'b4combobox',
                                operand: CondExpr.operands.eq,
                                storeAutoLoad: false,
                                hideLabel: true,
                                editable: false,
                                valueField: 'Name',
                                emptyItem: { Name: '-' },
                                url: '/Municipality/ListWithoutPaging'
                            }
                        },
                        {
                            text: 'Адрес',
                            dataIndex: 'Address',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'ProgramCr',
                    fieldLabel: 'Программа КР',
                    store: 'B4.store.dict.ProgramCr',
                    textProperty: 'Name',
                    columns: [
                        { text: 'Программа КР', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Work',
                    fieldLabel: 'Вид работы',
                    store: 'B4.store.dict.Work',
                    textProperty: 'Name',
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
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