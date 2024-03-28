Ext.define('B4.view.instructions.DataView', {
    extend: 'Ext.tree.Panel',
    alias: 'widget.instructionsview',
    requires: [
        'B4.ux.button.Update'
    ],
    rootVisible: false,
    useArrows: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('Ext.data.TreeStore', {
            autoLoad:true,
            fields: ['Id', 'DisplayName', 'IsInstruction', 'File'],
            proxy: {
                type: 'ajax',
                url: B4.Url.action('/InstructionGroup/GetTree'),
                reader: {
                    type: 'json'
                },
                root: {
                    text: 'root',
                    expanded: true
                }
            }
        });
        Ext.applyIf(me, {
            store: store,
            loader: {
                autoLoad: false
            },
            viewConfig: {
                loadMask: true
            },
                hideHeaders: true,
                columns: [
                    {
                        xtype: 'treecolumn',
                        dataIndex: 'DisplayName',
                        flex: 1
                    },
                    {
                        xtype: 'actioncolumn',
                        width: 20,
                        tooltip: 'Загрузить',
                        itemId: 'instructionDlColumn',
                        renderer: function (value, meta, record) {
                            var col = this;
                            col.icon = !record.get('IsInstruction') ? '' : B4.Url.content('content/img/icons/disk_download.png');
                            return value;
                        },
                        handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                            if (!rec.get('IsInstruction'))
                                return;
                            
                            var param = { id: rec.get('File').Id };

                            var urlParams = Ext.urlEncode(param);

                            var newUrl = Ext.urlAppend('/FileUpload/Download?' + urlParams, '_dc=' + (new Date().getTime()));
                            newUrl = B4.Url.action(newUrl);

                            Ext.DomHelper.append(document.body, {
                                tag: 'iframe',
                                id: 'downloadIframe',
                                frameBorder: 0,
                                width: 0,
                                height: 0,
                                css: 'display:none;visibility:hidden;height:0px;',
                                src: newUrl
                            });
                        }
                    }
                ],
                dockedItems: [
                    {
                        xtype: 'toolbar',
                        dock: 'top',
                        items: [
                            {
                                xtype: 'buttongroup',
                                items: [
                                    {
                                        xtype: 'b4updatebutton',
                                        listeners: {
                                            'click': function () {
                                                store.load();
                                            }
                                        }
                                    }
                                ]
                            }
                        ]
                    }
                ]
            }
        );
        
        me.callParent(arguments);
    }
});