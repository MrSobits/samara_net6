/*
Данный аспект предназначен для описание взаимодействия компонентов списков документа с документами связи (Предшествующие или Предыдущие)
*/

Ext.define('B4.aspects.GjiDocumentList', {
    extend: 'B4.base.Aspect',

    requires: ['B4.DisposalTextValues'],

    alias: 'widget.gjidocumentlistaspect',

    panelSelector: null,
    gridDocumentSelector: null,
    gridRelationSelector: null,
    storeDocumentName: null,
    storeRelationName: null,
    controller: null,

    init: function (controller) {
        var actions = {};
        this.callParent(arguments);

        this.controller = controller;

        actions[this.gridDocumentSelector] = {
            'rowaction': { fn: this.documentGridRowAction, scope: this },
            'itemdblclick': { fn: this.rowDblClick, scope: this },
            'gridaction': { fn: this.documentGridAction, scope: this },
            'select': { fn: this.documentGridSelectRow, scope: this }
        };

        actions[this.gridDocumentSelector + ' b4updatebutton'] = { 'click': { fn: this.documentGridBtnAction, scope: this } };

        var storeDocument = this.controller.getStore(this.storeDocumentName);
        storeDocument.on('beforeload', this.onStoreDocumentBeforeLoad, this);
        storeDocument.on('load', this.onStoreDocumentLoad, this);

        //иногда этот грид может не рисоваться, например, при нескольких актах проверки предписаний
        if (this.gridRelationSelector) {
            actions[this.gridRelationSelector] = {
                'rowaction': { fn: this.relationGridRowAction, scope: this },
                'itemdblclick': { fn: this.rowDblClick, scope: this },
                'gridaction': { fn: this.relationGridAction, scope: this }
            };

            actions[this.gridRelationSelector + ' b4updatebutton'] = { 'click': { fn: this.relationGridBtnAction, scope: this } };

            var storeRelation = this.controller.getStore(this.storeRelationName);
            storeRelation.on('beforeload', this.onStoreRelationBeforeLoad, this);
        }

        controller.control(actions);
    },

    loadDocumentStore: function () {
        this.controller.getStore(this.storeDocumentName).load();
    },

    loadRelationStore: function () {
        this.controller.getStore(this.storeRelationName).load();
    },

    getDocumentGrid: function () {
        return Ext.ComponentQuery.query(this.gridDocumentSelector)[0];
    },

    getRelationGrid: function () {
        return Ext.ComponentQuery.query(this.gridRelationSelector)[0];
    },

    documentGridBtnAction: function (btn) {
        this.getDocumentGrid().fireEvent('gridaction', this.getDocumentGrid(), btn.actionName);
    },

    relationGridBtnAction: function (btn) {
        this.getRelationGrid().fireEvent('gridaction', this.getRelationGrid(), btn.actionName);
    },

    rowDblClick: function (view, record, item, index, e, eOpts) {
        this.editRecord(record);
    },

    onStoreDocumentLoad: function (store, operation) {
        if (this.gridRelationSelector) {
            if (store.getCount() > 0) {
                this.getDocumentGrid().getSelectionModel().select(0);
            } else {
                this.controller.getStore(this.storeRelationName).removeAll();
            }
        }
    },

    onStoreDocumentBeforeLoad: function (store, operation) {
        if (this.controller.params && this.controller.params.stageId) {
            operation.params["stageId"] = this.controller.params.stageId;
        }
    },

    onStoreRelationBeforeLoad: function (store, operation) {
        //Перед обновлением грида связи мы получаем выделенную строку
        //в первом гриде и передаем Id документа в параметры
        if (this.getDocumentGrid().getSelectionModel().getSelection()[0]) {
            var btnChildren = this.getRelationGrid().down('#btnRelationsChildrenDocs');
            var btnParent = this.getRelationGrid().down('#btnRelationsParentDocs');

            var rec = this.getDocumentGrid().getSelectionModel().getSelection()[0];
            //Если нажата кнопка 'последующие документы, то передаем Id в качестве родительского
            //Иначе получаем те документы для которых текущий документ дочерний'
            if (btnChildren.pressed) {
                //передаем параметр id о том что выбранный документ является родительским для других
                //чтобы получить все его дочерние
                operation.params["parentDocumentId"] = rec.get('Id');
            }
            else if (btnParent.pressed) {
                //передаем параметр id о том что выбранный документ является дочерним для других
                //чтобы получить все его родительские
                operation.params["childrenDocumentId"] = rec.get('Id'); 
            }
               
        }
    },

    documentGridSelectRow: function (rowModel, record, index, opt) {
        //при выделении сроки мы вызываем обновление грида
        this.loadRelationStore();
    },

    documentGridRowAction: function (grid, action, record) {
        switch (action.toLowerCase()) {
            case 'edit':
                this.editRecord(record);
                break;
            case 'delete':
                this.deleteRecord(record);
                break;
        }
    },

    deleteRecord: function (record) {
        var me = this;
        
        Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить документ?', function (result) {
            if (result == 'yes') {
                record.idProperty = "Id";
                me.controller.mask('Удаление', me.controller.getMainComponent());
                record.destroy({
                    success: function() {
                        //Обновляем дерево меню
                        var tree = Ext.ComponentQuery.query(me.controller.params.treeMenuSelector)[0];
                        tree.getStore().load();
                        me.loadDocumentStore();
                        me.controller.unmask();
                        Ext.Msg.alert('Удаление!', 'Документ успешно удален');
                    },
                    failure: function (result, operation) {
                        var response = Ext.decode(operation.response.responseText);
                        me.controller.unmask();
                        Ext.Msg.alert('Ошибка удаления!', Ext.isString(response) ? response : response.message);
                    }
                });

            }
        }, this);
    },
    
    editRecord: function (record) {
        var me = this;
        //заполняем параметры
        var params = {};
        params.documentId = record.get('DocumentId');
        params.inspectionId = me.controller.params.inspectionId;
        params.containerSelector = me.controller.params.containerSelector;
        params.treeMenuSelector = me.controller.params.treeMenuSelector;
        params.title = me.getTitle(record.get('TypeDocumentGji'));

        var controllerName = me.getControllerName(record.get('TypeDocumentGji'));
        me.loadEditPanel(controllerName, params);
    },

    documentGridAction: function (grid, action) {
        switch (action.toLowerCase()) {
            case 'update':
                this.loadDocumentStore();
                break;
        }
    },

    relationGridAction: function (grid, action) {
        switch (action.toLowerCase()) {
            case 'update':
                this.loadRelationStore();
                break;
        }
    },

    relationGridRowAction: function (grid, action, record) {
        switch (action.toLowerCase()) {
            case 'edit':
                this.editRecord(record);
                break;
        }
    },

    relationGridRowDblClick: function (view, record, item, index, e, eOpts) {
        
    },

    GridRowDblClick: function (view, record, item, index, e, eOpts) {
        
    },

    getControllerName: function (documentType) {
        var controllerName = '';
        switch (documentType) {
            //Распоряжение
            case 10: controllerName = 'B4.controller.Disposal'; break;

            //Акт проверки
            case 20: controllerName = 'B4.controller.ActCheck'; break;

            //Акт проверки предписания (он же акт устранения нарушений)
            case 30: controllerName = 'B4.controller.ActRemoval'; break;

            //Акт обследования
            case 40: controllerName = 'B4.controller.ActSurvey'; break;

            //Предписание
            case 50: controllerName = 'B4.controller.Prescription'; break;

            //Протокол
            case 60: controllerName = 'B4.controller.ProtocolGji'; break;

            //Постановление
            case 70: controllerName = 'B4.controller.Resolution'; break;

            //Представление
            case 90: controllerName = 'B4.controller.Presentation'; break;
        }

        return controllerName;
    },

    getTitle: function (documentType) {
        var title = '';
        switch (documentType) {
            //Распоряжение          
            case 10: title = B4.DisposalTextValues.getSubjectiveCase(); break;

            //Акт проверки           
            case 20: title = 'Акт проверки'; break;

            //Акт проверки предписания (он же акт устранения анарушения)           
            case 30: title = 'Акт проверки предписания'; break;

            //Акт обследования            
            case 40: title = 'Акт обследования'; break;

            //Предписание          
            case 50: title = 'Предписание'; break;

            //Протокол           
            case 60: title = 'Протокол'; break;

            //Постановление            
            case 70: title = 'Постановление'; break;

            //Представление             
            case 90: title = 'Представление'; break;
        }

        return title;
    },

    loadEditPanel: function (controllerName, params) {
        var me = this;
        //Накладываю маску чтобы после нажатия пункта меню в дереве нельзя было нажать 10 раз до инициализации контроллера
        if (!me.controller.hideMask) {
            me.controller.hideMask = function () { me.controller.unmask(); };
        }
        me.controller.mask('Загрузка', me.controller.getMainComponent());
        me.controller.loadController(controllerName, params, params.containerSelector, me.controller.hideMask);
    }
});