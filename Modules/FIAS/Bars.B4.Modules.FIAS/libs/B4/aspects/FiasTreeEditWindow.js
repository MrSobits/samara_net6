/*
Данный аспект предназначен исключителньо для работы дерева ФИАС объектов
Навигационной панели и формы редактирования
*/


Ext.define('B4.aspects.FiasTreeEditWindow', {
    extend: 'B4.base.Aspect',

    alias: 'widget.fiastreeeditwindowaspect',

    treeSelector: '#fiasTreePanel',
    editWindowSelector: '#fiasPlacesEditWindow',
    editWindowView: 'Fias.EditWindow',
    replaceWindowSelector: '#fiasReplaceWindow',
    replaceWindowView: 'Fias.ReplaceWindow',
    modelName: 'Fias',
    parentId: 0,
    parentFiasRecord: null,
    selectedTreeRecord: null,
    
    _editWindow: null,

    init: function (controller) {
        var actions = {};
        this.callParent(arguments);

        //вешаемся на события дерева
        actions[this.treeSelector] = {
            'itemclick': { fn: this.onTreeItemClick, scope: this },
            'load': { fn: this.onTreeAfterLoad, scope: this },
            'beforeload': { fn: this.onTreeBeforeLoad, scope: this }
        };

        //Нажатие 'Добавить в корень'
        actions[this.treeSelector + ' #btnAddRoot'] = { 'click': { fn: this.onAddRoot, scope: this} };

        //Нажатие 'Добавить дочерний'
        actions[this.treeSelector + ' #btnAddChildren'] = { 'click': { fn: this.onAddChildren, scope: this} };

        //Нажатие 'Заменить'
        actions[this.treeSelector + ' #btnReplace'] = { 'click': { fn: this.onReplace, scope: this} };

        //Нажатие 'Редактировать'
        actions[this.treeSelector + ' #btnEdit'] = { 'click': { fn: this.onEdit, scope: this} };

        //Нажатие 'Удалить'
        actions[this.treeSelector + ' #btnDelete'] = { 'click': { fn: this.onDelete, scope: this} };

        //Нажатие 'Сохранить' на форме редактирования
        actions[this.editWindowSelector + ' #btnSave'] = { 'click': { fn: this.saveRequestHandler, scope: this} };

        //Нажатие 'Закрыть' на форме редактирования
        actions[this.editWindowSelector + ' #btnClose'] = { 'click': { fn: this.closeRequestHandler, scope: this} };

        //Подписка beforeload у Выпадающего списка Уровня
        actions[this.editWindowSelector + ' #levelCombobox'] = { 'storebeforeload': { fn: this.onLevelStoreBeforeLoad, scope: this} };

        controller.control(actions);
    },

    TreeLoad: function () {
        var tree = this.getTree();
        tree.down('#navigationToolbar').setDisabled(true);
        tree.getStore().load();
    },
    
    onTreeBeforeLoad: function(store, opts) {
        var node = opts.node;
        if (node && node.data && node.data.fiasGuidId) {
            Ext.apply(store.getProxy().extraParams, {                
                parentGuid: node.data.fiasGuidId
            });
        }
    },

    onTreeAfterLoad: function () {
        var tree = this.getTree();
        tree.down('#navigationToolbar').setDisabled(false);
        //Тут потом ченибудь допишем после загрузки стора
        //Активацию кнопока на правом гриде например чтобы ненажали пока дерево незагрузится
    },

    getWindow: function () {
        if (this._editWindow) {
            return this._editWindow;
        }

        var editWindow = Ext.ComponentQuery.query(this.editWindowSelector)[0];

        if (editWindow) {
            editWindow = editWindow.destroy();
        }

        if (!editWindow) {
            editWindow = this.controller.getView(this.editWindowView).create({
                constrain: true,
                itemId: this.editWindowSelector.replace('#', '')
            });
        }
        if (B4.getBody().getActiveTab()) {
            B4.getBody().getActiveTab().add(editWindow);
        } else {
            B4.getBody().add(editWindow);
        }

        this._editWindow = editWindow;

        this._editWindow.on('close', this.onCachedWindowClose, this);

        return this._editWindow;
    },
    
    onCachedWindowClose: function(win) {
        this._editWindow.destroy();
        this._editWindow = null;
    },
    
    getReplaceWindow: function() {
        var editWindow = Ext.ComponentQuery.query(this.replaceWindowSelector)[0];

        if (!editWindow) {
            editWindow = this.controller.getView(this.replaceWindowView).create();
        }

        return editWindow;
    },

    getTree: function () {
        return Ext.ComponentQuery.query(this.treeSelector)[0];
    },

    onTreeItemClick: function (view, record) {
        //Фиксируем выбранную запись
        this.selectedTreeRecord = record;
        this.controller.getStore('FiasStreet').load();
        //Тут потом надо еще обновить правый грид с улицами
    },

    onAddRoot: function () {
        this.parentId = 0;
        this.parentFiasRecord = null;
        this.editRecord();
    },

    onAddChildren: function () {
        if (!this.selectedTreeRecord) {
            Ext.Msg.alert('Внимание!', 'Необходимо выделить запись для которой необходимо добавить дочерний элемент!');
            return;
        }

        //Фиксируем выбранную запись как родительскую
        this.parentId = this.selectedTreeRecord.get('fiasId');
        this.parentFiasRecord = null;
        this.editRecord();
    },

    onReplace: function () {
        if (!this.selectedTreeRecord) {
            Ext.Msg.alert('Внимание!', 'Необходимо выделить запись, котрую необходимо заменить!');
            return;
        }
    },

    onEdit: function () {
        if (!this.selectedTreeRecord) {
            Ext.Msg.alert('Внимание!', 'Необходимо выделить запись для редактирования!');
            return;
        }
        //Фиксируем также родителя редактируемой записи (Это нужно для получения списка доступных Уровней по уровню родителя)
        this.parentId = this.selectedTreeRecord.get('fiasParentId');
        this.parentFiasRecord = null;
        this.editRecord(this.selectedTreeRecord);
    },

    onDelete: function () {
        if (!this.selectedTreeRecord) {
            Ext.Msg.alert('Внимание!', 'Необходимо выделить запись для удаления!');
            return;
        }

        this.deleteRecord(this.selectedTreeRecord);
    },

    onLevelStoreBeforeLoad: function () {
        // Функция была пропущена, необходимо для правильного открытия ФИАС, так как есть подписка на нее
    },

    closeRequestHandler: function () {
        this.getWindow().close();
    },

    saveRequestHandler: function () {
        var rec, editWindow = this.getWindow();

        editWindow.getForm().updateRecord();
        rec = editWindow.getRecord();

        if (this.parentFiasRecord && this.parentFiasRecord.get('AOLevel') >= rec.get('AOLevel')) {
            Ext.Msg.alert('Ошибка сохранения!', 'Уровень дочерней записи не должен быть больше уровня записи родительской');
        }

        if (editWindow.getForm().isValid()) {
            var me = this;

            var model = me.controller.getModel(me.modelName);

            var recId = rec.getId();
            rec.save({ id: rec.getId() })
            .next(function (result) {
                editWindow.close();

                var tree = me.getTree();
                var selectNode;
                if (recId > 0) {

                    selectNode = tree.getSelectionModel().getSelection()[0];
                    //если запись редактировалась то надо заменить нод
                    //То есть берем выделенный нод и занлсим в него обновленные данные
                    selectNode.set('text', result.record.get('OffName') + ' ' + result.record.get('ShortName'));
                    selectNode.set('level', result.record.get('AOLevel'));
                } else {
                    //если запись добавилась то создаем новый нод и добавляем в выделенный нод дочерний элемент

                    //Загружаем запись потмоучто после сохранения нам необходим AOId

                    model.load(result.record.getId(), {
                        success: function (loadedRecord) {

                            selectNode = tree.getSelectionModel().getSelection()[0];

                            if (!selectNode) {
                                selectNode = tree.getRootNode();
                            }

                            selectNode.insertChild(0, {
                                id: loadedRecord.get('Id'),
                                text: loadedRecord.get('OffName') + ' ' + loadedRecord.get('ShortName'),
                                expanded: true,
                                level: loadedRecord.get('AOLevel'),
                                fiasId: loadedRecord.get('Id'),
                                fiasParentId: me.parentFiasRecord ? me.parentFiasRecord.get('Id') : null,
                                fiasGuidId: loadedRecord.get('AOGuid'),
                                fiasParentGuidId: loadedRecord.get('ParentGuid'),
                                mirrorGuid: loadedRecord.get('MirrorGuid'),
                                leaf: true
                            });

                            selectNode.set('leaf', false);
                            selectNode.expand();
                        },
                        scope: me
                    });
                }

                //Если добавили не улицу, то нужно вставить элемент в нужную позицию дерева
                //Если добавили улицу, то нужно обновить таблицу
            }, me)
            .error(function (result) {
                Ext.Msg.alert('Ошибка сохранения!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
            }, me);
        }
    },

    deleteRecord: function (treeElement) {

        if (!treeElement.get('typeRecord') == 10) {
            Ext.Msg.alert('Внимание!', 'Записи загруженные из ФИАС удалить нельзя!');
            return;
        }

        var me = this;
        var tree = this.getTree();

        Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
            if (result == 'yes') {
                var model = this.controller.getModel(this.modelName);

                var rec = new model({ Id: treeElement.get('fiasId') });

                rec.destroy()
                    .next(function () {
                        this.parentId = 0;
                        this.parentFiasRecord = null;
                        this.selectedTreeRecord = null;

                        var selectNode = tree.getSelectionModel().getSelection()[0];
                        if (selectNode != null) {
                            selectNode.parentNode.removeChild(selectNode);
                        }
                    }, this)
                    .error(function (result) {
                        Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                    }, this);
            }
        }, me);
    },

    editRecord: function (treeElement) {
        var me = this,
        id = treeElement ? treeElement.get('fiasId') : null,
        model;

        model = this.controller.getModel(this.modelName);

        //получаем родительскую запись по ParentId
        model.load(this.parentId, {
            success: function (parentRec) {

                this.parentFiasRecord = parentRec;

                //После получения родительской записи формируем новую запис либо загружаем существующую
                //И для новой записи проставляем сразу все значения из родительской (чтбы легче было редактировать)
                if (id > 0) {
                    model.load(id, {
                        success: function (rec) {
                            me.setData(rec);
                        },
                        scope: this
                    });
                }
                else {
                    var newRec = new model({ Id: 0 });
                    this.setData(newRec);
                }

            },
            scope: this
        });
    },

    setData: function (rec) {

        var editWindow = this.getWindow();

        //проставляем данные родителя
        if (rec.getId() == 0 && this.parentFiasRecord) {
            rec.set('ParentGuid', this.parentFiasRecord.get('AOGuid'));
            rec.set('CodeRegion', this.parentFiasRecord.get('CodeRegion'));
            rec.set('CodeAuto', this.parentFiasRecord.get('CodeAuto'));
            rec.set('CodeArea', this.parentFiasRecord.get('CodeArea'));
            rec.set('CodeCity', this.parentFiasRecord.get('CodeCity'));
            rec.set('CodeCtar', this.parentFiasRecord.get('CodeCtar'));
            rec.set('CodePlace', this.parentFiasRecord.get('CodePlace'));
            rec.set('CodeStreet', this.parentFiasRecord.get('CodeStreet'));
            rec.set('CodeExtr', this.parentFiasRecord.get('CodeExtr'));
            rec.set('CodeSext', this.parentFiasRecord.get('CodeSext'));
            rec.set('PostalCode', this.parentFiasRecord.get('PostalCode'));
            rec.set('MirrorGuid', this.parentFiasRecord.get('MirrorGuid'));

            //Если создается новая запись то берем родительский Уровень и ставим новой записи следующий 
            //уровень (как будто бы по default)
            if (this.parentFiasRecord) {
                var level = this.parentFiasRecord.get('AOLevel');
                if (1 < level < 7)
                    level++;
                else if (level == 90)
                    level = 91;

                rec.set('AOLevel', level);
            }
        }

        //блокируем все поля которые не относятся к уровню записи
        editWindow.down('#codeRegionTextField').setDisabled(false);
        editWindow.down('#codeAutoTextField').setDisabled(false);
        editWindow.down('#codeAreaTextField').setDisabled(false);
        editWindow.down('#codeCityTextField').setDisabled(false);
        editWindow.down('#codeCtarTextField').setDisabled(false);
        editWindow.down('#codePlaceTextField').setDisabled(false);
        editWindow.down('#codeStreetTextField').setDisabled(false);
        editWindow.down('#codeExtrTextField').setDisabled(false);
        editWindow.down('#codeSextTextField').setDisabled(false);

        switch (rec.get('AOLevel')) {
            case 2:
                {
                    editWindow.down('#codeRegionTextField').setDisabled(true);
                }
                break;

            case 3:
                {
                    editWindow.down('#codeRegionTextField').setDisabled(true);
                    editWindow.down('#codeAutoTextField').setDisabled(true);
                }
                break;

            case 4:
                {
                    editWindow.down('#codeRegionTextField').setDisabled(true);
                    editWindow.down('#codeAutoTextField').setDisabled(true);
                    editWindow.down('#codeAreaTextField').setDisabled(true);
                }
                break;

            case 5:
                {
                    editWindow.down('#codeRegionTextField').setDisabled(true);
                    editWindow.down('#codeAutoTextField').setDisabled(true);
                    editWindow.down('#codeAreaTextField').setDisabled(true);
                    editWindow.down('#codeCityTextField').setDisabled(true);
                }
                break;

            case 6:
                {
                    editWindow.down('#codeRegionTextField').setDisabled(true);
                    editWindow.down('#codeAutoTextField').setDisabled(true);
                    editWindow.down('#codeAreaTextField').setDisabled(true);
                    editWindow.down('#codeCityTextField').setDisabled(true);
                    editWindow.down('#codeCtarTextField').setDisabled(true);
                }
                break;

            case 7:
                {
                    editWindow.down('#codeRegionTextField').setDisabled(true);
                    editWindow.down('#codeAutoTextField').setDisabled(true);
                    editWindow.down('#codeAreaTextField').setDisabled(true);
                    editWindow.down('#codeCityTextField').setDisabled(true);
                    editWindow.down('#codeCtarTextField').setDisabled(true);
                    editWindow.down('#codePlaceTextField').setDisabled(true);
                }
                break;

            case 90:
                {
                    editWindow.down('#codeRegionTextField').setDisabled(true);
                    editWindow.down('#codeAutoTextField').setDisabled(true);
                    editWindow.down('#codeAreaTextField').setDisabled(true);
                    editWindow.down('#codeCityTextField').setDisabled(true);
                    editWindow.down('#codeCtarTextField').setDisabled(true);
                    editWindow.down('#codePlaceTextField').setDisabled(true);
                    editWindow.down('#codeStreetTextField').setDisabled(true);
                }
                break;

            case 91:
                {
                    editWindow.down('#codeRegionTextField').setDisabled(true);
                    editWindow.down('#codeAutoTextField').setDisabled(true);
                    editWindow.down('#codeAreaTextField').setDisabled(true);
                    editWindow.down('#codeCityTextField').setDisabled(true);
                    editWindow.down('#codeCtarTextField').setDisabled(true);
                    editWindow.down('#codePlaceTextField').setDisabled(true);
                    editWindow.down('#codeStreetTextField').setDisabled(true);
                    editWindow.down('#codeExtrTextField').setDisabled(true);
                }
                break;
        }

        editWindow.down('#levelCombobox').setDisabled(false);

        editWindow.loadRecord(rec);

        //Необходимо проверить Если у записи TypeRecord = 10
        //То есть загруженные записи из ФИАС редактировать нельзя
        if (rec.get('TypeRecord') == 10) {
            editWindow.down('#btnSave').setDisabled(true);
        } else {
            editWindow.down('#btnSave').setDisabled(false);
        }

        editWindow.show();
    }

});