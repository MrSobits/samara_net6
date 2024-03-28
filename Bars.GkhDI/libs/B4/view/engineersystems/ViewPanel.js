Ext.define('B4.view.engineersystems.ViewPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: 'border',
    bodyPadding: 5,
    itemId: 'disinfoengineersystemspanel',
    title: 'Инженерные системы',
    trackResetOnLoad: true,
    autoScroll: true,
    requires: [
        'B4.ux.grid.Panel',
        'B4.form.ComboBox'
    ],
    alias: 'widget.disinfoengineersystemspanel',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
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
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right',
                                labelWidth: 320
                            },
                            title: 'Описание - Инженерные системы',
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'TypeHeating',
                                    fieldLabel: 'Тип системы теплоснабжения',
                                    hideTrigger: true,
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true,
                                    qtipText: 'Данные из [Жилой дом] - [Паспорт тех. объекта] - [3.1 Отопление (теплоснабжение)]'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'TypeHotWater',
                                    fieldLabel: 'Тип системы горячего водоснабжения',
                                    hideTrigger: true,
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true,
                                    qtipText: 'Данные из [Жилой дом] - [Паспорт тех. объекта] - [3.2 Горячее водоснабжение (ГВС)]'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'TypeColdWater',
                                    fieldLabel: 'Тип системы холодного водоснабжения',
                                    hideTrigger: true,
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true,
                                    qtipText: 'Данные из [Жилой дом] - [Паспорт тех. объекта] - [3.3 Холодное водоснабжение (ХВС)]'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'TypeGas',
                                    fieldLabel: 'Тип системы газоснабжения',
                                    hideTrigger: true,
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true,
                                    qtipText: 'Данные из [Жилой дом] - [Паспорт тех. объекта] - [3.6 Газоснабжение]'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'TypeVentilation',
                                    fieldLabel: 'Тип системы вентиляции',
                                    hideTrigger: true,
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true,
                                    qtipText: 'Данные из [Жилой дом] - [Паспорт тех. объекта] - [3.7 Вентиляция]'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Firefighting',
                                    fieldLabel: 'Тип системы пожаротушения',
                                    hideTrigger: true,
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true,
                                    qtipText: 'Данные из [Жилой дом] - [Паспорт тех. объекта] - [3.10 Пожаротушение]'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'TypeDrainage',
                                    fieldLabel: 'Тип системы водостоков',
                                    hideTrigger: true,
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true,
                                    qtipText: 'Данные из [Жилой дом] - [Паспорт тех. объекта] - [3.8 Водостоки]'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            region: 'north',
                            items: [
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        anchor: '100%',
                                        labelAlign: 'right',
                                        labelWidth: 320
                                    },
                                    title: 'Группа "Система электроснабжения"',
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'TypePower',
                                            fieldLabel: 'Тип системы электроснабжения',
                                            hideTrigger: true,
                                            flex: 0.8,
                                            maxWidth: 540,
                                            readOnly: true,
                                            qtipText: 'Данные из [Жилой дом] - [Паспорт тех. объекта] - [3.5 Электроснабжение]'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'TypePowerPoints',
                                            fieldLabel: 'Количество вводов в дом',
                                            hideTrigger: true,
                                            flex: 0.8,
                                            maxWidth: 540,
                                            readOnly: true,
                                            qtipText: 'Данные из [Жилой дом] - [Паспорт тех. объекта] - [3.5 Электроснабжение] - [Система электроснабжения]'
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
                                    defaults: {
                                        anchor: '100%',
                                        labelAlign: 'right',
                                        labelWidth: 320
                                    },
                                    title: 'Группа "Водоотведения"',
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'TypeSewage',
                                            fieldLabel: 'Тип системы водоотведения',
                                            hideTrigger: true,
                                            flex: 0.8,
                                            maxWidth: 540,
                                            readOnly: true,
                                            qtipText: 'Данные из [Жилой дом] - [Паспорт тех. объекта] - [3.4 Водоотведение (канализация)]'
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'SewageVolume',
                                            fieldLabel: 'Объем выгребных ям (куб. м.)',
                                            hideTrigger: true,
                                            allowDecimals: false,
                                            flex: 0.8,
                                            maxWidth: 540,
                                            readOnly: true,
                                            qtipText: 'Данные из [Жилой дом] - [Паспорт тех. объекта] - [3.4 Водоотведение (канализация)] - [Количественные показатели]'
                                        }
                                    ]
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