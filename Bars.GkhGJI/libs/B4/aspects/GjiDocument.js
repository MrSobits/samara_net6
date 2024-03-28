/*
Данный аспект предназначен для описание взаимодействия компонентов в документа ГЖИ
в именно Сохранение основных сведений, кнопки Отмена, Удалить, Зарегистрировать
*/

Ext.define('B4.aspects.GjiDocument', {
    extend: 'B4.aspects.GkhEditPanel',

    alias: 'widget.gjidocumentaspect',

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    // Необходимость предупреждения об удалении связных объектов
    enableRelatedObjectDeletingWarning: false,

    init: function(controller) {
        var actions = {};
        this.callParent(arguments);

        actions[this.editPanelSelector + ' #btnCancel'] = { 'click': { fn: this.btnCancelClick, scope: this } };
        actions[this.editPanelSelector + ' #btnDelete'] = { 'click': { fn: this.btnDeleteClick, scope: this } };

        this.on('beforesetdata', this.onBeforeSetData, this);
        this.on('aftersetpaneldata', this.onAfterSetPanelData, this);
        this.on('savesuccess', this.onSaveSuccess, this);

        this.stateButtonSelector = this.editPanelSelector + ' #btnState';
        controller.control(actions);
    },
    
    getTreePanel: function() {
        return this.componentQuery(this.controller.params.treeMenuSelector);
    },

    reloadTreePanel: function() {
        getTreePanel().getStore().load();
    },

    onSaveSuccess: function (asp, rec) {
        if (!(asp.controller.params && asp.controller.params.title)) {
            return;
        }

        if (rec.get('DocumentNumber')) {
            this.getPanel().setTitle(asp.controller.params.title + " " + rec.get('DocumentNumber'));
        } else {
            this.getPanel().setTitle(asp.controller.params.title);
        }
    },

    onBeforeSetData: function () {
        debugger;
        var groups = Ext.ComponentQuery.query(this.editPanelSelector + ' buttongroup');
        var idx = 0;
        //теперь пробегаем по массиву groups и дизаблим все группы кнопок на панели
        while (true) {

            if (!groups[idx])
                break;

            groups[idx].setDisabled(true);
            idx++;
        }

        return true;
    },

    onAfterSetPanelData: function () {
        debugger;
        var groups = Ext.ComponentQuery.query(this.editPanelSelector + ' buttongroup');
        var idx = 0;
        //теперь пробегаем по массиву groups и активируем все группы кнопок на панели
        while (true) {

            if (!groups[idx])
                break;

            groups[idx].setDisabled(false);
            idx++;
        }
    },

    //после нажатия на Удалить идет удаление документа
    btnDeleteClick: function() {
        var me = this;

        Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить документ?', function(result) {
            if (result == 'yes') {
                if (me.enableRelatedObjectDeletingWarning) {
                    Ext.Msg.confirm('Удаление записи!', 'При удалении данной записи произойдет удаление всех связанных объектов. Продолжить удаление?',
                        function (resultWithRelatedEntities) {
                            if (resultWithRelatedEntities == 'yes') {
                                me.deleteDocument();
                            }
                        });
                } else {
                    me.deleteDocument();
                }
            }
        }, this);
    },

    deleteDocument: function () {
        var me = this,
            panel = me.getPanel(),
            record = panel.getForm().getRecord()

        me.mask('Удаление', B4.getBody());
        record.destroy()
            .next(function() {
                //Обновляем дерево меню
                var tree = me.getTreePanel();
                tree.getStore().load();

                Ext.Msg.alert('Удаление!', 'Документ успешно удален');

                panel.close();
                me.unmask();
            }, me)
            .error(function(result) {
                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                me.unmask();
            }, me);
    },

    //При нажатии на кнопку Оттмена мы просто загружаем существующие данные документа
    btnCancelClick: function() {
        this.setData(this.controller.params.documentId);
    }
});