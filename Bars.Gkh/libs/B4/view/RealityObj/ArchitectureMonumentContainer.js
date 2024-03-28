Ext.define('B4.view.realityobj.ArchitectureMonumentContainer', {
    extend: 'Ext.form.FieldSet',
    alias: 'widget.realityobjarchitecturemonumentcontainer',
    layout: {
        type: 'hbox',
        align: 'stretch'
    },
    title: 'Cтатус объекта культурного наследия',

    defaults: {
      flex: 1  
    },
    margin: '0 5',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 350
                                    },
                                    flex: 4,
                                    items: [
                                        {
                                            xtype: 'b4combobox',
                                            editable: false,
                                            items: [[false, 'Не имеется'], [true, 'Имеется']],
                                            name: 'IsCulturalHeritage',
                                            fieldLabel: 'Статус объекта культурного наследия',
                                            itemId: 'cbIsCulturalHeritageRealityObject'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'MonumentDocumentNumber',
                                            fieldLabel: 'Номер документа'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 170
                                    },
                                    margin: '0 0 0 30',
                                    flex: 3,
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'CulturalHeritageAssignmentDate',
                                            fieldLabel: 'Дата присвоения',
                                            format: 'd.m.Y',
                                            disabled: true
                                        },
                                        {
                                            xtype: 'b4filefield',
                                            name: 'MonumentFile',
                                            fieldLabel: 'Файл'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 350,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'MonumentDepartmentName',
                                    fieldLabel: 'Наименование органа, выдавшего документ о признании дома памятником архитектуры'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 350,
                                flex: 1
                            },
                            margin: '5 0 0 0',
                            items: [
                                {
                                    xtype: 'b4combobox',
                                    editable: false,
                                    items: [[true, 'Да'], [false, 'Нет']],
                                    name: 'IsIncludedRegisterCHO',
                                    fieldLabel: 'Включён в реестр ОКН'
                                },
                                {
                                    xtype: 'b4combobox',
                                    editable: false,
                                    items: [[true, 'Да'], [false, 'Нет']],
                                    name: 'IsIncludedListIdentifiedCHO',
                                    fieldLabel: 'Включён в перечень выявленных ОКН'
                                },
                                {
                                    xtype: 'b4combobox',
                                    editable: false,
                                    items: [[true, 'Да'], [false, 'Нет']],
                                    name: 'IsDeterminedSubjectProtectionCHO',
                                    fieldLabel: 'Предмет охраны ОКН определен'
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
