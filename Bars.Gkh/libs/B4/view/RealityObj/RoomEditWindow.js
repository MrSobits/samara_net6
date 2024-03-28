Ext.define('B4.view.realityobj.RoomEditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.roomeditwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    width: 455,
    minHeight: 330,
    bodyPadding: 5,
    title: 'Карточка помещения',
    closeAction: 'hide',
    trackResetOnLoad: true,

    layout: 'fit',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.ux.button.ChangeValue',
        'B4.view.entityloglight.EntityLogLightGrid'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: {
                xtype: 'tabpanel',
                border: 0,
                items: [
                    {
                        title: 'Общие сведения',
                        bodyStyle: Gkh.bodyStyle,
                        bodyPadding: '7 5 7 5',
                        border: false,
                        defaults: {
                            margins: '5 5 5 5',
                            labelAlign: 'right',
                            labelWidth: 155,
                            xtype: 'numberfield',
                            allowBlank: false,
                            hideTrigger: true
                        },
                        items: [
                            {
                                xtype: 'hiddenfield',
                                name: 'Id'
                            },
                            {
                                fieldLabel: '№ квартиры/помещения',
                                name: 'RoomNum',
                                itemId: 'tfRoomNum',
                                xtype: 'textfield',
                                maxLength: 10,
                                maskRe: /[a-zA-Zа-яА-Я0-9]/
                            },
                            {
                                xtype: 'checkbox',
                                itemId: 'cbIsRoomHasNoNumber',
                                name: 'IsRoomHasNoNumber',
                                fieldLabel: 'У помещения отсутствует номер'
                            },
                            {
                                xtype: 'container',
                                layout: 'hbox',
                                items: [
                                    {
                                        fieldLabel: 'Общая площадь',
                                        name: 'Area',
                                        labelAlign: 'right',
                                        labelWidth: 155,
                                        xtype: 'numberfield',
                                        allowBlank: false,
                                        editable: false,
                                        hideTrigger: true,
                                        width: 310,
                                        margins: '0 5 5 0',
                                        decimalSeparator: ','
                                    },
                                    {
                                        xtype: 'changevalbtn',
                                        className: 'Room',
                                        propertyName: 'Area',
                                        onValueSaved: function(val) {
                                            var numfield = this.up('container').down('numberfield[name=Area]'),
                                                grid = this.up('roomeditwindow').down('entityloglightgrid');
                                            numfield.setValue(val);
                                            grid.getStore().load();
                                        },
                                        margins: '0 0 5 0'
                                    }
                                ]
                            },
                            {
                                fieldLabel: 'Жилая площадь',
                                name: 'LivingArea'
                            },
                            {
                                fieldLabel: 'Тип помещения',
                                name: 'Type',
                                xtype: 'b4enumcombo',
                                enumName: 'B4.enums.realty.RoomType',
                                includeEmpty: false,
                                enumItems: [],
                                hideTrigger: false
                            },
                            {
                                xtype: 'b4selectfield',
                                name: 'Entrance',
                                fieldLabel: 'Подъезд',
                                textProperty: 'Number',
                                store: 'B4.store.realityobj.Entrance',
                                editable: false,
                                columns: [
                                    { dataIndex: 'Number', flex: 1, text: 'Номер подъезда' },
                                    { dataIndex: 'RealEstateType', flex: 1, text: 'Тип дома' },
                                    { dataIndex: 'Tariff', flex: 1, text: 'Тариф' }
                                ],
                                emptyText: '-',
                                allowBlank: true,
                                listeners: {
                                    windowcreated: {
                                        fn: function (field, window) {
                                            var store = window.down('grid').getStore(),
                                                model,
                                                hasEmptyRec = store.find('Id', 0) > -1,
                                                rec;

                                            if (!hasEmptyRec) {

                                                store.on('load', function (updStore, records) {
                                                    model = store.getProxy().getModel();
                                                    rec = new model({ Id: 0, Number: '-' });
                                                    updStore.add(rec);
                                                });

                                                store.load();
                                            }
                                        }
                                    }
                                }
                            },
                            {
                                fieldLabel: 'Этаж',
                                name: 'Floor',
                                allowDecimals: false
                            },
                            {
                                fieldLabel: 'Количество комнат',
                                name: 'RoomsCount',
                                allowDecimals: false
                            },
                            {
                                fieldLabel: 'Тип собственности',
                                name: 'OwnershipType',
                                xtype: 'b4enumcombo',
                                enumName: 'B4.enums.RoomOwnershipType',
                                includeEmpty: false,
                                enumItems: [],
                                hideTrigger: false,
                                validator: function (value) {
                                    if (value === B4.enums.RoomOwnershipType.Meta.NotSet.Display) {
                                        return 'Это поле обязательно для заполнения';
                                    }

                                    return true;
                                }
                            },
                            {
                                xtype: 'textarea',
                                name: 'Notation',
                                fieldLabel: 'Примечание',
                                maxLength: 300,
                                flex: 1,
                                allowBlank: true
                            },
                            {
                                xtype: 'checkbox',
                                name: 'IsRoomCommonPropertyInMcd',
                                itemId: 'cbIsRoomCommonPropertyInMcd',
                                fieldLabel: 'Помещение составляет общее имущество в МКД'
                            }
                        ]
                    },
                    {
                        title: 'История изменений',
                        disabled: true,
                        border: 0,
                        xtype: 'entityloglightgrid'
                    }
                ]
            },
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
        me.down('entityloglightgrid').getStore().on('beforeload', me.onBeforeLogStoreLoad, me);
        me.down('entityloglightgrid').on('render', function(grid) {
            grid.getStore().load();
        });
    },

    onBeforeLogStoreLoad: function(store, operation) {
        var me = this,
            roomId = me.down('hiddenfield[name=Id]').getValue();

        operation.params = operation.params || {};

        operation.params.entityId = roomId;
        operation.params.parameter = 'room_area';
    }
});