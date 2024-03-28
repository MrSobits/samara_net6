Ext.define('B4.view.housingfundmonitoring.AddWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.housingfundmonitoringaddwindow',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',

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
                    onSelectAll: function () {
                        var me = this;

                        me.setValue('All');
                        me.updateDisplayedText('Выбраны все');
                        me.selectWindow.hide();
                    },
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
                    xtype: 'combobox',
                    fieldLabel: 'Год',
                    allowBlank: false,
                    emptyText: 'Выберите год',
                    index: 'Year',
                    queryMode: 'local',
                    valueField: 'num',
                    displayField: 'name',
                    editable: false,
                    store: yearStore,
                    name: 'Year'
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