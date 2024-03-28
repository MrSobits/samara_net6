Ext.define('B4.view.administration.ImportExportPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.importexportpanel',
    
    requires: ['B4.form.SelectField', 'B4.form.FileField', 'B4.view.administration.ImportExportLogGrid'],
    
    bodyStyle: Gkh.bodyStyle,
    title: 'Импорт/экспорт данных системы',
    bodyPadding: '5',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            items: [
                {
                    xtype: 'fieldcontainer',
                    fieldLabel: 'Выбор сущностей',
                    labelWidth: 120,
                    id: 'fcEntity',
                    layout: 'hbox',
                    items: [
                        {
                            xtype: 'b4selectfield',
                            store: Ext.create('B4.store.administration.ImportExportStore'),
                            textProperty: 'Description',
                            idProperty: 'TableName',
                            selectionMode: 'MULTI',
                            width: 700,
                            margin: '0 20 0 0',
                            columns: [
                                { header: 'Наименование', flex: 1, dataIndex: 'Description', filter: { xtype: 'textfield' } }
                            ]
                        },
                        {
                            xtype: 'button',
                            text: 'Экспорт',
                            action: 'Export',
                            iconCls: 'icon-disk-upload'
                        }
                    ]
                },
                {
                    xtype: 'fieldcontainer',
                    fieldLabel: 'Файл импорта',
                    labelWidth: 120,
                    id: 'fcFile',
                    layout: 'hbox',
                    items: [
                        {
                            xtype: 'b4filefield',
                            width: 700,
                            name: 'File',
                            margin: '0 20 0 0',
                            onTrigger2Click: function () {
                                this.onClearFile();
                            },
                            initTriggers: function () {
                                if (!this.trigger1Cls) {
                                    this.trigger1Cls = this.iconClsSelectFile;
                                }
                                if (!this.trigger2Cls) {
                                    this.trigger2Cls = this.iconClsClearFile;
                                }
                            }
                        },
                        {
                            xtype: 'button',
                            text: 'Импорт',
                            action: 'Import',
                            iconCls: 'icon-database-save'
                        }
                    ]
                },
                {
                    title: 'Логи',
                    xtype: 'importexportloggrid',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});