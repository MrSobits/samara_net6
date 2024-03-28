Ext.define('B4.controller.dict.AuditPurposeGji', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.permission.GkhInlineGridPermissionAspect',
        'B4.aspects.GridEditWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context',
        controllerLoader: 'B4.mixins.LayoutControllerLoader'
    },

    models: [
        'dict.AuditPurposeGji',
        'dict.AuditPurposeSurveySubjectGji'
    ],
    stores: [
        'dict.AuditPurposeGji',
        'dict.AuditPurposeSurveySubjectGji'
    ],

    views: [
        'dict.auditpurposegji.EditWindow',
        'dict.auditpurposegji.Grid',
        'dict.auditpurposesurveysubjectgji.Grid'
    ],

    mainView: 'dict.auditpurposegji.Grid',
    mainViewSelector: 'auditPurposeGjiGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'auditPurposeGjiGrid'
        },
        {
            ref: 'survSubjGrid',
            selector: 'auditPurposeSurveySubjectGjiGrid'
        }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'auditPurposeGjiGridWindowAspect',
            gridSelector: 'auditPurposeGjiGrid',
            editFormSelector: '#auditPurposeGjiEditWindow',
            storeName: 'dict.AuditPurposeGji',
            modelName: 'dict.AuditPurposeGji',
            editWindowView: 'dict.auditpurposegji.EditWindow',
            onSaveSuccess: function (me, rec) {
                me.controller.setCurrentId(rec.get('Id'));
            },
            listeners: {
                aftersetformdata: function (me, rec) {
                    var store = me.controller.getStore('dict.AuditPurposeSurveySubjectGji'),
                        documentId = rec.get('Id');
                    //проверяем, если документ не изменился, то грид не обновляем
                    if (documentId != me.controller.documentId) {
                        me.controller.setCurrentId(documentId);
                        store.load();
                    }
                    var grid = me.controller.getSurvSubjGrid();
                    grid.setDisabled(rec.get('Id') == 0);
                },
                getdata: function (asp, records) {
                    var surveySubjects = asp.controller.getSurvSubjGrid().getSelectionModel().getSelection();
                    var surveySubjectIds = [];
                    surveySubjects.forEach(function (entry) {
                        surveySubjectIds.push(entry.getId());
                    });
                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddAuditPurposeSurveySubjectGji', 'AuditPurposeSurveySubjectGji', {
                        auditPurposeId: records.data.Id,
                        surveySubjectIds: surveySubjectIds.join(", ")
                    })).next(function (response) {
                            Ext.Msg.alert('Сохранение!', 'Изменения успешно сохранены');
                            asp.controller.unmask();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });

                    return true;
                }
            },
            closeWindowHandler: function () {
                this.controller.documentId = 0;
                this.getForm().close();
            }
        }
],

    init: function () {
        var me = this;
        var store = me.getStore('dict.AuditPurposeSurveySubjectGji');
        store.on('beforeload', this.onBeforeLoad, this);
        store.on('load', this.onAfterLoad, this);
        me.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('auditPurposeGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
    },

    onBeforeLoad: function (store, operation) {
        if (this.documentId) {
            operation.params.documentId = this.documentId;
        }
    },

    onAfterLoad: function (store, operation) {
        var index = -1;
        var grid = this.getSurvSubjGrid();

        for (var i = 0; i < store.getCount(); i++) {
            index = store.find('Selected', true, i);
            if (index > -1) {
                grid.getSelectionModel().select(index, true);
                i = index;
            }
        }
    },

    setCurrentId: function (id) {
        this.documentId = id;
    }
});