Ext.define('B4.view.realityobj.HouseInfoEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.realityobjhouseinfoeditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    minWidth: 500,
    minHeight: 320,
    maxHeight: 320,
    bodyPadding: 5,
    title: 'Сведения о помещении',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.dict.UnitMeasure',

        'B4.enums.KindRightToObject'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 180
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    maxLength: 50
                },
                {
                    xtype: 'textfield',
                    name: 'NumObject',
                    fieldLabel: '№ объекта',
                    maxLength: 300
                },
                {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    name: 'TotalArea',
                    fieldLabel: 'Площадь',
                    decimalSeparator: ',',
                    minValue: 0
                },
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'UnitMeasure',
                    fieldLabel: 'Единица измерения',


                    store: 'B4.store.dict.UnitMeasure',
                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1 },
                    { text: 'Описание', dataIndex: 'Description', flex: 1 }]
                },
                {
                    xtype: 'combobox', editable: false,
                    fieldLabel: 'Вид права',
                    store: B4.enums.KindRightToObject.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'KindRight'
                },
                {
                    xtype: 'textfield',
                    name: 'NumRegistrationOwner',
                    fieldLabel: '№ зарег. права / ограничения',
                    maxLength: 300
                },
                {
                    xtype: 'datefield',
                    name: 'DateRegistration',
                    fieldLabel: 'Дата регистрации права',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'textfield',
                    name: 'Owner',
                    fieldLabel: 'Правообладатель',
                    maxLength: 300
                },
                {
                    xtype: 'container',
                    layout: 'form',
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'DateRegistrationOwner',
                            fieldLabel: 'Дата регистрации (рождения) правообладателя',
                            labelWidth: 300,
                            labelAlign: 'right',
                            format: 'd.m.Y'
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