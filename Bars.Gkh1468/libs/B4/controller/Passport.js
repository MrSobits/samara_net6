Ext.define('B4.controller.Passport', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.mixins.Context',
        'B4.view.passport.Editor',
        'B4.dynamic.Helper',
        'B4.Ajax', 'B4.Url',
        'B4.mixins.MaskBody'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    views: ['passport.Editor'],

    refs: [
        {
            ref: 'okiPasspEditor',
            selector: 'passpeditor[passptype="oki"]'
        },
        {
            ref: 'housePasspEditor',
            selector: 'passpeditor[passptype="house"]'
        },
        {
            ref: 'partTree',
            selector: 'passpeditor treepanel[treetype="parttree"]'
        },
        {
            ref: 'attrValEditor',
            selector: 'passpeditor form[formtype="attrvaleditor"]'
        }
    ],

    okipasspeditor: function (id) {
        var me = this,
            view = me.getOkiPasspEditor() || Ext.widget('passpeditor', {
                controllerName: 'OkiProviderPassportRow',
                passportControllerName: 'OkiProviderPassport',
                passptype: 'oki',
                passpId: id
            });

        me.bindContext(view);
        me.application.deployView(view);
    },

    loadData: function (panel, passpId, partId) {
        var me = this, editor;

        editor = panel.up('passpeditor');

        me.mask('Загрузка данных', panel);
        B4.Ajax.request({
            url: B4.Url.action('GetMetaValues', editor.controllerName),
            method: 'GET',
            // Некоторые разделы содержат много данных, объем которых превышает 3-4 МБ
            // и в случае слабого канала связи не успевают за 30 сек скачаться клиенту
            timeout: 300000,
            params: { providerPassportId: editor.passpId, partId: partId, metaId: panel.metaId }
        }).next(function (response) {
            var json = Ext.JSON.decode(response.responseText);
            me.bindData(panel, json.data);

            me.unmask();
        })
        .error(function () {
            me.unmask();
            throw new Error('Request error');
        });
    },

    housepasspeditor: function (id) {
        var me = this,
            view = me.getHousePasspEditor() || Ext.widget('passpeditor', {
                controllerName: 'HouseProviderPassportRow',
                passportControllerName: 'HouseProviderPassport',
                passptype: 'house',
                passpId: id
            });

        me.bindContext(view);
        me.application.deployView(view);
    },

    init: function () {
        this.callParent(arguments);
        var me = this,
            actions = {
                'passpeditor[passptype]': {
                    render: me.onEditorRender
                },
                'passpeditor treepanel[treetype="parttree"]': {
                    selectionchange: me.onPartTreeSelect
                },
                'passpeditor form button[cmd="save"]': {
                    click: me.onEditorSave
                },
                'passpeditor button[action="delete"]': {
                    click: me.onMultipleItemRemove,
                    render: function (c) {
                        c.setDisabled(me.readOnly);
                    }
                },
                'panel[type="multy"] button[action="add"]': {
                    click: me.addMultyValue,
                    render: function(c) {
                        c.setDisabled(me.readOnly);
                    }
                },
                'panel[type="multy"] button[action="back"]': {
                    click: function (sender) {
                        var panel = sender.up('panel[type = "multy"]');
                        panel.changeValue('back');
                    }
                },
                'panel[type="multy"] button[action="next"]': {
                    click: function (sender) {
                        var panel = sender.up('panel[type = "multy"]');
                        panel.changeValue('next');
                    }
                },
                'panel[type="multy"] numberfield[dataIndex=CurentRow]': {
                    change: function (e, t) {
                        me.changeMultyPanelValue(e, 'set', t);
                    },
                    specialkey: function (e, t) {
                        if (t.getKey() != 13) {
                            return;
                        }

                        me.changeMultyPanelValue(e, 'no');
                    }
                },
                'panel[type="multy"]': {
                    beforeexpand: me.beforeExpandMulty
                },
                'field[metaId]': {
                    change: function (sender, newValue) {
                        var editor;

                        if (sender.isValid() && sender.oldValue != newValue) {
                            editor = this.getCmpInContext('passpeditor');
                            editor.valueIsChange = true;
                            sender.valueIsChange = true;
                        }
                    }
                }
            };
        me.control(actions);
    },

    changeMultyPanelValue: function (sender, direction, newValue) {
        var me = this;
        var panel = sender.up('panel[type = "multy"]'),
            editor = this.getCmpInContext('passpeditor'),
            doChange = function () {
                var value = panel.changeValue(direction, newValue);
                if (value > 0) {
                    panel.loadMultyMetaData();
                }
            };
        if (editor.valueIsChange && !me.readOnly) {
            Ext.Msg.confirm('Вопрос', 'Имеются не сохраненые данные.<br/>' +
                'Перед переходом на другую запись необходимо сохранить изменения.<br/>' +
                'Да - сохранить и перейти на другую запись.</br>' +
                'Нет - перейти на другую запись без сохранения.</br>',
                function (btn) {
                    if (btn == 'yes') {
                        var req = me.onEditorSave(sender);
                        if (req) {
                            req.next(function () {
                                doChange();
                            });
                        }
                    } else {
                        me.resetChangeFlags(sender);
                        doChange();
                    }
                });

            return;
        }

        doChange();
    },

    resetChangeFlags: function (sender) {
        var form = sender.up('form'),
            editor = form.up('[passptype]'),
            fields = Ext.ComponentQuery.query('[valueIsChange=true]', form);

        Ext.each(fields, function (field) {
            field.valueIsChange = false;
        });

        editor.valueIsChange = false;
    },

    addMultyValue: function (sender) {
        var panel = sender.up('panel[type="multy"]');
        panel.addRecord();
    },

    beforeExpandMulty: function (sender) {
        var me = this,
            editor;

        if (!sender.isLoaded) {
            editor = this.getCmpInContext('passpeditor');
            me.mask('Загрузка данных', sender);
            B4.Ajax.request({
                url: B4.Url.action('List', editor.controllerName),
                method: 'POST',
                params: {
                    providerPassportId: editor.passpId,
                    partId: editor.partId,
                    metaId: sender.metaId
                }
            }).next(function (response) {
                var json, meta;
                json = Ext.decode(response.responseText);
                meta = json.data.meta;
                if (meta) {
                    B4.dynamic.Helper.addMultyItems(sender, meta);
                }
                me.unmask();

            })
            .next(function () {
                sender.loadMultyMetaData();
            })
            .error(function () {
                me.unmask();
            });
        }
    },

    onEditorRender: function (editor) {
        var permissions = [],
            me = this;

        if (editor.passptype == 'oki') {
            editor.down('b4selectfield[name="RealityObject"]').hide();
            permissions = ['Gkh1468.Passport.MyOki.Edit'];
        } else if (editor.passptype == 'house') {
            permissions = ['Gkh1468.Passport.MyHouse.Edit'];
        }

        B4.Ajax.request({
            url: B4.Url.action('GetObjectSpecificPermissions', 'Permission'),
            method: 'POST',
            params: {
                ids: Ext.encode([editor.passpId]),
                permissions: Ext.encode(permissions)
            }
        }).next(function (response) {
            var perm = Ext.decode(response.responseText)[0];
            me.readOnly = !perm[0];
            editor.down('form button[cmd="save"]').setDisabled(me.readOnly);
        }).error(function () {
            me.readOnly = true;
            editor.down('form button[cmd="save"]').setDisabled(true);
        });

        me.mask('Загрузка...', editor);

        B4.Ajax.request({
            url: B4.Url.action('Get', editor.passportControllerName),
            method: 'GET',
            params: { id: editor.passpId }
        }).next(function (response) {
            var json = Ext.JSON.decode(response.responseText);

            if (json.data) {
                if (json.data.Municipality) {
                    editor.down('[name="Municipality"]').setValue(json.data.Municipality);
                }
                if (json.data.RealityObject) {
                    editor.down('[name="RealityObject"]').setValue(json.data.RealityObject);
                }
                if (json.data.ReportYear) {
                    editor.down('[name="ReportYear"]').setValue(json.data.ReportYear);
                }
                if (json.data.ReportMonth) {
                    editor.down('[name="ReportMonth"]').setValue(json.data.ReportMonth);
                }
                if (json.data.Contragent) {
                    editor.down('[name="Contragent"]').setValue(json.data.Contragent);
                }
            }
            editor.down('form').getForm().loadRecord(json.data);
        });

        B4.Ajax.request({
            url: B4.Url.action('List', editor.controllerName),
            method: 'GET',
            params: { providerPassportId: editor.passpId, isPart: true }
        }).next(function (response) {
            var json = Ext.JSON.decode(response.responseText),
                newRoot = {},
                root;

            root = json.data.meta;
            newRoot.Childrens = Ext.Array.map(root.Childrens, function (item) {
                var child = item.Part;
                child.leaf = item.Leaf;

                child.Childrens = Ext.Array.map(item.Childrens, function (ch) {
                    var c = ch.Part;
                    c.attributes = ch.Childrens;
                    c.Value = ch.Value;
                    c.ValueId = ch.ValueId;
                    c.leaf = ch.Leaf;

                    return c;
                });

                return child;
            });

            editor.down('treepanel[treetype="parttree"]').setRootNode(newRoot);
            me.unmask();
        }).error(function () {
            me.unmask();
            throw new Error('Request error');
        });
    },

    onPartTreeSelect: function (panel, selection) {
        var me = this,
            attributes,
            valEditor = me.getAttrValEditor(),
            editor = valEditor.up('passpeditor'),
            partId = 0;


        if (selection.length > 0) {
            partId = selection[0].get("Id");
            if (partId <= 0) {
                return;
            }
        }

        editor.partId = partId;

        me.mask('Загрузка...', editor);

        B4.Ajax.request({
            url: B4.Url.action('List', editor.controllerName),
            method: 'GET',
            // Некоторые разделы содержат много данных, объем которых превышает 3-4 МБ
            // и в случае слабого канала связи не успевают за 30 сек скачаться клиенту
            timeout: 300000,
            params: { providerPassportId: editor.passpId, partId: partId }
        }).next(function (response) {
            var meta,
                json = Ext.JSON.decode(response.responseText);

            meta = json.data.meta;
            valEditor.removeAll();

            if (meta && meta.Childrens) {
                attributes = me.flattenAttribute(meta.Childrens);
                valEditor.add(B4.dynamic.Helper.getItems(attributes, {
                    controllerName: editor.controllerName,
                    passpId: editor.passpId,
                    partId: partId
                }));
            }
            me.unmask();
        })
        .next(function () {
            me.loadData(valEditor, editor.passpId, partId);
        })
        .error(function () {
            me.unmask();
            throw new Error('Request error');
        });
    },

    flattenAttribute: function (childrens) {
        var me = this;
        return Ext.Array.map(childrens, function (child) {
            var attr = child.Attribute;
            attr.Childrens = me.flattenAttribute(child.Childrens);
            if (child.ChildConfig) {
                attr.ChildConfig = me.flattenAttribute(child.ChildConfig);
            }
            return attr;
        });
    },

    bindData: function (panel, data) {
        Ext.each(data, function (row) {
            var editors;

            editors = Ext.ComponentQuery.query(Ext.String.format('[metaId={0}]', row.MetaId), panel);
            if (editors[0]) {
                editors[0].suspendEvents();
                if (editors[0].type == 'multy') {
                    editors[0].valueId = row.ValueId;
                    editors[0].parentValue = row.ParentValue;
                    editors[0].oldValue = row.ValueId;
                    editors[0].setValue(row.ValueId);
                } else {
                    editors[0].valueId = row.ValueId;
                    editors[0].parentValue = row.ParentValue;
                    editors[0].oldValue = row.Value;
                    editors[0].setValue(row.Value);
                }
                editors[0].resumeEvents();
            }
        });
    },

    onEditorSave: function (btn) {
        var form = btn.up('form'),
            fields,
            records = [],
            editor;

        editor = form.up('[passptype]');

        if (!form.getForm().isValid()) {
            B4.QuickMsg.msg('Внимание', 'Имеются ошибки заполнения формы', 'error');
            return;
        }

        fields = Ext.ComponentQuery.query('[valueIsChange=true]', form);

        Ext.each(fields, function (field) {
            var record = {
                ProviderPassport: editor.passpId,
                MetaAttribute: field.metaId,
                Value: field.getValue(),
                ParentValue: field.parentValue ? field.parentValue.toString() : null,
                FieldId: field.getId()
            };

            if (field.valueId) {
                record.Id = field.valueId;
            }

            records.push(record);
        });

        if (records.length > 0) {
            var myMask = new Ext.LoadMask(editor, { msg: "Сохранение..." });
            myMask.show();
            return B4.Ajax.request({
                url: B4.Url.action('SaveRecord', editor.controllerName),
                method: 'POST',
                params: { records: Ext.encode(records) }
            }).next(function (resp) {
                var data = Ext.JSON.decode(resp.responseText);

                myMask.hide();
                myMask.destroy();
                editor.valueIsChange = false;

                Ext.each(fields, function (field) {
                    field.valueId = data[field.getId()];
                    field.valueIsChange = false;
                });

                B4.QuickMsg.msg('Сообщение', 'Сохранение прошло успешно', 'success');
            }).error(function () {
                myMask.hide();
                myMask.destroy();
                B4.QuickMsg.msg('Сообщение', 'Произошла ошибка при сохранении', 'error');
            });
        } else {
            B4.QuickMsg.msg('Сообщение', 'Сохранение прошло успешно', 'success');
        }
    },
    /**
     * Удаление значений из групп-множ атрибута
     */
    onMultipleItemRemove: function (btn) {
        var panel = btn.up('[type="multy"]');
        var editor = this.getCmpInContext('passpeditor');
        var value = panel.getValue();
        if (value) {
            B4.Ajax.request({
                url: B4.Url.action('DeleteMultyMetaValues', editor.controllerName),
                method: 'POST',
                params: {
                    valueId: value
                }
            }).next(function (response) {
                var obj = Ext.decode(response.responseText);

                if (obj.success) {
                    B4.QuickMsg.msg('Сообщение', 'Удаление прошло успешно');
                    panel.deleteCurrentValue();
                } else {
                    B4.QuickMsg.msg('Сообщение', obj.message ? obj.message : 'Произошла ошибка при удалении', 'error');
                }
            }).error(function () {
                B4.QuickMsg.msg('Сообщение', 'Произошла ошибка при удалении', 'error');
            });
        } else {
            panel.deleteCurrentValue();
            B4.QuickMsg.msg('Сообщение', 'Удаление прошло успешно');
        }
    }
});