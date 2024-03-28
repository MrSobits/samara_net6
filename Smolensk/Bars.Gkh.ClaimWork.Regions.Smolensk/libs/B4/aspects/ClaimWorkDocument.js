Ext.define('B4.aspects.ClaimWorkDocument', {
    extend: 'B4.aspects.GkhEditPanel',

    alias: 'widget.claimworkdocumentaspect',

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    // Наименование аспекта для работы с меню создания документов
    docCreateAspectName: null,

    init: function (controller) {
        var me = this,
            actions = {};

        me.callParent(arguments);

        actions[this.editPanelSelector + ' button[action=delete]'] = { 'click': { fn: this.btnDeleteClick, scope: this } };

        controller.control(actions);
    },

    onPreSaveSuccess: function() {
        this.callParent(arguments);

        if (this.docCreateAspectName) {
            var asp = this.controller.getAspect(this.docCreateAspectName);
            if (asp) {
                asp.reloadMenu();
            }
        }
    },

    getRecordBeforeSave: function (record) {
        var clw = record.get('ClaimWork');
        if (clw && clw.Id) {
            record.set('ClaimWork', clw.Id);
        }
        return record;
    },

    btnDeleteClick: function() {
        var me = this,
            panel = me.getPanel(),
            record = panel.getForm().getRecord(),
            docId = me.controller.getContextValue(panel, 'docId'),
            docType = me.controller.getContextValue(panel, 'docType'),
            questionStr = 'Вы действительно хотите удалить документ?';

        switch (docType) {
            case 'lawsuit':
                B4.Ajax.request({
                    url: B4.Url.action('CheckChildDocs', 'Petition', {
                        id: docId
                    }),
                    timeout: 999999
                }).next(function (resp) {
                    var data = Ext.decode(resp.responseText).data;
                    if (data) {
                        questionStr = data;
                    }
                    me.recordDestroy(record, questionStr);
                }).error(function (e) {
                    Ext.Msg.alert('Ошибка удаления!', e.message);
                });
                break;

            case 'execprocess':
                B4.Ajax.request({
                    url: B4.Url.action('CheckChildDocs', 'ExecutoryProcess', {
                        id: docId
                    }),
                    timeout: 999999
                }).next(function (resp) {
                    var data = Ext.decode(resp.responseText).data;
                    if (data) {
                        questionStr = data;
                    }
                    me.recordDestroy(record, questionStr);
                }).error(function (e) {
                    Ext.Msg.alert('Ошибка удаления!', e.message);
                });
                break;               
            default:
                me.recordDestroy(record, questionStr);
                break;
        }
    },

    recordDestroy: function (record, questionStr) {
        var me = this;
        Ext.Msg.confirm('Удаление записи!', questionStr, function (result) {
            if (result === 'yes') {
                me.mask('Удаление', B4.getBody());
                record.destroy()
                    .next(function () {
                        var view = me.controller.getMainView(),
                            claimworkId = me.controller.getContextValue(view, 'claimWorkId'),
                            type = me.controller.getContextValue(view, 'type');
                        B4.QuickMsg.msg('Удаление', 'Документ удален успешно', 'success');
                        if (claimworkId && type) {
                            Ext.History.add(Ext.String.format("claimwork/{0}/{1}/deletedocument", type, claimworkId));
                        }
                        me.unmask();
                    }, me)
                    .error(function (result) {
                        me.unmask();
                        Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                    }, me);
            }
        }, me);
    }
});