Ext.define('B4.view.realityobj.decision.JobYear', {
    extend: 'Ext.form.Panel',

    alias: 'widget.jobyeardecision',

    requires: [
        'Ext.grid.plugin.CellEditing'
    ],

    entity: 'JobYearDecision',
    disabled: true,

    minHeight: 130,

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    items: [
        {
            xtype: 'container',
            defaults: {
                flex: 1
            },
            layout: {
                type: 'hbox',
                align: 'stretch'
            },
            items: [
                {
                    xtype: 'hidden',
                    name: 'Id',
                    flex: 0
                },
                {
                    xtype: 'checkbox',
                    specialacc: true,
                    disabled: true,
                    name: 'IsChecked',
                    margin: '0 0 0 10',
                    boxLabel: 'Перечень услуг и/или работ по КР'
                }
            ]
        },
        {
            col: 'right',
            xtype: 'grid',
            margin: '5 5 5 5',
            flex: 1,
            store: {
                fields: [
                    {
                        name: 'Job',
                        sortType: function (v) {
                            return v ? v.Name : v;
                        }
                    },
                    { name: 'PlanYear'},
                    { name: 'UserYear'}
                ]
            },
            columns: [
                {
                    text: 'Наименование',
                    flex: 1,
                    dataIndex: 'Job',
                    renderer: function (v) {
                        return v ? v.Name : v;
                    }
                },
                {
                    text: 'Установленная норма',
                    flex: 1,
                    dataIndex: 'PlanYear'
                },
                {
                    text: 'Принятое решение',
                    flex: 1,
                    dataIndex: 'UserYear',
                    editor: {
                        xtype: 'numberfield',
                        hideTrigger: true
                    }
                }
            ],
            plugins: [
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing',
                    listeners: {
                        edit: function(editor, e) {
                            var userYear = e.record.get('UserYear'),
                                planYear = e.record.get('PlanYear');

                            if (userYear > planYear) {
                                Ext.Msg.alert({
                                    title: 'Ошибка ввода данных',
                                    msg: 'Значение в поле "Принятое решение" должно быть меньше значения поля "Установленная норма".',
                                    buttons: Ext.Msg.OK,
                                    icon: Ext.window.MessageBox.ERROR
                                });

                                e.record.set(e.field, "");
                            }
                        }
                    }
                })
            ]
        }
    ],
    

    setValues: function (v) {
        var form = this,
            store;
        if (v) {
            store = form.down('grid').getStore();
            form.getForm().setValues(v);
            if (Ext.isArray(v.JobYears)) {
                store.removeAll();
                store.add(v.JobYears);
            }
        }
    },

    getValues: function () {
        var form = this,
            store = form.down('grid').getStore(),
            values = form.getForm().getValues(),
            jobYears = [];
        store.each(function (rec) {
            jobYears.push(rec.getData());
        });
        values.JobYears = jobYears;
        return values;
    }
});