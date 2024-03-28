Ext.define('B4.view.baseplanaction.AddWindow', {
    extend: 'B4.form.Window',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 600,
    bodyPadding: 5,
    title: 'Проверка по плану мероприятий',
    closeAction: 'hide',
    trackResetOnLoad: true,

    alias: 'widget.basePlanActionAddWindow',
    
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        
        'B4.form.ComboBox',
        
        'B4.store.dict.PlanActionGji',
        'B4.store.Contragent',
        
        'B4.enums.TypeJurPerson',
        'B4.enums.PersonInspection'
    ],

    initComponent: function () {
        var me = this,
            currTypeJurPerson = B4.enums.TypeJurPerson.getItems(),
            currPersonInspection = B4.enums.PersonInspection.getItems(),
            newPersonInspection = [],
            newTypeJurPerson = [];

        Ext.iterate(currTypeJurPerson, function (val) {
            if (val[0] != 30 && val[0] != 70)
                newTypeJurPerson.push(val);
        });
        
        Ext.iterate(currPersonInspection, function (val) {
            if (val[0] != 30)
                newPersonInspection.push(val);
        });
        
        Ext.applyIf(me, {
            defaults: {
                labelWidth: 170,
                labelAlign: 'right',
                anchor: '100%'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.PlanActionGji',
                    name: 'Plan',
                    fieldLabel: 'План',
                    allowBlank: false,
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                    ]
                },
                {
                    xtype: 'b4combobox',
                    name: 'PersonInspection',
                    editable: false,
                    allowBlank: false,
                    fieldLabel: 'Тип субъекта наблюдения',
                    displayField: 'Display',
                    items: newPersonInspection,
                    valueField: 'Value'
                },
                {
                    xtype: 'b4combobox',
                    name: 'TypeJurPerson',
                    editable: false,
                    fieldLabel: 'Тип организации',
                    displayField: 'Display',
                    items: newTypeJurPerson,
                    valueField: 'Value'
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Contragent',
                    textProperty: 'Name',
                    fieldLabel: 'Организация',
                    store: 'B4.store.Contragent',
                    editable: false,
                    allowBlank: false,
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Муниципальный район', dataIndex: 'Municipality', flex: 1,
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
                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'textfield',
                    name: 'PhysicalPerson',
                    allowBlank: false,
                    fieldLabel: 'ФИО'
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