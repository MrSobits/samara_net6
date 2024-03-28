Ext.define('B4.controller.belaypolicy.MkdExclude', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhInlineGrid'],

    models: ['belaypolicy.Mkd'],
    stores: ['belaypolicy.MkdExclude'],
    views: ['belaypolicy.MkdExcludeGrid'],

    aspects: [
        {
            /*
            * Аспект инлайн редактирования таблицы
            */
            xtype: 'gkhinlinegridaspect',
            name: 'mkdExcludeGkhInlineGridAspect',
            gridSelector: '#belayPolicyMkdExcludeGrid',
            storeName: 'belaypolicy.MkdExclude',
            modelName: 'belaypolicy.Mkd',
            saveButtonSelector: '#belayPolicyMkdExcludeGrid #saveButton',
            rowAction: function(grid, action, record) {
                if (this.fireEvent('beforerowaction', this, grid, action, record) !== false) {
                    switch (action.toLowerCase()) {
                    case 'delete':
                        this.deleteRecord(record);
                        break;
                    case 'custom':
                        this.backRecord(record);
                        break;
                    }
                }
            },
            //меняем значени IsExcluded на false (то есть становится не исключенным), сохраняем и перезагружаем сторе
            backRecord: function(record) {
                var me = this;
                Ext.Msg.confirm('Включение в договора!', 'Вы действительно хотите включить дом в договор?', function(res) {
                    if (res == 'yes') {
                        record.set('IsExcluded', false);
                        record.save({ Id: record.getId() })
                            .next(function() {
                                this.controller.getStore('belaypolicy.MkdExclude').load();
                                Ext.Msg.alert('Включение в договор!', 'Объект перенесен во включенные договора');
                            }, me)
                            .error(function(result) {
                                Ext.Msg.alert('Включение в договор!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                            }, me);
                    }
                }, me);
            }
        }
    ],

    mainView: 'belaypolicy.MkdExcludeGrid',
    mainViewSelector: '#belayPolicyMkdExcludeGrid',

    init: function() {
        this.getStore('belaypolicy.MkdExclude').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function() {
        this.getStore('belaypolicy.MkdExclude').load();
    },

    onBeforeLoad: function(store, operation) {
        if (this.params) {
            operation.params.belayPolicyId = this.params.get('Id');
            operation.params.isExcluded = true;
        }
    }
});