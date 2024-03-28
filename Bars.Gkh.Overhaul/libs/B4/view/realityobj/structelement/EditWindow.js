Ext.define('B4.view.realityobj.structelement.EditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.rostructeleditwin',

    mixins: ['B4.mixins.window.ModalMask'],
    width: 700,
    minWidth: 700,
    bodyPadding: 5,
    closeAction: 'hide',
    trackResetOnLoad: true,
    structElName:null,
    fields: [],
    engineeringNetwork: false,
    showNameEditor: false,
    bodyStyle: Gkh.bodyStyle,
    margin: -1,
    defaults: {
        labelAlign: 'right',
        labelWidth: 150
    },
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.enums.YesNoNotSet',
        'B4.enums.ConditionStructElement',
        'B4.enums.EngineeringSystemType',
        'B4.view.dict.unitmeasure.Grid',
        'B4.store.dict.UnitMeasure',
        'B4.store.cmnestateobj.StructuralElement',
        'B4.view.Control.GkhDecimalField'
    ],

    initComponent: function () {
        var me = this,
            items = [],
            systemType = null,
            networkLength = null,
            networkPower = null;

        if (me.showNameEditor) {
            items.push({
                xtype: 'textfield',
                //name: 'ElementName',
                name: 'Name',
                fieldLabel: 'Наименование',
                anchor: '100%',
                allowBlank: false
            });
        }

        if (me.engineeringNetwork) {
            systemType = {
                xtype: 'b4combobox',
                name: 'SystemType',
                fieldLabel: 'Тип системы',
                items: B4.enums.EngineeringSystemType.getItems(),
                editable: false
            };
            networkLength = {
                xtype: 'textfield',
                name: 'NetworkLength',
                fieldLabel: 'Протяженность сетей',
                labelWidth: 150,
                labelAlign: 'right',
                flex: 1
            };
            networkPower = {
                xtype: 'gkhdecimalfield',
                name: 'NetworkPower',
                minValue: 0,
                hideTrigger: true,
                fieldLabel: 'Мощность',
                negativeText: 'Значение не может быть отрицательным',
                labelAlign: 'right',
                flex: 1
            };
        }

        items = items.concat([
            systemType,
            {
                xtype: 'b4selectfield',
                fieldLabel: 'Конструктивный элемент',
                name: 'RealityObject',
                store: 'B4.store.RealityObject',
                hidden: true,             
                anchor: '100%',
                textProperty: 'Address',
                labelWidth: 150,
                labelAlign: 'right',
                flex: 1
            },
            {
                xtype: 'b4selectfield',
                fieldLabel: 'Конструктивный элемент',
                name: 'StructuralElement',
                store: 'B4.store.cmnestateobj.StructuralElement',
                editable: false,
                anchor: '100%',
                labelWidth: 150,
                labelAlign: 'right',
                flex: 1
            },
            {
                xtype: 'b4filefield',
                name: 'FileInfo',
                fieldLabel: 'Файл',
                labelWidth: 150,
                labelAlign: 'right',
                flex: 1
            },
            {
                xtype: 'numberfield',
                name: 'LastOverhaulYear',
                fieldLabel: 'Год установки или последнего кап.ремонта',
                minValue: 0,
                hideTrigger: true,
                allowDecimals: false,
                negativeText: 'Значение не может быть отрицательным',
                anchor: '100%',
                flex: 1
            },           
            {
                xtype: 'fieldcontainer',
                layout: 'hbox',
                items: [
                    {
                        xtype: 'numberfield',
                        name: 'Wearout',
                        fieldLabel: 'Износ(%)',
                        minValue: 0,
                        maxValue: 100,
                        hideTrigger: true,
                        allowDecimals: true,
                        negativeText: 'Значение не может быть отрицательным',
                        anchor: '100%',
                        labelWidth: 150,
                        labelAlign: 'right',
                        flex: 1,
                        allowBlank: false
                    },
                    {
                        xtype: 'numberfield',
                        name: 'WearoutActual',
                        fieldLabel: 'Износ акт.(%)',
                        minValue: 0,
                        maxValue: 100,
                        hideTrigger: true,
                        allowDecimals: true,
                        negativeText: 'Значение не может быть отрицательным',
                        anchor: '100%',
                        labelAlign: 'right',
                        flex: 1,
                        allowBlank: false
                    },
                ]
            },
            {
                xtype: 'fieldcontainer',
                layout: 'hbox',
                items: [
                    {
                        xtype: 'b4selectfield',
                        fieldLabel: 'Единица измерения',
                        name: 'UnitMeasure',
                        store: 'B4.store.dict.UnitMeasure',
                        editable: false,
                        disable: true,
                        readOnly: true,
                        allowBlank: false,
                        anchor: '100%',
                        labelWidth: 150,
                        labelAlign: 'right',
                        flex: 1
                    },
                    {
                        xtype: 'numberfield',
                        name: 'Volume',
                        fieldLabel: 'Объем',
                        minValue: 0,
                        hideTrigger: true,
                        allowDecimals: true,
                        negativeText: 'Значение не может быть отрицательным',
                        anchor: '100%',
                        labelAlign: 'right',
                        flex: 1
                    }
                ]
            },
            {
                xtype: 'b4combobox',
                name: 'Condition',
                fieldLabel: 'Состояние',
                items: B4.enums.ConditionStructElement.getItems(),
                editable: false
            },
            {
                xtype: 'fieldcontainer',
                layout: 'hbox',
                items: [
                    networkLength,
                    networkPower
                ]
            }
        ]);
        Ext.each(this.fields, function (field) {
            items.push(field);
        });

        Ext.applyIf(me, {
            title: 'Конструктивный элемент: ' + me.structElName,
            items: items,
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    cmd: 'statechange',
                                    id: 'eledit-state',
                                    text: 'Статус',
                                    menu: []
                                },
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