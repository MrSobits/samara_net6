Ext.define('B4.controller.GkhConfig', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.ux.grid.Panel',
        'B4.ux.config.ObjectEditor',
        'B4.ux.config.CollectionEditor',
        'B4.ux.config.DictionaryEditor',
        'B4.ux.config.EnumEditor',
        'B4.ux.config.RawEditor',
        'B4.utils.config.Helper',
        'B4.ux.config.GenericCellEditing',
        'B4.ux.config.GenericEditorColumn',
        'B4.ux.button.Add',
        'B4.ux.button.Delete',
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.ux.button.Update'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        loader: 'B4.mixins.LayoutControllerLoader',
        context: 'B4.mixins.Context'
    },

    views: [
        'config.NavigationPanel',
        'config.Panel'
    ],

    mainView: 'config.Panel',
    mainViewSelector: 'configpanel',

    refs: [
        {
            ref: 'dynamicPanel',
            selector: '#dynamicPanel'
        },
        {
            ref: 'navigationPanel',
            selector: 'confignavigationpanel'
        },
        {
            ref: 'rootsGrid',
            selector: 'confignavigationpanel treepanel'
        },
        {
            ref: 'wrapperPanel',
            selector: '#wrapperPanel'
        }
    ],

    init: function() {
        var me = this;

        me.callParent(arguments);

        this.control({
            'confignavigationpanel treepanel': {
                'select': {
                    fn: me.onRootSelected,
                    scope: me
                }
            },
            'configpanel b4savebutton[name=configpanelb4savebutton]': {
                'click': {
                    fn: me.onSave,
                    scope: me
                }
            },
            'configpanel b4updatebutton[name=configpanelb4updatebutton]': {
                'click': {
                    fn: me.onReload,
                    scope: me
                }
            }
        });
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('configpanel');

        me.bindContext(view);
        me.application.deployView(view);

        me.loadRoots();
    },

    loadRoots: function() {
        var me = this;

        me.mask('Загрузка');
        B4.Ajax.request({
                url: B4.Url.action('ListRoots', 'GkhConfig')
            })
            .next(function(resp) {
                var panel = me.getNavigationPanel(),
                    response = Ext.decode(resp.responseText);
                panel.store.setRootNode(response);
                me.unmask();
            })
            .error(function() {
                me.unmask();
            });
    },

    onRootSelected: function(_, rec) {
        var me = this,
            panel = me.getDynamicPanel(),
            wrapperPanel = me.getWrapperPanel();

        me.mask('Загрузка');

        wrapperPanel.disable();
        panel.removeAll(true);
        B4.Ajax.request({
                url: B4.Url.action('ListItems', 'GkhConfig'),
                params: {
                    parent: rec.raw.id
                }
            })
            .next(function(resp) {
                var response = Ext.decode(resp.responseText);
                wrapperPanel.setTitle(rec.get('text'));
                if (response.data && response.data.length > 0) {
                    panel.add(B4.utils.config.Helper.getItems(response.data));
                    wrapperPanel.enable();
                }

                me.unmask();
            })
            .error(function() {
                me.unmask();
            });
    },

    onReload: function() {
        var me = this,
            grid = me.getRootsGrid(),
            sm = grid.getSelectionModel(),
            selected = sm.getSelection();

        if (selected.length == 1) {
            me.onRootSelected(null, selected[0]);
        }
    },

    onSave: function() {
        var me = this,
            dp = me.getDynamicPanel(),
            form = dp.getForm(),
            values = form.getValues(false, true, false, true),
            keys = Ext.Object.getKeys(values),
            result = {};

        if (!form.isValid()) {
            Ext.Msg.alert('Ошибка', 'Не все поля формы заполнены корректно');
            return false;
        }

        Ext.iterate(values, function (k, v) {
            result[k] = Ext.encode(v);
        });

        me.mask('Загрузка');

        B4.Ajax.request({
            url: B4.Url.action('SaveAppConfig', 'GkhConfig'),
            method: 'POST',
            params: {
                configs: Ext.encode(result)
            }
        })
        .next(function (resp) {
            me.onAfterSave(form, keys, resp);
            me.unmask();
        })
        .error(function (resp) {
            me.onAfterSave(form, keys, resp);
            me.unmask();
        });
    },

    onAfterSave: function(form, keys, resp) {
        var response = Ext.decode(resp.responseText),
            message = response ? response.message : null,
            errors = response ? response.errors : null,
            failedFields = [];

        if ((!errors || Ext.Object.getSize(errors) == 0) && (!response || !response.success)) {
            Ext.each(keys, function(k) {
                var field = form.findField(k);
                if (field) {
                    field.markInvalid(message || 'Не удалось сохранить параметр');
                }
            });
            Ext.Msg.alert('Ошибка', message || 'Произошла ошибка при сохранении параметров');
            return;
        }

        Ext.each(keys, function(k) {
            var field = form.findField(k);
            if (field) {
                if (!errors.hasOwnProperty(k)) {
                    field.initValue();
                } else {
                    failedFields.push(errors[k]);
                    field.markInvalid(errors[k]);
                }
            }
        });

        if (failedFields.length == 0) {
            B4.QuickMsg.msg('Сохранение', 'Параметры успешно сохранены', 'success');
        } else {
            Ext.Msg.alert('Сохранение', 'Часть параметров не удалось сохранить:<br/>' + failedFields.join('<br/>'));
        }
    }
});