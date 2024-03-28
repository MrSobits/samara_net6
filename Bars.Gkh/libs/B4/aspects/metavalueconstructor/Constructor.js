Ext.define('B4.aspects.metavalueconstructor.Constructor', {
    extend: 'B4.aspects.GridEditForm',
    requires: [],

    alias: 'widget.metavalueconstructoraspect',

    /* Панель содержащая все редакторы */
    editPanelSelector: null,

    /* Панель редактирования элемента верхнего уровня */
    editElementSelector: null,

    /* Панель отображения дерева */
    treePanelSelector: null,

    /* Панель отображения элементов верхнего уровня */
    treeSelector: null,

    /* Панель редактирования элементов нижнего уровня*/
    treeEditPanelSelector: null,

    modelName: 'B4.model.metavalueconstructor.DataMetaInfo',

    /* Наименование уровней и селекторы для форм редактирования */
    levels: [],

    /* Код по умолчанию для сущностей  */
    defaultCode: null,

    treeSelectFieldSelector: null,

    ROOT_LEVEL: 0,
    CHILD_LEVEL: 1,

    init: function (config) {
        var me = this;

        me.callParent(arguments);
        me.addEvents('onchangegroup', 'initiated');

        me.on('deletesuccess', me.disablePanel, me);
        me.on('onchangegroup', me.onChangeGroup, me);
    },

    otherActions: function(actions) {
        var me = this;

        actions[this.treeSelector] = {
            'rowaction': {
                fn: this.rowAction,
                scope: this
            },
            'selectionchange': {
                fn: this.onSelectionChange,
                scope: this
            }
        };

        actions[me.treePanelSelector] = {
            'rowaction': {
                fn: me.rowAction,
                scope: me
            }
        };
        
        actions[me.treeSelector + ' button[action=delete]'] = {
            'click': {
                fn: function(btn) {
                    var selected = me.getSelected(me.ROOT_LEVEL);

                    if (!selected) {
                        return false;
                    }

                    me.getTreePanel(me.ROOT_LEVEL).fireEvent('rowaction', me.getTreePanel(me.ROOT_LEVEL), btn.action, selected);

                    return true;
                },
                scope: me
            }
        };

        actions[me.treePanelSelector + ' button[action=delete]'] = {
            'click': {
                fn: function (btn) {
                    var selected = me.getTreePanel(me.CHILD_LEVEL).getSelectionModel().getSelection()[0];

                    if (!selected) {
                        return false;
                    }

                    me.getTreePanel(me.CHILD_LEVEL).fireEvent('rowaction', me.getTreePanel(me.CHILD_LEVEL), btn.action, selected);

                    return true;
                },
                scope: me
            }
        };

        actions[this.treeSelector + ' b4addbutton'] = {
            'click': {
                fn: function () {
                    me.editRecord(null, 0);
                },
                scope: this
            }
        };

        actions[this.treeSelector + ' b4updatebutton'] = {
            'click': {
                fn: function () {
                    me.updateTreePanel();
                },
                scope: this
            }
        };

        actions[me.treePanelSelector + ' menuitem']= {
            'click': {
                fn: function(menuItem) {
                    me.editRecord(null, menuItem.level);
                },
                scope: me
            }
        };
        
        actions[me.treePanelSelector + ' b4updatebutton'] = {
            'click': {
                fn: function() {
                    me.updateTreePanel(me.CHILD_LEVEL);
                },
                scope: me
            }
        };

        actions[me.treeEditPanelSelector + ' b4savebutton'] = {
            'click': {
                fn: me.saveRequestHandler,
                scope: me
            }
        };

        if (me.editPanelSelector) {
            actions[(me.editPanelSelector)] = {
                'afterrender': {
                    fn: function () {
                        me.getTreePanel(me.ROOT_LEVEL).disable();
                        me.disablePanel(me, 0);

                        me.fireEvent('initiated', me);
                    },
                    scope: me
                }
            };
        }

        if (me.treeSelectFieldSelector) {
            actions[me.treeSelectFieldSelector] = {
                'beforeshowselectwindow': {
                    fn: function(sel, selectWindow) {
                        var me = this,
                            treePanel = selectWindow.down('treepanel');

                        me.mask('Загрузка...', treePanel.down('treeview'));
                        me.loadTreePanel(0, 'GetDataFillers', null, treePanel, null, function() { me.unmask(); });
                    },
                    scope: me
                }
            }
        }
    },

    /* Метод необходимо переопределить в контроллере для получения реализации */
    getGroupId: function() {
        Ext.Msg.alert('Ошибка!', 'Не удалось определить тип конструктора');
        return null;
    },

    onChangeGroup: function(groupId) {
        var me = this;

        me.getForm(me.CHILD_LEVEL).getForm().reset();
        me.getForm(me.ROOT_LEVEL).getForm().reset();
        me.getTreePanel(me.CHILD_LEVEL).setRootNode(null);
        me.getTreePanel(me.ROOT_LEVEL).setRootNode(null);

        me.getTreePanel(me.ROOT_LEVEL).disable();
        me.disablePanel(me, 0);

        if (groupId) {
            me.getTreePanel(me.ROOT_LEVEL).enable();
            me.updateTreePanel();
        }
    },

    onSelectionChange: function(selModel, newRecords) {
        var me = this,
            record = newRecords && newRecords.length > 0 ? newRecords[0] : null;

        if (record) {
            me.updateTreePanel(me.CHILD_LEVEL);
        }
    },

    editRecord: function (record, level) {
        var me = this,
            i,
            id = record ? record.getId() : null,
            model = this.getModel(record),
            metaLevel = level
                ? level
                : record
                    ? record.get('Level')
                    : 0;

        id ? model.load(id, {
            success: function (rec) {
                me.setFormData(rec);
            },
            scope: this
        }) : me.setFormData(new model({
            Id: 0,
            Name: 'Новый ' + me.levels[metaLevel].name,
            Code: me.defaultCode,
            Actual: true,
            Group: me.getGroupId(),
            Level: metaLevel,
            Parent: metaLevel > 0 ? me.getSelected(metaLevel - 1).getId() : null
        }));
        

        me.setPanelEnabled(metaLevel);
        me.getForm(metaLevel).getForm().isValid();
    },

    btnAction: function (btn, level) {
        this.getTreePanel(level).fireEvent('gridaction', this.getTreePanel(level), btn.actionName);
    },

    setFormData: function (rec) {
        var level = rec.get('Level'),
            form = this.getForm(level);
        if (this.fireEvent('beforesetformdata', this, rec, form) !== false) {
            form.loadRecord(rec);
            form.getForm(level).updateRecord();
            form.getForm(level).isValid();

            if (level > 0) {
                form.setFormSettings(level);
            }
        }

        this.fireEvent('aftersetformdata', this, rec, form);
    },

    saveRequestHandler: function (btn) {
        var metaLevel = this.getRecord(btn).get('Level'),
            rec,
            form = this.getForm(metaLevel);
        if (this.fireEvent('beforesaverequest', this) !== false) {
            form.getForm().updateRecord();
            rec = this.getRecordBeforeSave(form.getRecord());

            this.fireEvent('getdata', this, rec);

            if (form.getForm().isValid()) {
                if (this.fireEvent('validate', this)) {
                    this.saveRecord(rec);
                }
            } else {
                var errorMessage = this.getFormErrorMessage(form);
                Ext.Msg.alert('Ошибка сохранения!', errorMessage);
            }
        }
    },

    saveRecordHasNotUpload: function (rec) {
        var me = this;
        var metaLevel = rec.get('Level'),
            form = this.getForm(metaLevel),
            isNew = rec.phantom;

        me.mask('Сохранение', form);
        rec.save({ id: rec.getId() })
            .next(function (result) {
                me.unmask();

                var model = me.getModel();

                if (result.responseData && result.responseData.data) {
                    var data = result.responseData.data;
                    if (data.length > 0) {
                        var id = data[0] instanceof Object ? data[0].Id : data[0];
                        model.load(id, {
                            success: function (newRec) {
                                me.updateTreePanel(newRec.get('Level'), isNew ? newRec : null);
                                me.setFormData(newRec);

                                me.fireEvent('savesuccess', me, newRec);
                            }
                        });
                    } else {
                        me.fireEvent('savesuccess', me);
                    }
                } else {
                    me.fireEvent('savesuccess', me);
                }
            }, this)
            .error(function (result) {
                me.unmask();
                me.fireEvent('savefailure', result.record, result.responseData);

                Ext.Msg.alert('Ошибка сохранения!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
            }, this);
    },

    deleteRecord: function (record) {
        var me = this;

        Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись? Все связанные элементы будут удалены', function (result) {
            if (result === 'yes') {
                var model = this.getModel(record);

                var rec = new model({ Id: record.getId() });
                me.mask('Удаление', B4.getBody());
                rec.destroy({ timeout: 30 * 60 * 1000 }) // 30 минут, при больше кол-ве элементов может удаляться достаточно долго
                    .next(function () {
                        this.fireEvent('deletesuccess', this, record.get('Level'));
                        me.unmask();
                        me.updateTreePanel(record.get('Level'));
                    }, this)
                    .error(function (result) {
                        Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                        me.unmask();
                    }, this);
            }
        }, me);
    },

    getPanel: function() {
        if (this.editPanelSelector) {
            return this.componentQuery(this.editPanelSelector);
        }
        return null;
    },

    getForm: function (level) {
        var me = this,
            editFormSelector = me.levels !== null && me.levels.length > level ? me.levels[level].editFormSelector : me.editFormSelector;
        if (editFormSelector) {
            return this.componentQuery(editFormSelector);
        }

        return null;
    },

    getTreePanel: function (level) {
        if (!level) {
            return this.componentQuery(this.treeSelector);
        }

        if (level > 0 && this.treePanelSelector) {
            return this.componentQuery(this.treePanelSelector);
        }

        return null;
    },

    updateTreePanel: function (lvl, selectedRec) {
        var me = this,
            id = lvl ? me.getSelected(me.ROOT_LEVEL).getId() : 0,
            treePanel = me.getTreePanel(lvl ? lvl : 0),
            selected = treePanel.getSelectionModel().getSelection()[0],
            selectPath,
            action = lvl && lvl > 0 ? 'GetTree' : 'GetRootElements';

        if (selected) {
            selectPath = selected.getPath();
        }

        me.controller.mask('Загрузка...', treePanel.down('treeview'));
        me.loadTreePanel(id, action, selectPath, treePanel, selectedRec, function() { me.controller.unmask() });
    },

    loadTreePanel: function (id, action, selectPath, treePanel, selectedRec, callback) {
        var me = this;

        Ext.Ajax.request({
            method: 'GET',
            url: B4.Url.action(action, 'DataMetaInfo'),
            params: {
                id: id,
                groupId: me.getGroupId()
            },
            success: function (response) {
                var dataTree = Ext.JSON.decode(response.responseText);
                treePanel.setRootNode(dataTree);
                treePanel.expandAll();

                if (selectedRec) {
                    selectPath = treePanel.getStore().getById(selectedRec.getId()).getPath();
                }

                if (selectPath) {
                    treePanel.selectPath(selectPath);
                }

                if (callback) {
                    callback();
                }
            },
            failure: function (response) {
                var obj = Ext.decode(response.responseText);

                if (callback) {
                    callback();
                }

                Ext.Msg.alert('Ошибка', obj.message || 'Не удалось загрузить данные');
            }
        });
    },

    disablePanel: function (asp, level) {
        var me = this;

        if (level === 0) {
            me.getForm(me.ROOT_LEVEL).getForm().reset();
            me.getTreePanel().getStore().getProxy().clear();
            me.getPanel().disable();
        }

        me.getForm(me.CHILD_LEVEL).getForm().reset();
    },

    getSelected: function (level) {
        var me = this,
            selection = me.getTreePanel(level).getSelectionModel().getSelection(),
            selectedRec = selection && selection.length > 0 ? selection[0] : null;

        while (selectedRec) {
            if (selectedRec.get('Level') === level) {
                return selectedRec;
            } else {
                selectedRec = selectedRec.parentNode;
            }
        }

        return null;
    },

    getRecord: function(btn) {
        return btn.up('toolbar').up().getForm().getRecord();
    },

    setPanelEnabled: function (level) {
        var me = this,
            treePanel = me.getTreePanel(me.CHILD_LEVEL),
            menuItems = treePanel.down('b4addbutton').menu.items.items,
            selected = me.getSelected(level);

        if (!level && level !== 0 || level === 0) {
            me.getPanel().enable();

            me.getTreePanel(me.CHILD_LEVEL).getSelectionModel().deselectAll();
        }

        if (level > 0) {
            me.getForm(me.CHILD_LEVEL).enable();
        } else {
            me.getForm(me.CHILD_LEVEL).disable();
            me.getForm(me.CHILD_LEVEL).getForm().reset();
        }

        Ext.Array.forEach(menuItems,
            function (element) {
                if (selected && selected.get('Level') + 1 === element.level || level >= element.level) {
                    element.enable();
                } else {
                    element.disable();
                }
            });
    }
});