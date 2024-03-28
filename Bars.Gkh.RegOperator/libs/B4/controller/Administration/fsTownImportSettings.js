Ext.define('B4.controller.administration.fsTownImportSettings', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.Permission',
        'B4.Ajax',
        'B4.mixins.MaskBody',
        'B4.mixins.Context',
        'B4.aspects.GkhButtonImportAspect',
        'B4.QuickMsg',
        'B4.Url'
    ],

    models: [
        'administration.fsTownImportSettings',
        'administration.fsTownImportSettingsSubData'
    ],

    stores: [
        'administration.fsTownImportSettings',
        'administration.fsTownImportSettingsSubData'
    ],

    views: [
        'B4.view.administration.fstownimportsettings.MainGrid',
        'B4.view.administration.fstownimportsettings.EditWindow',
        'B4.view.administration.fstownimportsettings.SubWindow'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'fstownimportsettingsmiangrid'
        },
        {
            ref: 'editWin',
            selector: 'fstownimportsettingseditwindow'
        },
        {
            ref: 'subWin',
            selector: 'fstownimportsettingssubwindow'
        }
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    aspects: [
        {
            xtype: 'permissionaspect',
            applyOn: {
                event: 'show',
                selector: 'fstownimportsettingsmiangrid'
            },
            permissions: [
                {
                    name: 'Import.FsGorodImportInfoSettings.Import',
                    applyTo: 'b4importbutton',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'Import.FsGorodImportInfoSettings.Export',
                    applyTo: '[name=exportBtn]',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                }
            ]
        },
        {
            /*
            *аспект для импорта
            */
            xtype: 'gkhbuttonimportaspect',
            buttonSelector: 'fstownimportsettingsmiangrid b4importbutton',
            codeImport: 'FsGorodImportInfoSettings',
            loadImportList: false
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'fsTownImportSettingsGridWindowAspect',
            gridSelector: 'fstownimportsettingsmiangrid',
            editFormSelector: 'fstownimportsettingseditwindow',
            modelName: 'administration.fsTownImportSettings',
            editWindowView: 'B4.view.administration.fstownimportsettings.EditWindow',
            listeners: {
                aftersetformdata: function (asp, record) {
                    var win = asp.controller.getEditWin();
                    
                    if (parseInt(record.getId())) {
                        win.down('[name=top]').setDisabled(false);
                        win.down('[name=top]').getStore().load();
                        win.down('[name=data]').setDisabled(false);
                        win.down('[name=data]').getStore().load();
                    } else {
                        win.down('[name=top]').setDisabled(true);
                        win.down('[name=data]').setDisabled(true);
                    }
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'topfsTownImportSettingsGridWindowAspect',
            gridSelector: 'fstownimportsettingseditwindow [name=top]',
            editFormSelector: '#headerWindow',
            modelName: 'administration.fsTownImportSettingsSubData',
            editWindowView: 'B4.view.administration.fstownimportsettings.SubWindow',
            listeners: {
                beforesave: function (asp, rec) {
                    rec.set('Regex', Ext.String.htmlEncode(rec.get('Regex')));
                    rec.set('ImportInfo', asp.controller.getEditWin().down('hiddenfield').getValue());
                    rec.set('IsMeta', true);
                },
                beforesetformdata: function (asp, rec) {
                    rec.set('Regex', Ext.String.htmlDecode(rec.get('Regex')));
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'datafsTownImportSettingsGridWindowAspect',
            gridSelector: 'fstownimportsettingseditwindow [name=data]',
            editFormSelector: '#dataWindow',
            modelName: 'administration.fsTownImportSettingsSubData',
            editWindowView: 'B4.view.administration.fstownimportsettings.SubWindow',
            listeners: {
                beforesave: function (asp, rec) {
                    rec.set('Regex', Ext.String.htmlEncode(rec.get('Regex')));
                    rec.set('ImportInfo', asp.controller.getEditWin().down('hiddenfield').getValue());
                },
                beforesetformdata: function (asp, rec) {
                    rec.set('Regex', Ext.String.htmlDecode(rec.get('Regex')));
                }
            },
            getForm: function () {
                var me = this,
                    editWindow;

                if (me.editFormSelector) {
                    editWindow = me.componentQuery(me.editFormSelector);

                    if (editWindow && !editWindow.getBox().width) {
                        editWindow = editWindow.destroy();
                    }

                    if (!editWindow) {

                        editWindow = me.controller.getView(me.editWindowView).create(
                            {
                                constrain: true,
                                renderTo: B4.getBody().getActiveTab().getEl(),
                                closeAction: 'destroy',
                                ctxKey: me.controller.getCurrentContextKey ? me.controller.getCurrentContextKey() : '',
                                phantomColumn: true
                            });

                        editWindow.show();
                    }

                    return editWindow;
                }
            }
        }
    ],

    init: function () {
        var me = this;
        me.control({
            'fstownimportsettingsmiangrid': {
                'selectionchange': me.onMainGridSelectChange
            },
            'fstownimportsettingsmiangrid [name=exportBtn]': {
                'click': {
                    fn: me.onExportBtnClick,
                    scope: me
                }
            },
            'fstownimportsettingssubwindow [name=PropertyName]': {
                'change': {
                    fn: me.onChangePropertyName,
                    scope: me
                }
            }
        });
        me.callParent(arguments);
    },

    onChangePropertyName: function (field, value) {
        var window = field.up('fstownimportsettingssubwindow'),
            paymentAgentField;

        if (window) {
            paymentAgentField = window.down('b4selectfield[name=PaymentAgent]');

            if (paymentAgentField) {
                // если Поле пусто или не равно AgentId, то очищаем предыдущее значение Кода платежного агента и ставим Disabled
                if (Ext.isEmpty(value) || value !== "AgentId") {
                    paymentAgentField.setDisabled(true);
                    paymentAgentField.setValue(null);
                } else {
                    paymentAgentField.setDisabled(false);
                }
            }
        }
    },

    onMainGridSelectChange: function (selModel, selected) {
        var grid = this.getMainView();
        grid.down('[name=exportBtn]').setDisabled(selected.length === 0);
    },

    onExportBtnClick: function (btn) {
        var me = this,
            grid = btn.up('fstownimportsettingsmiangrid'),
            record = grid.getSelectionModel().getSelection()[0];

        me.mask('Экспорт',grid);

        B4.Ajax.request({
            url: B4.Url.action('Export', 'FsGorodImportInfo', { id: record.get('Id') })
        }).next(function (resp) {
            var blob = new Blob([Ext.JSON.decode(resp.responseText)], { type: "application/json" });
            var url = window.URL.createObjectURL(blob);
            var a = document.createElement("a");
            a.style = "display: none";
            document.body.appendChild(a);
            a.href = url;
            a.download = record.get('Name').replace(/\./g, '') + '.json';
            a.addEventListener('click', function () {
                setTimeout(function () {
                    window.URL.revokeObjectURL(url);
                }, 100);
            });
            a.click();
            me.unmask();
            grid.getStore().load();
        }).error(function (resp) {
            me.unmask();
            B4.QuickMsg.msg('Ошибка импорта!', resp.message, 'error');
        });
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('fstownimportsettingsmiangrid');

        me.bindContext(view);
        me.application.deployView(view);
    }
});