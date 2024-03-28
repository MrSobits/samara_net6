Ext.define('B4.view.PreviewFileWindow', {
    extend: 'Ext.window.Window',
    alias: 'widget.previewFileWindow',

    amdModules: ['B4/CondExpr', 'require/css!content/css/CommonUiMain'],

    requires: [
        'B4.mixins.MaskBody',
        'B4.ux.button.Close'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    layout: {
        type: 'fit',
        align: 'stretch'
    },

    width: 800,
    height: 600,

    title: 'Просмотр файла',

    constrain: true,

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: {
                xtype: 'component',
                autoEl: {
                    tag: 'iframe',
                    style: 'height: 100%; width: 100%; border: none;',
                    src: B4.Url.action('/FilePreview/PreviewFile?id=' + me.fileId)
                },
                listeners: {
                    load: {
                        element: 'el',
                        fn: function () {
                            this.parent().unmask();
                        }
                    },
                    render: function () {
                        this.up('window').body.mask('Загрузка...');
                    }
                }
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Скачать',
                                    name: 'Save',
                                    iconCls: 'icon-page-save'
                                }
                            ]
                        },
                        '->',
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    handler: function (btn) {
                                        btn.up('window').close();
                                    }
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