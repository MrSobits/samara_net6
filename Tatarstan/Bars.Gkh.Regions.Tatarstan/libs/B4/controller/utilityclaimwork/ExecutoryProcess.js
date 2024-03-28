Ext.define('B4.controller.utilityclaimwork.ExecutoryProcess', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.StateContextButton',
        'B4.aspects.ClaimWorkDocument',
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.ClaimWorkDocCreateButton',
        'B4.aspects.StateButton'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'utilityclaimwork.ExecutoryProcess',
        'utilityclaimwork.ExecutoryProcessDoc'
    ],

    views: [
        'utilityclaimwork.ExecProcEditPanel',
        'utilityclaimwork.ExecProcDocumentGrid',
        'utilityclaimwork.ExecProcDocumentAddWindow'
    ],

    stores: [
        'utilityclaimwork.ExecutoryProcessDoc'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'utilityexecproceditpanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Clw.ClaimWork.UtilityDebtor.Debt.Save', applyTo: 'b4savebutton', selector: 'utilityexecproceditpanel'
                },
                {
                    name: 'Clw.ClaimWork.UtilityDebtor.Debt.Save', applyTo: 'button[name=State]', selector: 'utilityexecproceditpanel'
                }
            ]
        },
        {
            xtype: 'claimworkdoccreatebuttonaspect',
            name: 'utilityExecProcCreateButtonAspect',
            buttonSelector: 'utilityexecproceditpanel acceptmenubutton',
            containerSelector: 'utilityexecproceditpanel'
        },
        {
            xtype: 'claimworkdocumentaspect',
            name: 'utilityExecProcEditPanelAspect',
            editPanelSelector: 'utilityexecproceditpanel',
            modelName: 'utilityclaimwork.ExecutoryProcess',
            docCreateAspectName: 'utilityExecProcCreateButtonAspect',
            listeners: {
                aftersetpaneldata: function (asp, rec) {
                    var me = this;                   
                    me.controller.getAspect('utilityExecProcButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                },
                savesuccess: function () {
                    this.controller.getAspect('utilityExecProcCreateButtonAspect').reloadMenu();
                }
            }
        },
        {
            /*
             * Вешаем аспект смены статуса в карточке редактирования
             */
            xtype: 'statecontextbuttonaspect',
            name: 'utilityExecProcButtonAspect',
            stateButtonSelector: 'utilityexecproceditpanel button[name=State]',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    asp.setStateData(entityId, newState);
                }
            }
        },
        {
            /*
            Аспект взаимодействия Таблицы документов с формой редактирования
            */
            xtype: 'grideditctxwindowaspect',
            name: 'utilityExecProcDocAspect',
            gridSelector: 'execprocdocumentgrid',
            editFormSelector: 'execprocdocumentaddwin',
            modelName: 'utilityclaimwork.ExecutoryProcessDoc',
            editWindowView: 'utilityclaimwork.ExecProcDocumentAddWindow',

            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('ExecutoryProcess', asp.controller.getCurrentDoc());
                    }
                }
            }

        }
    ],

    getCurrentDoc: function () {
        var me = this;
        return me.getContextValue(me.getMainComponent(), 'docId');
    },

    index: function (type, id, docId) {
        var me = this,
            view = me.getMainView() || Ext.widget('utilityexecproceditpanel'),
            store;

        view.ctxKey = Ext.String.format('claimwork/{0}/{1}/{2}/execprocess', type, id, docId);
        me.bindContext(view);

        me.setContextValue(view, 'claimWorkId', id);
        me.setContextValue(view, 'type', type);
        me.setContextValue(view, 'docId', docId);
        me.setContextValue(view, 'docType', 'execprocess');
        me.application.deployView(view, 'claim_work');

        me.getAspect('utilityExecProcEditPanelAspect').setData(docId);
        me.getAspect('utilityExecProcCreateButtonAspect').setData(id, type);

        store = view.down('execprocdocumentgrid').getStore();
        store.clearFilter(true);
        store.filter('docId', docId);
    }
});