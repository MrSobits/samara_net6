Ext.define('B4.view.fuelinfo.AddWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.fuelinfoaddwindow',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.Month',

        'B4.form.SelectField',
        'B4.store.dict.municipality.ListByParamAndOperator'
    ],

    layout: 'form',
    modal: true,
    width: 500,
    bodyPadding: 5,
    title: 'Добавить новый',

    initComponent: function () {
        var me = this;

        var yearStore = Ext.create('Ext.data.Store', {
            fields: ['num', 'name']
        });

        var lastYear = new Date().getFullYear() - 1;
        var endYear = lastYear + 10;

        while (lastYear < endYear) {
            yearStore.add({ num: lastYear, name: lastYear.toString() });
            lastYear++;
        }

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    fieldLabel: 'Муниципальное образование',
                    name: 'Municipality',
                    store: 'B4.store.dict.municipality.ListByParamAndOperator',
                    editable: false,
                    allowBlank: false,
                    selectionMode: 'MULTI',
                    columns: [
                        {
                            text: 'Наименование', dataIndex: 'Name', flex: 1,
                            filter: {
                                xtype: 'textfield'
                            }
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '7 0 10 2',
                    defaults: {
                        labelWidth: 200,
                        flex: 1
                    },
                    layout: 'hbox',
                    items: [
                        {
                            xtype: 'label',
                            text: 'Отчетный период:'
                        },
                        {
                            xtype: 'combobox',
                            fieldLabel: 'Год',
                            hideLabel: true,
                            allowBlank: false,
                            emptyText: 'Выберите год',
                            index: 'Year',
                            queryMode: 'local',
                            valueField: 'num',
                            displayField: 'name',
                            editable: false,
                            store: yearStore,
                            name: 'Year'
                        },
                        {
                            xtype: 'combobox',
                            fieldLabel: 'Месяц',
                            hideLabel: true,
                            allowBlank: false,
                            emptyText: 'Выберите месяц',
                            valueField: 'Value',
                            displayField: 'Display',
                            editable: false,
                            store: B4.enums.Month.getStore(),
                            name: 'Month',
                            margin: '0 0 0 5'
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