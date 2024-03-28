Ext.define('B4.view.objectcr.AddWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 500,
    bodyPadding: 5,
    itemId: 'objectCrAddWindow',
    title: 'Объект капитального ремонта',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.form.ComboBox',
        
        'B4.store.dict.programcr.RealityObject',
        'B4.store.dict.ProgramCr',
        
        'B4.ux.button.Close',
        'B4.ux.button.Save',

        'B4.enums.AddWorkFromLongProgram'
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
                    xtype: 'b4selectfield',
                    name: 'ProgramCr',
                    fieldLabel: 'Программа КР',
                    flex: 1,
                    textProperty: 'Name',
                    anchor: '100%',
                    store: 'B4.store.dict.ProgramCr',
                    columns: [
                        {
                            text: 'Программа КР', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' }
                        }
                    ],
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'b4selectfield',
                    name: 'RealityObject',
                    fieldLabel: 'Объект недвижимости',
                    textProperty: 'Address',
                    anchor: '100%',
                    store: 'B4.store.dict.programcr.RealityObject',
                    editable: false,
                    columns: [
                        {
                            text: 'Муниципальный район', dataIndex: 'Municipality', flex: 1,
                            filter: {
                                xtype: 'b4combobox',
                                operand: CondExpr.operands.eq,
                                storeAutoLoad: false,
                                hideLabel: true,
                                editable: false,
                                valueField: 'Name',
                                emptyItem: { Name: '-' },
                                url: '/Municipality/ListMoAreaWithoutPaging'
                            }
                        },
                        {
                            text: 'Адрес',
                            dataIndex: 'Address',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    allowBlank: false,
                    listeners: {
                        beforeload: function (field, options, store) {
                            options.params = options.params || {};

                            var programField = field.up().down('b4selectfield[name = ProgramCr]'),
                                 programId = programField.getValue(),
                                 rec = programField.getStore().getById(programId),
                                 useAddWorkFromDpkr = rec.get('AddWorkFromLongProgram') === B4.enums.AddWorkFromLongProgram.Use;

                            options.params.onlyFromRegionProgram = useAddWorkFromDpkr;
                        }
                    }
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