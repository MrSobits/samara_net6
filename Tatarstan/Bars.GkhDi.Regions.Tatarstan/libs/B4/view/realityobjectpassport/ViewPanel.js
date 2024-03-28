Ext.define('B4.view.realityobjectpassport.ViewPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.realityobjectpassportviewpanel',

    closable: true,
    layout: 'border',
    bodyPadding: 5,
    itemId: 'realityobjectpassportViewPanel',
    title: 'Паспорт',
    trackResetOnLoad: true,
    autoScroll: true,
    requires: [
        'B4.form.ComboBox',
        'B4.ux.grid.Panel',
        'B4.ux.button.Update'

    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        }
                    ]
                }
            ],

            items: [
                {
                    xtype: 'container',
                    region: 'north',
                    padding: 2,
                    style: 'font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 3px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">На данной странице представлены данные только для чтения. Для редактирования данных необходимо перейти в Реестр жилых домов/паспорт дома</span>'
                },
                {
                    xtype: 'container',
                    region: 'north',
                    items: [
                        {
                            xtype: 'fieldset',
                            itemId: 'fsActState',
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right',
                                labelWidth: 320
                            },
                            title: 'Основная информация',
                            items: [
                                {
                                    xtype: 'numberfield',
                                    name: 'BuildYear',
                                    fieldLabel: 'Год постройки',
                                    hideTrigger: true,
                                    allowDecimals: false,
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true,
                                    qtipText: 'Данные из [Жилой дом] - [Общие сведения] - [Год постройки]'

                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'DateCommissioning',
                                    fieldLabel: 'Год ввода в эксплуатацию',
                                    hideTrigger: true,
                                    allowDecimals: false,
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true,
                                    qtipText: 'Данные из [Жилой дом] - [Общие сведения] - [Год из даты сдачи в эксплуатацию]'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'TypeOfProject',
                                    fieldLabel: 'Серия, тип постройки здания',
                                    hideTrigger: true,
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true,
                                    qtipText: 'Данные из [Жилой дом] - [Общие сведения] - [Групбокс Серия, тип проекта]'
                                },
                                {
                                    xtype: 'combobox',
                                    editable: false,
                                    name: 'TypeHouse',
                                    fieldLabel: 'Тип дома',
                                    displayField: 'Display',
                                    store: B4.enums.TypeHouse.getStore(),
                                    valueField: 'Value',
                                    allowBlank: false,
                                    maxWidth: 540,
                                    readOnly: true,
                                    qtipText: 'Данные из [Жилой дом] - [Общие сведения] - [Тип дома]'
                                },
                                {
                                    xtype: 'textfield',
                                    floating: false,
                                    name: 'TypeOfFormingCr',
                                    fieldLabel: 'Способ формирования фонда капитального ремонта',
                                    hideTrigger: true,
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    region: 'north',
                    items: [
                        {
                            xtype: 'fieldset',
                            itemId: 'fsActState',
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right',
                                labelWidth: 320
                            },
                            title: 'Количество этажей',
                            items: [
                                {
                                    xtype: 'numberfield',
                                    name: 'Floors',
                                    fieldLabel: 'Наименьшее (ед)',
                                    hideTrigger: true,
                                    allowDecimals: false,
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true,
                                    qtipText: 'Данные из [Жилищный фонд] - [Объекты жилищного фонда] - [Реестр жилых домов] - [Дом] - [раздел Технический паспорт] - [Раздел Общие сведения о жилом доме] - [Количество этажей, наименьшее]'
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'MaximumFloors',
                                    fieldLabel: 'Наибольшее (ед)',
                                    hideTrigger: true,
                                    allowDecimals: false,
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true,
                                    qtipText: 'Данные из [Жилищный фонд] - [Объекты жилищного фонда] - [Реестр жилых домов] - [Дом] - [раздел Технический паспорт] - [Раздел Общие сведения о жилом доме] - [Количество этажей, наибольшее]'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    region: 'north',
                    items: [
                        {
                            xtype: 'fieldset',
                            itemId: 'fsActState',
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right',
                                labelWidth: 320
                            },
                            title: 'Дополнительная информация',
                            items: [
                                {
                                    xtype: 'numberfield',
                                    name: 'NumberEntrances',
                                    fieldLabel: 'Количество подъездов',
                                    hideTrigger: true,
                                    allowDecimals: false,
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true,
                                    qtipText: 'Данные из [Жилищный фонд] - [Объекты жилищного фонда] - [Реестр жилых домов] - [Дом] - [раздел Технический паспорт] - [Раздел Общие сведения о жилом доме] - [Количество подъездов]'
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'NumberLifts',
                                    fieldLabel: 'Количество лифтов',
                                    hideTrigger: true,
                                    allowDecimals: false,
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true,
                                    qtipText: 'Данные из [Жилищный фонд] - [Объекты жилищного фонда] - [Реестр жилых домов] - [Дом] - [раздел Технический паспорт] - [Раздел Общие сведения о жилом доме] - [Количество лифтов]'
                                },
                                {
                                    xtype: 'textfield',
                                    floating: false,
                                    name: 'EnergyEfficiencyClass',
                                    fieldLabel: 'Класс энергетической эффективности',
                                    hideTrigger: true,
                                    readOnly: true,
                                    flex: 0.8,
                                    maxWidth: 540,
                                    qtipText: 'Данные из [Жилой Дом] - [Паспорт технического объекта] - [6.1 Энергетический аудит] - [Таблица "Общие сведения"] - [Класс энергоэффективности здания]'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    region: 'north',
                    items: [
                        {
                            xtype: 'fieldset',
                            itemId: 'fsActState',
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right',
                                labelWidth: 320
                            },
                            title: 'Количество помещений',
                            items: [
                                {
                                    xtype: 'numberfield',
                                    name: 'AllNumberOfPremises',
                                    fieldLabel: 'Всего (ед)',
                                    hideTrigger: true,
                                    allowDecimals: false,
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true,
                                   qtipText: 'Данные из [Жилищный фонд] - [Объекты жилищного фонда] - [Реестр жилых домов] - [Дом] - [раздел Технический паспорт] - [Раздел Общие сведения о жилом доме] - [Сумма (Количество квартир и количество нежилых помещений)]'
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'NumberApartments',
                                    fieldLabel: 'Жилых (ед)',
                                    hideTrigger: true,
                                    allowDecimals: false,
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true,
                                    qtipText: 'Данные из [Жилищный фонд] - [Объекты жилищного фонда] - [Реестр жилых домов] - [Дом] - [раздел Технический паспорт] - [Раздел Общие сведения о жилом доме] - [Количество квартир]'
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'NumberNonResidentialPremises',
                                    fieldLabel: 'Нежилых (ед)',
                                    hideTrigger: true,
                                    allowDecimals: false,
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true,
                                    qtipText: 'Данные из [Жилищный фонд] - [Объекты жилищного фонда] - [Реестр жилых домов] - [Дом] - [раздел Технический паспорт] - [Раздел Общие сведения о жилом доме] - [Количество нежилых помещений]'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    region: 'north',
                    items: [
                        {
                            xtype: 'fieldset',
                            itemId: 'fsActState',
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right',
                                labelWidth: 320
                            },
                            title: 'Общая площадь дома',
                            items: [
                                {
                                    xtype: 'numberfield',
                                    name: 'AreaMkd',
                                    fieldLabel: 'Общая площадь дома (кв.м)',
                                    hideTrigger: true,
                                    allowDecimals: true,
                                    decimalSeparator: ',',
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true,
                                    qtipText: 'Данные из [Жилой Дом] - [Общие сведения] - [Групбокс "Общие характеристики"] - [Общая площадь МКД (кв. м.)]'
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'AreaLiving',
                                    fieldLabel: 'Общая площадь жилых помещений (кв.м)',
                                    hideTrigger: true,
                                    allowDecimals: true,
                                    decimalSeparator: ',',
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true,
                                    qtipText: 'Данные из [Жилой Дом] - [Общие сведения] - [Групбокс "Общие характеристики"] - [В т. ч. жилых всего (кв. м.)]'
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'AreaNotLivingPremises',
                                    fieldLabel: 'Общая площадь нежилых помещений (кв.м)',
                                    hideTrigger: true,
                                    allowDecimals: true,
                                    decimalSeparator: ',',
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true,
                                    qtipText: 'Данные из [Жилой Дом] - [Общие сведения] - [Групбокс "Общие характеристики"] -[ В т. ч. нежилых всего (кв. м.)]'
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'AreaOfAllNotLivingPremises',
                                    fieldLabel: 'Общая площадь помещений, входящих в состав общего имущества (кв.м)',
                                    hideTrigger: true,
                                    allowDecimals: true,
                                    decimalSeparator: ',',
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true,
                                    qtipText: 'Данные из [Жилой дом] - [Общие сведения] - [Групбокс "Общие характеристики"] - [Общая площадь помещений, входящих в состав общего имущества (кв.м.)]'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    region: 'north',
                    items: [
                        {
                            xtype: 'fieldset',
                            itemId: 'fsActState',
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right',
                                labelWidth: 320
                            },
                            title: 'Сведения о земельном участке, на котором расположен многоквартирный дом',
                            items: [
                                {
                                    xtype: 'textfield',
                                    floating: false,
                                    name: 'CadastreNumber',
                                    fieldLabel: 'Кадастровый номер земельного участка',
                                    hideTrigger: true,
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true,
                                    qtipText: 'Данные из [Жилой дом] - [Общие сведения] - [Общие характеристики] - [Кадастровый номер земельного участка]'
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'DocumentBasedArea',
                                    fieldLabel: 'Площадь земельного участка, входящего в состав общего имущества в многоквартирном доме (кв. м)',
                                    hideTrigger: true,
                                    allowDecimals: true,
                                    decimalSeparator: ',',
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true,
                                    qtipText: 'Данные из [Жилой дом] - [Паспорт технического объекта] - [2. Экспликация земельного участка] - [Общая площадь земельного участка по документам (кв. м.)]'
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'ParkingArea',
                                    fieldLabel: 'Площадь парковки в границах земельного участка (кв. м)',
                                    hideTrigger: true,
                                    allowDecimals: true,
                                    decimalSeparator: ',',
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true,
                                    qtipText: 'Данные из [Жилой дом] - [Паспорт технического объекта] - [2. Экспликация земельного участка] - [Площадь парковки в границах земельного участка (кв. м)]'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    region: 'north',
                    items: [
                        {
                            xtype: 'fieldset',
                            itemId: 'fsActState',
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right',
                                labelWidth: 320
                            },
                            title: 'Элементы благоустройства',
                            items: [
                                {
                                    xtype: 'textfield',
                                    floating: false,
                                    name: 'ChildrenArea',
                                    fieldLabel: 'Детская площадка',
                                    hideTrigger: true,
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true,
                                    qtipText: 'Данные из [Жилой Дом] - [Паспорт технического объекта] - [2. Экспликация земельного участка] - [Площадки детские]'
                                },
                                {
                                    xtype: 'textfield',
                                    floating: false,
                                    name: 'SportArea',
                                    fieldLabel: 'Спортивная площадка',
                                    hideTrigger: true,
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true,
                                    qtipText: 'Данные из [Жилой Дом - [Паспорт технического объекта - [2. Экспликация земельного участка] - [Площадки спортивные]'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    region: 'north',
                    name: 'OwnerInfoContainer',
                    itemId: 'ownerInfoContainer',
                    hidden: true,
                    items: [
                        {
                            xtype: 'fieldset',
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right',
                                labelWidth: 320,
                                flex: 0.8,
                                maxWidth: 540,
                                readOnly: true
                            },
                            title: 'Сведения о владельце специального счета',
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'OwnerInn',
                                    fieldLabel: 'ИНН владельца',
                                    qtipText: 'Данные из [Участники процесса] - [Контрагент]'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'OwnerName',
                                    fieldLabel: 'Наименование владельца',
                                    qtipText: 'Данные из [Участники процесса] - [Контрагент]'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    region: 'north',
                    name: 'ProtocolInfoContainer',
                    itemId: 'protocolInfoContainer',
                    hidden: true,
                    items: [
                        {
                            xtype: 'fieldset',
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right',
                                labelWidth: 320,
                                flex: 0.8,
                                maxWidth: 540,
                                readOnly: true
                            },
                            title: 'Реквизиты протокола общего собрания собственников помещений, на котором принято решение о способе формирования фонда капитального ремонта',
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'ProtocolDate',
                                    fieldLabel: 'Дата протокола',
                                    format: 'd.m.Y',
                                    hideTrigger: true,
                                    qtipText: 'Данные из [Жилищный фонд] - [Реестр жилых домов] - [Жилой дом] - [Протокол решений]'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'ProtocolNumber',
                                    fieldLabel: 'Номер протокола',
                                    qtipText: 'Данные из [Жилищный фонд] - [Реестр жилых домов] - [Жилой дом] - [Протокол решений]'
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'Paysize',
                                    fieldLabel: 'Размер взноса на капитальный ремонт на 1 кв. м (руб.)',
                                    hideTrigger: true,
                                    allowDecimals: true,
                                    decimalSeparator: ',',
                                    qtipText: '1. Данные из [Жилищный фонд] - [Реестр жилых домов] - [Жилой дом] - [Протокол решений] - [Принятое решение]. 2. [Капитальный ремонт] - [Параметры программы капитального ремонта]'
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