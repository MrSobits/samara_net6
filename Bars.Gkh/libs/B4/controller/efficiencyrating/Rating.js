Ext.define('B4.controller.efficiencyrating.Rating', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.GkhEditPanel',
        'B4.enums.efficiencyrating.DataMetaObjectType',

        'B4.utils.config.Helper'
    ],

    models: [
        'B4.model.efficiencyrating.ManagingOrganization',
        'B4.model.Contragent'
    ],

    views: ['efficiencyrating.manorg.EditPanel'],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'efManorgEditPanel'
        },
        {
            ref: 'treePanel',
            selector: 'efManorgFactorTreePanel'
        },
        {
            ref: 'efAttributeEditPanel',
            selector: 'efAttributeEditPanel'
        }
    ],

    mainView: 'efficiencyrating.manorg.EditPanel',
    mainViewSelector: 'efManorgEditPanel',

    aspects: [
        {
            xtype: 'gkheditpanel',
            name: 'efficiencyratingEditPanelAspect',
            editPanelSelector: 'efManorgEditPanel',
            modelName: 'B4.model.efficiencyrating.ManagingOrganization',
            listeners: {
                aftersetpaneldata: function(asp, rec, panel) {
                    var me = this,
                        managingOrganization = rec.get('ManagingOrganization'),
                        contragentId = managingOrganization.Contragent instanceof Object ? managingOrganization.Contragent.Id : managingOrganization.Contragent;

                    if (panel.title === 'Рейтинг эффективности') {
                        B4.model.Contragent.load(contragentId, {
                            success: function (newRec) {
                                panel.setTitle('Рейтинг эффективности ' + newRec.get('ShortName'));
                            }
                        });
                    }

                    me.controller.updateTreePanel.call(me, rec, panel.down('efManorgFactorTreePanel'));
                }
            },

            otherActions: function(actions) {
                var me = this;

                // переопределяем подписку из аспекта т.к. на эту кнопку не надо сохранять основную модель
                actions[me.editPanelSelector + ' b4savebutton'] = { 'click': { fn: Ext.emptyFn, scope: me } };

                actions[me.editPanelSelector + ' b4updatebutton'] = { 'click': { fn: me.onUpdateBtnClick, scope: me } };
            },

            onUpdateBtnClick: function (btn) {
                var me = this;

                me.setData(me.controller.getCurrentId());
            },

            getFormErrorMessage: function (form) {
                //получаем все поля формы
                var fields = form.getForm().getFields();

                var errorMessage = '<b>Не заполнены обязательные поля:</b> ';
                errorMessage += '<ul>';

                //проверяем, если поле не валидно, то записиваем fieldLabel в строку инвалидных полей
                Ext.each(fields.items, function (field) {
                    if (!field.isValid()) {
                        errorMessage += Ext.String.format('<li>{0}</li>', Ext.String.htmlEncode(field.fieldLabel));
                    }
                });

                errorMessage += '</ul>';
                return errorMessage;
            },
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Orgs.EfficiencyRating.ManagingOrganization.CalcValues', applyTo: 'button[actionName=calcvalues]', selector: 'efManorgEditPanel' },
                { name: 'Gkh.Orgs.EfficiencyRating.ManagingOrganization.Edit', applyTo: 'b4savebutton', selector: 'efManorgEditPanel' }
            ]
        }
    ],

    updateTreePanel: function (rec, treePanel) {
        var me = this,
            selected = treePanel.getSelectionModel().getSelection()[0],
            selectPath;

        if (selected) {
            selectPath = selected.getPath();
        }

        me.controller.mask('Загрузка', treePanel.getView());
        Ext.Ajax.request({
            method: 'GET',
            url: B4.Url.action('GetMetaValues', 'BaseDataValue'),
            params: {
                efmanorgId: rec.getId(),
                dataMetaObjectType: B4.enums.efficiencyrating.DataMetaObjectType.EfficientcyRating
            },
            success: function (response) {
                var dataTree = Ext.JSON.decode(response.responseText);

                dataTree.Id = 0;
                treePanel.setRootNode(dataTree);
                treePanel.expandAll();

                if (selectPath) {
                    treePanel.selectPath(selectPath);
                    me.controller.onRootSelected(treePanel, treePanel.getStore().getById(selected.getId()));
                }

                me.controller.unmask();
            },
            failure: function (response) {
                var obj = Ext.decode(response.responseText);
                me.controller.unmask();

                Ext.Msg.alert('Ошибка', obj.message || 'Не удалось загрузить данные');
            }
        });
    },

    onSaveSection: function() {
        var me = this,
            panel = me.getEfAttributeEditPanel(),
            formPanel = panel.down('form[name=dynamicForm]'),
            form = formPanel.getForm(),
            treePanel = me.getTreePanel(),
            store = treePanel.getView().getStore(),
            records = [],
            formValues = form.getValues(false, true, false, true);

        if (!form.isValid()) {
            Ext.Msg.alert('Ошибка!', me.getAspect('efficiencyratingEditPanelAspect').getFormErrorMessage(formPanel));
            return;
        }

        for (var prop in formValues) {
            if (formValues.hasOwnProperty(prop)) {
                records.push({ Id: parseInt(prop), Value: formValues[prop] });
            }
        }

        if (!records.length) {
            return;
        }

        me.mask('Сохранение', panel);
        B4.Ajax.request({
                url: B4.Url.action('Update', 'BaseDataValue'),
                method: 'POST',
                params: {
                    records: Ext.encode(records)
                }
            })
            .next(function (response) {
                var dataElements = Ext.decode(response.responseText).data,
                    parentRec,
                    attributes;

                if (dataElements.length || dataElements.length > 0) {
                    parentRec = store.getById(dataElements[0].Parent.Id);
                    attributes = parentRec.get('AttributeObjects');

                    Ext.Array.each(dataElements, function (element) {
                        Ext.Array.each(attributes, function(el) {
                            if (el.id == element.Id) { el.value = element.Value; }});
                    });

                    me.onRootSelected.call(me, treePanel, parentRec);
                }

                me.unmask();
            })
            .error(function(response) {
                var obj = Ext.decode(response.responseText);
                me.unmask();
                Ext.Msg.alert('Ошибка!', obj.message || 'При выполнении запроса произошла ошибка!');
            });
    },

    onRootSelected: function(treePanel, records) {
        var me = this,
            panel = me.getEfAttributeEditPanel(),
            record = records.length ? records[0] : records,
            dynamicForm = panel.down('form[name=dynamicForm]'),
            data = record ? record.get('AttributeObjects') : null;

        if (!record || !record.isLeaf()) {
            dynamicForm.removeAll(true);
            return;
        }

        panel.disable();
        dynamicForm.removeAll(true);

        if (data) {
            dynamicForm.add(B4.utils.config.Helper.getItems(data));
            dynamicForm.getForm().isValid();
            panel.enable();
        }
    },

    onCalcBtnClick: function() {
        var me = this,
            efmanorgId = me.getCurrentId(),
            view = me.getMainView();

        me.mask('Расчёт показателей', view);
        B4.Ajax.request({
            url: B4.Url.action('CalcNow', 'BaseDataValue'),
            method: 'POST',
            params: {
                efmanorgId: efmanorgId,
                dataMetaObjectType: B4.enums.efficiencyrating.DataMetaObjectType.EfficientcyRating
            }
        })
            .next(function (response) {
                me.getAspect('efficiencyratingEditPanelAspect').setData(efmanorgId);
                me.unmask();
            })
            .error(function (response) {
                me.unmask();
                Ext.Msg.alert('Ошибка!', response.message || 'При выполнении запроса произошла ошибка!');
            });
    },

    init: function () {
        this.callParent(arguments);

        this.control({
            'efManorgFactorTreePanel': {
                'selectionchange': {
                    fn: this.onRootSelected,
                    scope: this
                }
            },

            'efAttributeEditPanel b4savebutton': { 'click': { fn: this.onSaveSection, scope: this } },
            'efManorgEditPanel button[actionName=calcvalues]': { 'click': { fn: this.onCalcBtnClick, scope: this } }
        });
    },

    index: function (id) {
        var me = this,
           view = me.getMainView() || Ext.widget(me.mainViewSelector);

        me.bindContext(view);
        me.setContextValue(view, 'efmanorgId', id);
        me.application.deployView(view);

        me.getAspect('efficiencyratingEditPanelAspect').setData(id);
    },

    getCurrentId: function() {
        return this.getContextValue(this.getMainView(), 'efmanorgId');
    }
});