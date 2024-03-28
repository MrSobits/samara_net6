Ext.define('B4.view.importfrom1468rf.Form', {
    extend: 'Ext.form.Panel',
    
    alias: 'widget.importfrom1468form',
    
    requires: [
         'B4.form.TreeSelectField',
        'B4.form.SelectField',
        'B4.view.dict.municipality.Grid',
        'B4.form.FileField',
        'B4.store.dict.MunicipalityTree'
    ],
    
    initComponent: function() {
        var me = this;

        Ext.apply(me, {
            title: 'Импорт паспортов 1468',
            bodyStyle: Gkh.bodyStyle,
            bodyPadding: 5,
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 250,
                        width: 500
                    },
                    items: [
                        {
                            xtype: 'fieldset',
                            title: 'Импорт из 1468.рф',
                            defaults: {
                                labelWidth: 230,
                                width: 470
                            },
                            margin: '0 0 0 0',
                            items: [
                                {
                                    xtype: 'treeselectfield',
                                    name: 'Municipality',
                                    itemId: 'operatorMunicipalitiesTrigerField',
                                    fieldLabel: 'Муниципальные образования',
                                    titleWindow: 'Выбор муниципального образования',
                                    store: 'B4.store.dict.MunicipalitySelectTree',
                                    allowBlank: true,
                                    editable: false
                                },
                                {
                                    xtype: 'combobox',
                                    name: 'Month',
                                    fieldLabel: 'Месяц',
                                    editable: false,
                                    allowBlank: false,
                                    store: [
                                        [1, 'Январь'],
                                        [2, 'Февраль'],
                                        [3, 'Март'],
                                        [4, 'Апрель'],
                                        [5, 'Май'],
                                        [6, 'Июнь'],
                                        [7, 'Июль'],
                                        [8, 'Август'],
                                        [9, 'Сентябрь'],
                                        [10, 'Октябрь'],
                                        [11, 'Ноябрь'],
                                        [12, 'Декабрь']
                                    ]
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'Year',
                                    fieldLabel: 'Год',
                                    minValue: 1900,
                                    maxValue: 3000,
                                    allowBlank: false,
                                    allowDecimal: false
                                },
                                {
                                    xtype: 'button',
                                    name: 'ImportFromService',
                                    text: 'Импорт',
                                    width: 100,
                                    margin: '0 0 0 370',
                                    disabled: true
                                },
                                {
                                    xtype: 'displayfield',
                                    itemId: 'log',
                                    hidden: true
                                    
                                }
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            title: 'Массовый импорт из файла',
                            margin: '0 0 0 5',
                            defaults: {
                                labelWidth: 230,
                                width: 470
                            },
                            items: [
                                {
                                    xtype: 'form',
                                    bodyStyle: Gkh.bodyStyle,
                                    isFileUpload: true,
                                    border: false,
                                    items: [
                                        {
                                            xtype: 'b4filefield',
                                            name: 'FileImport',
                                            anchor: '100%',
                                            fieldLabel: 'Путь к файлу'
                                        },
                                        {
                                            xtype: 'button',
                                            name: 'ImportFromFile',
                                            text: 'Импорт',
                                            width: 100,
                                            margin: '0 0 0 370',
                                            disabled: true
                                        },
                                        {
                                            xtype: 'displayfield',
                                            itemId: 'logArchive',
                                            hidden: true

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