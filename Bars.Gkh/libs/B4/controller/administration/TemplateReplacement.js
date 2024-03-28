Ext.define('B4.controller.administration.TemplateReplacement', {
    extend: 'B4.base.Controller',
    params: {},
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],
    models: [
        'administration.TemplateReplacementReport',
        'administration.TemplateReplacement'
    ],
    stores: [
        'administration.TemplateReplacementReport',
        'administration.TemplateReplacement'
    ],

    views: [
        'administration.templatereplacement.ReportGrid',
        'administration.templatereplacement.ReportEditWindow',
        'administration.templatereplacement.EditWindow',
        'administration.templatereplacement.Grid'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'administration.templatereplacement.ReportGrid',
    mainViewSelector: 'templateReplacementReportGrid',

    refs: [{
        ref: 'mainView',
        selector: 'templateReplacementReportGrid'
    }],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Administration.TemplateReplacement.Edit', applyTo: 'b4savebutton', selector: '#templateReplacementEditWindow' }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'templateReplacementReportGridWindowAspect',
            gridSelector: 'templateReplacementReportGrid',
            editFormSelector: '#templateReplacementReportEditWindow',
            storeName: 'administration.TemplateReplacementReport',
            modelName: 'administration.TemplateReplacementReport',
            editWindowView: 'administration.templatereplacement.ReportEditWindow',
            editRecord: function (record) {
                var me = this,
                    id = record.data.Id,
                    form = me.getForm();

                if (id) {
                    me.controller.params.reportId = id;
                    form.down('#tfName').setValue(record.data.Name);
                    form.down('#tfDescription').setValue(record.data.Description);
                    form.show();
                    me.controller.getStore('administration.TemplateReplacement').load();
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'templateReplacementGridWindowAspect',
            gridSelector: '#templateReplacementGrid',
            editFormSelector: '#templateReplacementEditWindow',
            storeName: 'administration.TemplateReplacement',
            modelName: 'administration.TemplateReplacement',
            editWindowView: 'administration.templatereplacement.EditWindow',
            otherActions: function (actions) {
                var me = this;

                actions[me.editFormSelector + ' #btnGetBaseTemplate'] = { 'click': { fn: me.onBtnClick, scope: me } };
            },
            listeners: {
                aftersetformdata: function (asp, rec, form) {
                    form.down('#taDescription').setValue(asp.controller.params.description);
                }
            },
            onBtnClick: function () {
                var me = this;
                window.open(B4.Url.content(Ext.String.format('TemplateReplacement/GetBaseTemplate?reportId={0}&id={1}', me.controller.params.reportId, me.controller.params.id)));
            },
            editRecord: function (record) {
                var me = this,
                    id = record.get('Code'),
                    model = me.getModel(record),
                    win = me.getForm(),
                    fileField = win.down('[name=File]');

                me.controller.params.description = record.get('Description');

                me.controller.params.id = id;

                fileField.possibleFileExtensions = record.get('Extension');
                fileField.setFieldLabel(Ext.String.format('Шаблон замены (только {0})', record.get('Extension')));
                
                model.load(id, {
                    success: function (rec) {
                        me.setFormData(rec);
                    },
                    scope: me
                });
            },
            saveRecordHasUpload: function (rec) {
                var me = this,
                    model = me.getModel(rec);

                me.mask('Загрузка файла...', me.getForm());
                me.getForm().submit({
                    url: rec.getProxy().getUrl({ action: rec.getId() ? 'update' : 'create' }),
                    params: {
                        records: Ext.encode([rec.getData()])
                    },
                    success: function (form, action) {
                        me.unmask();
                        me.updateGrid();
                        if (action.result.data.length > 0) {
                            var id = action.result.data[0] instanceof Object ? action.result.data[0].Id : action.result.data[0];
                            model.load(id, {
                                success: function (newRec) {
                                    me.fireEvent('savesuccess', me, newRec);
                                }
                            });
                        }
                    },
                    failure: function (form, action) {
                        me.unmask();
                        me.fireEvent('savefailure', rec, action.result.message);
                        Ext.Msg.alert('Ошибка сохранения!', action.result.message);
                    }
                });
            }
        }
    ],

    init: function () {
        var me = this;

        me.getStore('administration.TemplateReplacement').on('beforeload', me.onBeforeLoad, me);
        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView();
        if (!view) {
            view = Ext.widget('templateReplacementReportGrid');
            me.bindContext(view);
            me.application.deployView(view);
            me.getStore('administration.TemplateReplacementReport').load();
        }
    },

    onBeforeLoad: function (store, operation) {
        var me = this;

        if (me.params) {
            operation.params.reportId = me.params.reportId;
        }
    }
});