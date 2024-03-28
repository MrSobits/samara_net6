Ext.define('B4.controller.regop.report.PaymentDocumentTemplate', {
    extend: 'B4.base.Controller',

    requires: [
       'B4.aspects.GridEditWindow',
       'B4.mixins.Context'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    views: [
        'regop.report.PaymentDocumentTemplateGrid',
        'regop.report.PaymentDocumentTemplateEdit'
    ],

    models: [
        'regop.report.PaymentDocumentTemplate'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'paydoctemplategrid'
        }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            gridSelector: 'paydoctemplategrid',
            editFormSelector: 'paydoctemplateedit',
            modelName: 'regop.report.PaymentDocumentTemplate',
            editWindowView: 'regop.report.PaymentDocumentTemplateEdit',
            listeners: {
                aftersetformdata: function (asp, rec, form) {
                    if (rec.get('TemplateCode') && rec.get('Period')) {
                        form.down('b4filefield[name=TemplateFile]').setValue({
                            id: {
                                templateCode: rec.get('TemplateCode'),
                                periodId: rec.get('Period')
                            },
                            name: 'Шаблон'
                        });
                    }
                    var isNew = rec.get('Id') == 0,
                        periodField = form.down('[name=Period]'),
                        templateField = form.down('[name=TemplateCode]');

                    periodField.setDisabled(!isNew);
                    templateField.setDisabled(!isNew);
                }
            }
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('paydoctemplategrid');
        me.bindContext(view);
        me.application.deployView(view);
    },

    init: function () {
        var me = this;

        me.control({
            'menuitem[action=DownloadOrigin]': {
                'click': {
                    fn: me.onDownloadOriginBtnClick,
                    scope: me
                }
            },
            'menuitem[action=DownloadEmpty]': {
                'click': {
                    fn: me.onDownloadEmptyBtnClick,
                    scope: me
                }
            },
            'b4combobox[name=TemplateCode]': {
                'change': {
                    fn: me.onCodedReportComboChange,
                    scope: me
                }
            }
        });

        me.callParent(arguments);
    },

    onDownloadOriginBtnClick: function (btn) {
        var code = btn.up('paydoctemplateedit').down('b4combobox[name=TemplateCode]');
        this.downloadOriginTemplate(code.getValue());
    },

    onDownloadEmptyBtnClick: function (btn) {
        var code = btn.up('paydoctemplateedit').down('b4combobox[name=TemplateCode]'),
            period = btn.up('paydoctemplateedit').down('b4selectfield[name=Period]');

        this.downloadEmptyTemplate(code.getValue(), period.getValue());
    },

    downloadOriginTemplate: function (templateCode) {
        window.open(B4.Url.action('DownloadTemplate', 'PaymentDocReportManager', {
            templateCode: templateCode,
            original: true
        }), '_original');
    },

    downloadEmptyTemplate: function (templateCode, periodId) {
        window.open(B4.Url.action('DownloadTemplate', 'PaymentDocReportManager', {
            templateCode: templateCode,
            periodId: periodId
        }));
    },

    onCodedReportComboChange: function (combo, newVal) {
        var downloadBtn = combo.up('paydoctemplateedit').down('[action=DownloadOrigin]'),
            downloadTplBtn = combo.up('paydoctemplateedit').down('[action=DownloadEmpty]');
        downloadBtn.setDisabled(!newVal);
        downloadTplBtn.setDisabled(!newVal);
    }
});