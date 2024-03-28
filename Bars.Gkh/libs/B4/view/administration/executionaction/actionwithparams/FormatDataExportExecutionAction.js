Ext.define('B4.view.administration.executionaction.actionwithparams.FormatDataExportExecutionAction', {
    extend: 'Ext.form.Panel',
    requires: [
        'B4.form.SelectField'
    ],

    border: false,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.base.Store',
            {
                fields: ['Code', 'Description'],
                autoLoad: false,
                pageSize: 100,
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'FormatDataExport',
                    listAction: 'ListAvailableCodes'
                },
                sorters: [{
                    property: 'Code',
                    direction: 'ASC'
                }]
            });

        Ext.applyIf(me, {
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            defaults: {
                labelWidth: 150
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    fieldLabel: 'Экспортируемые секции',
                    selectionMode: 'MULTI',
                    name: 'EntityCodes',
                    idProperty: 'Code',
                    textProperty: 'Description',
                    editable: false,
                    store: store,
                    columns: [
                        {
                            dataIndex: 'Code',
                            header: 'Код',
                            width: 150,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            dataIndex: 'Description',
                            header: 'Описание',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    onSelectAll: function () {
                        var me = this;

                        me.setValue(null);
                        me.updateDisplayedText('Выбраны все');
                        me.selectWindow.hide();
                    },
                },
                {
                    xtype: 'checkbox',
                    name: 'NoEmptyMandatoryFields',
                    fieldLabel: 'Не выгружать секции с ошибками',
                    labelWidth: 321,
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});