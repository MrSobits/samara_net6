Ext.define('B4.view.realityobj.RoomEditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.roomeditwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    width: 550,
    minHeight: 330,
    height: 600,
    bodyPadding: 5,
    title: 'Карточка помещения',
    closeAction: 'hide',
    trackResetOnLoad: true,

    layout: 'fit',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.EnumCombo',
        'B4.ux.button.ChangeValue',
        'B4.form.SelectField',
        'B4.view.entityloglight.EntityLogLightGrid',
        'B4.enums.RoomOwnershipType',
        'B4.enums.YesNo',
        'B4.form.FileField'
    ],

    initComponent: function () {
        var me = this;
        me.closable = false;

        Ext.applyIf(me, {
            items: {
                xtype: 'tabpanel',
                border: 0,
                items: [
                    {
                        title: 'Общие сведения',
                        bodyStyle: Gkh.bodyStyle,
                        bodyPadding: '7 25 7 5',
                        border: false,
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                        autoScroll: true,
                        defaults: {
                      //      margins: '5 5 5 5',
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
                                xtype: 'container',
                                layout: 'hbox',
                                defaults: {
                                    flex: 1
                                },
                                items: [
                                    {
                                        fieldLabel: '№ квартиры/помещения',
                                        name: 'RoomNum',
                                        labelAlign: 'right',
                                        labelWidth: 155,
                                        xtype: 'textfield',
                                        maskRe: /[\/\-a-zA-Zа-яА-Я0-9]/,
                                        allowBlank: false,
                                        editable: false,
                                        readOnly: true,
                                        enumItems: [],
                                        maxLength: 100,
                                        itemId: 'tfRoomNum',
                                        hideTrigger: true,
                                 //       width: 310,
                                        margins: '0 5 5 0'
                                    },
                                    {
                                        xtype: 'changevalbtn',
                                        className: 'Room',
                                        propertyName: 'RoomNum',
                                        width: 50,
                                        valueFieldConfig: {
                                            xtype: 'textfield',
                                            type: 'RoomNum',
                                            fieldLabel: 'Новое значение',
                                            maxLength: 10,
                                            maskRe: /[\/\-a-zA-Zа-яА-Я0-9]/
                                        },
                                        onValueSaved: function (val) {
                                            var roomfield = this.up('container').down('textfield[name=RoomNum]'),
                                                grid = this.up('roomeditwindow').down('entityloglightgrid');
                                            roomfield.setValue(val);
                                            grid.getStore().load();
                                        },
                                        margins: '0 0 5 0'
                                    }
                                ]
                            },
                            {
                                xtype: 'checkbox',
                                name: 'IsRoomHasNoNumber',
                                itemId: 'cbIsRoomHasNoNumber',
                                fieldLabel: 'У помещения отсутствует номер'
                            },
                            {
                                xtype: 'checkbox',
                                name: 'IsCommunal',
                                fieldLabel: 'Коммунальная квартира'
                            },
                            {
                                xtype: 'container',
                                layout: 'hbox',
                                type: 'ChamberNum',
                                defaults: {
                                    flex: 1
                                },
                                items: [
                                    {
                                        fieldLabel: '№ комнаты',
                                        name: 'ChamberNum',
                                        labelAlign: 'right',
                                        labelWidth: 155,
                                        xtype: 'textfield',
                                        maskRe: /[\/\-a-zA-Zа-яА-Я0-9]/,
                                        editable: false,
                                        readOnly: true,
                                        enumItems: [],
                                        maxLength: 10,
                                        itemId: 'tfChamberum',
                                        hideTrigger: true,
                                        width: 310,
                                        margins: '0 5 5 0'
                                    },
                                    {
                                        xtype: 'changevalbtn',
                                        className: 'Room',
                                        propertyName: 'ChamberNum',
                                        valueFieldConfig: {
                                            xtype: 'textfield',
                                            type: 'ChamberNum',
                                            fieldLabel: 'Новое значение',
                                            maxLength: 10,
                                            maskRe: /[\/\-a-zA-Zа-яА-Я0-9]/
                                        },
                                        onValueSaved: function (val) {
                                            var roomfield = this.up('container').down('textfield[name=ChamberNum]'),
                                                grid = this.up('roomeditwindow').down('entityloglightgrid');
                                            roomfield.setValue(val);
                                            grid.getStore().load();
                                        },
                                        margins: '0 0 5 0'
                                    }
                                ]
                            },
                            {
                                xtype: 'container',
                                layout: 'hbox',
                                defaults: {
                                    flex: 1
                                },
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
                                        decimalSeparator: ',',
                                        width: 310,
                                        margins: '0 5 5 0',
                                        minValue: 0
                                    },
                                    {
                                        xtype: 'changevalbtn',
                                        className: 'Room',
                                        propertyName: 'Area',
                                        onValueSaved: function (val) {
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
                                name: 'LivingArea',
                                decimalSeparator: ',',
                                allowBlank: true,
                            },
                            {
                                fieldLabel: 'Площадь общего имущества в коммунальной квартире',
                                name: 'CommunalArea',
                                decimalSeparator: ',',
                                decimalPrecision: 2,
                                disabled: true,
                                hidden: true,
                                maxLength: 10
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
                                store: 'B4.store.realityobj.Entrance',
                                textProperty: 'Number',
                                editable: false,
                                columns: [
                                    { dataIndex: 'Number', flex: 1, text: 'Номер подъезда' },
                                    { dataIndex: 'RealEstateType', flex: 1, text: 'Тип дома' },
                                    { dataIndex: 'Tariff', flex: 1, text: 'Тариф' }
                                ],
                                emptyText: '-',
                                hideTrigger: false,
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
                                xtype: 'checkbox',
                                name: 'HasSeparateEntrance',
                                fieldLabel: 'Помещение имеет отдельный вход'
                            },
                            {
                                fieldLabel: 'Этаж',
                                name: 'Floor',
                                allowDecimals: false,
                                allowBlank: true
                            },
                            {
                                fieldLabel: 'Количество комнат',
                                name: 'RoomsCount',
                                allowDecimals: false,
                                allowBlank: true
                            },
                            {
                                xtype: 'container',
                                layout: 'hbox',
                                defaults: {
                                    flex: 1
                                },
                                items: [
                                    {
                                        fieldLabel: 'Тип собственности',
                                        name: 'OwnershipType',
                                        labelAlign: 'right',
                                        labelWidth: 155,
                                        xtype: 'b4enumcombo',
                                        enumName: 'B4.enums.RoomOwnershipType',
                                        allowBlank: false,
                                        editable: false,
                                        readOnly: true,
                                        includeEmpty: false,
                                        enumItems: [],
                                        hideTrigger: false,
                                        width: 310,
                                        margins: '0 5 5 0',
                                        validator: me.ownershipTypeValidator
                                    },
                                    {
                                        xtype: 'changevalbtn',
                                        className: 'Room',
                                        propertyName: 'OwnershipType',
                                        valueFieldConfig: {
                                            xtype: 'b4enumcombo',
                                            enumName: 'B4.enums.RoomOwnershipType',
                                            includeEmpty: false,
                                            fieldLabel: 'Новое значение',
                                            allowBlank: false,
                                            validator: me.ownershipTypeValidator
                                        },
                                        onValueSaved: function (val) {
                                            var enumfield = this.up('container').down('b4enumcombo[name=OwnershipType]'),
                                                grid = this.up('roomeditwindow').down('entityloglightgrid');
                                            enumfield.setValue(val);
                                            grid.getStore().load();
                                        },
                                        margins: '0 0 5 0'
                                    }
                                ]
                            },
                            {
                                fieldLabel: 'Количество абонентов',
                                name: 'AccountsNum',
                                minValue: 0,
                                editable: false,
                                allowBlank: true,
                                fieldStyle: {
                                    backgroundImage: 'none;',
                                    backgroundColor: '#CCCCCC'
                                }
                            },
                            {
                                xtype: 'textarea',
                                name: 'Notation',
                                fieldLabel: 'Примечание',
                                maxLength: 300,
                                minHeight: 50,
                                flex: 1,
                                allowBlank: true,
                                itemId: 'notation'
                            },
                            {
                                xtype: 'textfield',
                                name: 'CadastralNumber',
                                fieldLabel: 'Кадастровый номер',
                                hidden: true,
                                disabled: true,
                                maxLength: 50,
                                allowBlank: true
                            },
                            {
                                xtype: 'numberfield',
                                name: 'PrevAssignedRegNumber',
                                fieldLabel: 'Ранее присвоенный гос. учетный номер',
                                hideTrigger: true,
                                decimalSeparator: ',',
                                decimalPrecision: 2,
                                maxLength: 10,
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
                    },
                    {
                        title: 'Сведения о непригодности помещения',
                        bodyStyle: Gkh.bodyStyle,
                        name: 'InformationUnfitness',
                        bodyPadding: '7 20 7 5',
                        border: false,
                        autoScroll: true,
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                        defaults: {
                            margins: '5 5 5 5',
                            labelAlign: 'right',
                            labelWidth: 170
                        },
                        items: [
                            {
                                xtype: 'combobox',
                                fieldLabel: 'Наличие признания квартиры непригодной для проживания',
                                name: 'RecognizedUnfit',
                                store: B4.enums.YesNo.getStore(),
                                displayField: 'Display',
                                valueField: 'Value',
                                editable: false
                            },
                            {
                                xtype: 'textfield',
                                fieldLabel: 'Основание признания квартиры непригодной для проживания',
                                name: 'RecognizedUnfitReason',
                                maxLength: 255
                            },
                            {
                                xtype: 'numberfield',
                                name: 'RecognizedUnfitDocNumber',
                                fieldLabel: 'Номер документа',
                                hideTrigger: true,
                                allowDecimals: false,
                                maxLength: 10
                            },
                            {
                                xtype: 'datefield',
                                name: 'RecognizedUnfitDocDate',
                                fieldLabel: 'Дата документа',
                                format: 'd.m.Y'
                            },
                            {
                                xtype: 'b4filefield',
                                name: 'RecognizedUnfitDocFile',
                                fieldLabel: 'Документ о признании квартиры непригодной для проживания'
                            }
                        ]
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
                            items: [
                                {
                                    text: 'Переходы',
                                    iconCls: 'icon-arrow-out',
                                    menu: {
                                        xtype: 'menu',
                                        items: [
                                            {
                                                text: 'Перейти к ЛС',
                                                action: 'redirecttopersonalaccount',
                                                iconCls: 'icon-arrow-out'
                                            },
                                            {
                                                text: 'Перейти к абоненту',
                                                action: 'redirecttoabonent',
                                                iconCls: 'icon-arrow-out'
                                            }
                                        ]
                                    }
                                }
                            ]
                        },
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
                            name: 'closebuttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ],
            listeners: {
                close: function () {
                    var me = this;
                    if (me.editRoomMode) {
                        var items = B4.getBody().items;
                        var index = items.findIndexBy(function (tab) {
                            return tab.urlToken != null && tab.urlToken.indexOf('realityobjectedit') === 0;
                        });

                        if (index != -1) {
                            B4.getBody().remove(items.items[index], true);
                        }
                    }
                }
            }
        });


        me.callParent(arguments);
        me.down('entityloglightgrid').getStore().on('beforeload', me.onBeforeLogStoreLoad, me);
        me.down('entityloglightgrid').on('render', function (grid) {
            grid.getStore().load();
        });
    },

    onBeforeLogStoreLoad: function (store, operation) {
        var me = this,
            roomId = me.down('hiddenfield[name=Id]').getValue();

        operation.params = operation.params || {};

        operation.params.entityId = roomId;
        operation.params.className = 'Room';
        operation.params.parameters = ['room_area', 'room_ownership_type', 'room_num', 'room_chamber_num'];
    },

    ownershipTypeValidator: function(value) {
        if (value === B4.enums.RoomOwnershipType.Meta.NotSet.Display) {
            return 'Это поле обязательно для заполнения';
        }

        return true;
    }
});