Ext.define('B4.controller.al.ReportCustom', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.mixins.Context'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    views: [
        'al.ReportCustomGrid',
        'al.ReportCustomEdit'
    ],

    models: [
        'al.ReportCustom'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'reportcustomgrid'
        }
    ],
    stores: [
        'B4.store.ReportCustomSelect'
    ],
    aspects: [
        {
            xtype: 'grideditwindowaspect',
            gridSelector: 'reportcustomgrid',
            editFormSelector: 'reportcustomedit',
            modelName: 'al.ReportCustom',
            editWindowView: 'al.ReportCustomEdit',
            listeners: {
                aftersetformdata: function (asp, rec, form) {
                    if (rec.get('CodedReportKey')) {
                        form.down('b4filefield[name=TemplateFile]').setValue({
                            id: rec.get('CodedReportKey'),
                            name: 'Шаблон'
                        });
                    }
                }
            }
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('reportcustomgrid');
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
            }
            //'b4combobox[name=CodedReportKey]': {
            //    'change': {
            //        fn: me.onCodedReportComboChange,
            //        scope: me
            //    }
            //}
        });

        me.callParent(arguments);
    },

    onDownloadOriginBtnClick: function (btn) {
        var combo = btn.up('reportcustomedit').down('b4combobox[name=CodedReportKey]');
        this.downloadOriginTemplate(combo.getValue());
    },

    onDownloadEmptyBtnClick: function (btn) {
        var combo = btn.up('reportcustomedit').down('b4combobox[name=CodedReportKey]');
        this.downloadEmptyTemplate(combo.getValue());
    },

    downloadOriginTemplate: function (codedReportKey) {
        window.open(B4.Url.action('DownloadTemplate', 'CodedReportManager', {
            CodedReportKey: codedReportKey,
            Original: true
        }), '_blank');
    },

    downloadEmptyTemplate: function (codedReportKey) {
        window.open(B4.Url.action('DownloadEmptyTemplate', 'CodedReportManager', {
            CodedReportKey: codedReportKey
        }), '_blank');
    },

    onCodedReportComboChange: function (combo, newVal) {
        var downloadBtn = combo.up('reportcustomedit').down('[action=DownloadOrigin]'),
            downloadTplBtn = combo.up('reportcustomedit').down('[action=DownloadEmpty]');
        downloadBtn.setDisabled(!newVal);
        downloadTplBtn.setDisabled(!newVal);
    }
});