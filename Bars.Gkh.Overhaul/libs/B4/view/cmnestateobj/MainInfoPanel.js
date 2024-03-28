Ext.define('B4.view.cmnestateobj.MainInfoPanel', {
    extend: 'Ext.form.Panel',
    bodyPadding: 5,
    bodyStyle: Gkh.bodyStyle,
    defaults: {
        width: 800,
        labelWidth: 150,
        labelAlign: 'right'
    },
    
    alias: 'widget.cmnestateobjmaininfo',
    
    title: 'Общие сведения',
    
    requires: [
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.view.dict.grouptype.Grid',
        'B4.store.dict.GroupType'
    ],
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'textfield',
                    fieldLabel: 'Код',
                    name: 'Code',
                    allowBlank: false,
                    maxLength: 200,
                    maxLengthText: 'Количество символов в поле Код превышает 200.'
                },
                {
                    xtype: 'numberfield',
                    name: 'ReformCode',
                    fieldLabel: 'Код реформы',
                    minValue: 0,
                    maxLength: 10,
                    hideTrigger: true,
                    allowDecimals: false,
                    negativeText: 'Значение не может быть отрицательным'
                },
                {
                    xtype: 'textfield',
                    name: 'GisCode',
                    fieldLabel: 'Код ГИС ЖКХ'
                },
                {
                    xtype: 'b4selectfield',
                    fieldLabel: 'Тип группы',
                    name: 'GroupType',
                    store: 'B4.store.dict.GroupType',
                    isGetOnlyIdProperty: true,
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Наименование',
                    name: 'Name',
                    allowBlank: false,
                    maxLength: 500,
                    maxLengthText: "Количество символов в поле Наименование превышает 500."
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Краткое наименование',
                    name: 'ShortName',
                    allowBlank: false,
                    maxLength: 300,
                    maxLengthText: "Количество символов в поле Краткое наименование превышает 300."
                },
                {
                    xtype: 'numberfield',
                    name: 'Weight',
                    fieldLabel: 'Вес',
                    minValue: 0,
                    hideTrigger: true,
                    allowDecimals: false,
                    negativeText: 'Значение не может быть отрицательным'
                },
                {
                    xtype: 'checkbox',
                    boxLabel: 'Соответствует ЖК РФ',
                    name: 'IsMatchHc',
                    margin: '0 0 0 150'
                },
                {
                    xtype: 'checkbox',
                    boxLabel: 'Включен в программу субъекта',
                    name: 'IncludedInSubjectProgramm',
                    margin: '0 0 0 150'
                },
                {
                    xtype: 'checkbox',
                    boxLabel: 'Является инженерной сетью',
                    name: 'IsEngineeringNetwork',
                    margin: '0 0 0 150'
                },
                {
                    xtype: 'checkbox',
                    boxLabel: 'Множественный объект',
                    name: 'MultipleObject',
                    margin: '0 0 0 150'
                },
                {
                    xtype: 'checkbox',
                    boxLabel: 'Является основным',
                    name: 'IsMain',
                    margin: '0 0 0 150'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton'
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