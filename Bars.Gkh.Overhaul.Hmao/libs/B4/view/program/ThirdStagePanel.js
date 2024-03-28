Ext.define('B4.view.program.ThirdStagePanel', {
    extend: 'Ext.panel.Panel',

    alias: 'widget.programthirdstagepanel',
    title: 'Долгосрочная программа',
    requires: [
        'B4.view.program.ThirdStageGrid',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.dict.municipality.ByParam'
    ],

    bodyStyle: Gkh.bodyStyle,
    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    items: [
        {
            xtype: 'container',
            layout: 'hbox',
            items: [
                {
                    xtype: 'combobox',
                    padding: '5 0 0 0',
                    store: Ext.create('B4.store.dict.municipality.ByParam', { remoteFilter: false }),
                    name: 'Municipality',
                    fieldLabel: 'Муниципальное образование',
                    labelWidth: 200,
                    width: 600,
                    labelAlign: 'right',
                    valueField: 'Id',
                    displayField: 'Name',
                    listeners: {
                        change: function (cmp, newValue) {
                            if (cmp.store.isLoading()) {
                                return;
                            }

                            cmp.store.clearFilter();
                            if (!Ext.isEmpty(newValue)) {
                                cmp.store.filter({
                                    property: 'Name',
                                    anyMatch: true,
                                    exactMatch: false,
                                    caseSensitive: false,
                                    value: newValue
                                });
                            }
                        }
                    }
                }
            ]
        },
        {
            xtype: 'programthirdstagegrid',
            flex: 1,
            border: 0
        }
    ]
});