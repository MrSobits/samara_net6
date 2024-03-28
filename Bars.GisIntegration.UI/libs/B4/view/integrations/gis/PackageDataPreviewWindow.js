Ext.define('B4.view.integrations.gis.PackageDataPreviewWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.xmldatapreviewwindow',
    modal: true,
    width: 550,
    height: 405,
    bodyPadding: 5,
    closable: true,
    maximizable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    title: 'Просмотр',
    xmlData: undefined,

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    border: false,
                    items: [
                        {
                            xtype: 'panel',
                            border: false,
                            title: 'xml представление',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'fit',
                                    flex: 1,
                                    padding: '5 0 0 0',
                                    items: [
                                        {
                                            xtype: 'textareafield',
                                            grow: true,
                                            name: 'xmlData',
                                            flex: 1,
                                            readOnly: true
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.on('afterrender',
            function () {
                var me = this;

                if (me.xmlData && me.xmlData.length > 0) {
                    me.down('textareafield').setValue(me.xmlData);
                }
            },
            me);

        me.callParent(arguments);
    }
});
