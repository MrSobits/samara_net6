Ext.define('B4.view.wizard.dictionary.comparedictionary.GisDictionariesStepFrame', {
    extend: 'B4.view.wizard.WizardBaseStepFrame',
    wizard: undefined,
    stepId: 'gisDictionaries',
    title: 'Выбор справочника ГИС ЖКХ',
    layout: 'fit',
    requires: [
        'B4.view.dictionaries.GisDictionariesGrid'
    ],

    initComponent: function () {
        var me = this;

        me.items = [
            {
                xtype: 'gisdictionariesgrid',
                listeners: {
                    selectionchange: function() {
                        me.fireEvent('selectionchange', me);
                    },
                    scope: me
                }
            }
        ];

        me.callParent(arguments);
    },

    init: function() {
        var me = this,
            gisDictionaries = me.wizard.gisDictionaries,
            hasGisDictionaries = gisDictionaries && gisDictionaries.length !== 0,
            gisDictionariesGrid = me.down('gisdictionariesgrid'),
            gisDictionariesStore = gisDictionariesGrid.getStore(),
            recordsCount = gisDictionariesStore.getCount();

        if (recordsCount === 0 && hasGisDictionaries === true) {
            gisDictionariesStore.loadData(gisDictionaries);
        }
    },

    doBackward: function () {
        this.wizard.setCurrentStep('packagesPreview');
    },

    doForward: function () {
        var me = this,
            gisDictionariesGrid = me.down('gisdictionariesgrid'),
            gisDictionariesSelectionModel = gisDictionariesGrid.getSelectionModel(),
            selectedRecord = gisDictionariesSelectionModel.getSelection()[0],
            gisDictionaryRegisryRegistryNumber = selectedRecord.get('RegistryNumber'),
            gisDictionaryGroup = selectedRecord.get('Group');

        me.wizard.mask();
        B4.Ajax.request({
            url: B4.Url.action('CompareDictionary', 'Dictionary'),
            params: {
                dictionaryCode: me.wizard.dictionaryCode,
                gisDictionaryRegisryRegistryNumber: gisDictionaryRegisryRegistryNumber,
                gisDictionaryGroup: gisDictionaryGroup
            },
            timeout: 9999999
        }).next(function (response) {

            me.wizard.result = {
                state: 'success',
                message: 'Сопоставление справочника успешно выполнено.'
            }
            me.wizard.setCurrentStep('finish');

            me.wizard.unmask();

        }, me).error(function (e) {
            me.wizard.result = {
                state: 'error',
                message: e.message || 'Не удалось сопоставить справочник'
            };
            me.wizard.setCurrentStep('finish');
            me.wizard.unmask();
        }, me);

        return;
    },

    allowForward: function () {

        var me = this,
            gisDictionariesGrid = me.down('gisdictionariesgrid'),
            gisDictionariesSelectionModel = gisDictionariesGrid.getSelectionModel(),
            selectedRecords = gisDictionariesSelectionModel.getSelection(),
            result = selectedRecords.length === 1;

        return result;
    }
});
