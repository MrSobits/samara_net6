Ext.define('B4.view.wizard.dictionary.comparedictionaryrecords.CompareRecordsStepFrame', {
    extend: 'B4.view.wizard.WizardBaseStepFrame',
    wizard: undefined,
    stepId: 'compareRecords',
    title: 'Сопоставление записей',
    layout: 'fit',
    requires: [
        'B4.view.dictionaries.RecordComparisonGrid'
    ],

    initComponent: function () {
        var me = this;

        me.items = [
            {
                xtype: 'recordcomparisongrid'
            }
        ];

        me.callParent(arguments);
    },

    init: function() {
        var me = this,
            grid = me.down('recordcomparisongrid'),
            store = grid.getStore(),
            gisRecStore = grid.getGisRecStore();

        gisRecStore.loadData(me.wizard.comparisonResult.GisRecords);
        store.loadData(me.wizard.comparisonResult.Records);
    },

    doBackward: function () {
        this.wizard.setCurrentStep('packagesPreview');
    },

    doForward: function () {
        var me = this,
            grid = me.down('recordcomparisongrid'),
            store = grid.getStore(),
            records = [];

        store.each(function (rec) {
            records.push(rec.getData());
        });

        me.wizard.mask();
        B4.Ajax.request({
            url: B4.Url.action('PersistRecordComparison', 'Dictionary'),
            params: {
                dictionaryCode: me.wizard.dictionaryCode,
                records: Ext.encode(records)
            },
            timeout: 9999999
        }).next(function () {
            me.wizard.result = {
                state: 'success',
                message: 'Сопоставление записей справочника успешно выполнено.'
            }

            me.wizard.setCurrentStep('finish');

            me.wizard.unmask();

        }, me).error(function (e) {
            me.wizard.result = {
                state: 'error',
                message: e.message || 'Не удалось сопоставить записи справочника'
            };
            me.wizard.setCurrentStep('finish');
            me.wizard.unmask();
        }, me);

        return;
    }
});
