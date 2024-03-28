/*
Данный аспект предназначен для описание взаимодействия компонентов в карточке Предписания
Необходим для сохранения общих сведений и содержит методы для создания дочерних документов
*/

Ext.define('B4.aspects.GjiPrescription', {
    extend: 'B4.aspects.GjiDocument',
    
    requires: [ 'B4.model.Disposal' ],

    alias: 'widget.gjiprescriptionaspect',
    
    init: function (controller) {
        var actions = {};
        this.callParent(arguments);

        this.controller = controller;
        
        actions[this.editPanelSelector + ' #btnCreatePrescriptionToDisposal'] = { 'click': { fn: this.createDisposal, scope: this } };
        
        controller.control(actions);
    },

    /*
    Метод создания Протокола из карточки Предписания
    в качестве параметров - идентификаторы нарушений
    */
    createProtocol: function(recordIds) {
        var me = this,
            model = me.controller.getModel('ProtocolGji'),
            rec = new model({ Id: 0 });

        rec.set('Inspection', me.controller.params.inspectionId);
        rec.set('TypeDocumentGji', 60);
        rec.set('ParentDocumentsList', [me.controller.params.documentId]);
        rec.set('ViolationsList', recordIds);

        //сохраняем
        rec.save({ id: rec.getId() })
            .next(function(result) {

                //Обновляем дерево меню
                me.reloadTreePanel();

                //Формируем параметры для контроллера редактирования предписания
                var params = {
                    inspectionId: me.controller.params.inspectionId,
                    documentId: result.record.getId(),
                    title: 'Протокол',
                    containerSelector: me.controller.params.containerSelector,
                    treeMenuSelector: me.controller.params.treeMenuSelector
                };
                //Накладываю маску чтобы после нажатия пункта меню в дереве нельзя было нажать 10 раз до инициализации контроллера
                if (!me.controller.hideMask) {
                    me.controller.hideMask = function() { me.controller.unmask(); };
                }

                me.controller.mask('Загрузка', me.controller.getMainComponent());
                me.controller.loadController('B4.controller.ProtocolGji', params, me.controller.params.containerSelector, me.controller.hideMask);

            }, this)
            .error(function(result) {
                Ext.Msg.alert('Ошибка сохранения!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
            }, this);
    },
    
    /*
    Метод создания Распоряжния о проверке предписания
    */
    createDisposal: function () {
        //создаем распоряжение о проверке предписания
        var me = this,
            model = me.controller.getModel('Disposal'),
            rec = new model({ Id: 0 });

        rec.set('Inspection', me.controller.params.inspectionId);
        rec.set('TypeDocumentGji', 10);
        rec.set('TypeDisposal', 20);
        rec.set('ParentDocumentsList', [me.controller.params.documentId]);

        //сохраняем
        rec.save({ id: rec.getId() })
            .next(function (result) {

                //Обновляем дерево меню
                me.reloadTreePanel();

                //Формируем параметры для контроллера редактирования предписания
                var params = {
                    inspectionId: me.controller.params.inspectionId,
                    documentId: result.record.getId(),
                    title: B4.DisposalTextValues.getSubjectiveCase(),
                    containerSelector: me.controller.params.containerSelector,
                    treeMenuSelector: me.controller.params.treeMenuSelector
                };
                //Накладываю маску чтобы после нажатия пункта меню в дереве нельзя было нажать 10 раз до инициализации контроллера
                if (!me.controller.hideMask) {
                    me.controller.hideMask = function () { me.controller.unmask(); };
                }
                me.controller.mask('Загрузка', me.controller.getMainComponent());
                me.controller.loadController('B4.controller.Disposal', params, me.controller.params.containerSelector, me.controller.hideMask);

            }, this)
            .error(function (result) {
                Ext.Msg.alert('Ошибка сохранения!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
            }, this);
    }
});