/**
 * Контроллер журнала предупреждений при импорте оплаты в закрытый период
 */
Ext.define('B4.controller.import.WarningInPaymentsToClosedPeriodsImport', {
    extend: 'B4.base.Controller',
    views: [
        'import.WarningInPaymentsToClosedPeriodsImportPanel',
        'import.PersonalAccountCompareInPaymentsGrid'
    ],
    mainView: 'import.WarningInPaymentsToClosedPeriodsImportPanel',
    mainViewSelector: 'warninginpaymentstoclosedperiodsimportpanel',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    /** 
     * Инициализировать
     */
    init: function () {
        var me = this;
        me.control({
            'warninginpaymentstoclosedperiodsimportpanel': {
                'render': { fn: me.onMainViewRender, scope: me },
                'select': { fn: me.onRowSelect, scope: me },
                'deselect': { fn: me.onRowSelect, scope: me },
                rowaction: me.onRowAction
            },
            'warninginpaymentstoclosedperiodsimportpanel button[action=ConfirmAutoCompare]': {
                'click': { fn: me.onClickConfirmAutoCompare }
            },
            'warninginpaymentstoclosedperiodsimportpanel button[action=ManualCompare]': {
                'click': { fn: me.onClickManualCompare }
            },
            // Диалогове окно ручного сопоставления
            'compareinpaymentsgrid button[action=Accept]': {
                'click': { fn: me.onClickAcceptComparing },
            },
            'compareinpaymentsgrid gridpanel': {
                'select': { fn: me.onCompareGridSelect, scope: me },
                'deselect': { fn: me.onCompareGridSelect, scope: me }
            }
        });
        me.callParent(arguments);
    },
    
    /**
     * Показать и запустить. 
     * По умолчанию, запустить - это метод index. Но для данного контроллера задан свой метод.
     * @param {int} id - Идентификатор записи из журнала импортов, к которой заполнен журнал предупреждений.
     */
    show: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);

        // Запомнить идентификатор. Будет использоваться при запросах к хранилищу.
        me.logImportId = id;

        me.bindContext(view);
        me.application.deployView(view);
    },

    /**
     * Обработать событие просчёта отображения
     */
    onMainViewRender: function (grid) {
        var me = this;
        // Повесить обработчики на событие "перед началом загрузки хранилища".
        // В этом событии будут цепляться дополнительные параметры к запросу в хранилище.
        grid.down('#dateWarningsGrid').getStore().on('beforeLoad', me.onStoreBeforeLoad, me);
        grid.getStore().on('beforeLoad', me.onStoreBeforeLoad, me);
    },

    /**
     * Обработать событие начала загрузки хранилища
     */
    onStoreBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.logImportId = me.logImportId; // Идентификатор записи из журнала импортов, к которой заполнен журнал предупреждений
    },

    /**
     * Обработчик нажатия на кнопку подтверждения автоматического сопоставления.
     * @param {b4button} btn - Кнопка.
     */
    onClickConfirmAutoCompare: function (btn) {        
        var me = this,
            grid = me.getMainView(),
            selectedRecords = grid.getSelectionModel().getSelection();        

        // Здесь не делается проверка на то, есть ли выделенные записи, т.к. реализована блокировка кнопки

        me.mask("Подтверждение автоматического сопоставления...");

        var warnings = []; // Массив идентификаторов записей, которые подтверждаются
        Ext.Array.each(selectedRecords, function (rec) {
            warnings.push(rec.get('Id'));            
        });
        B4.Ajax.request({
            url: B4.Url.action('ConfirmAutoComparePersonalAccount', 'ClosedPeriodsImport'),
            params: {
                warnings: warnings.join(',')                
            },
            timeout: 10 * 60 * 1000 // 10 мин. т.к. процесс может быть длительным
        }).next(function () {
            me.unmask();
            Ext.Msg.show({
                title: 'Сопоставление ЛС',
                msg: 'Автоматическое сопоставление прошло успешно',
                width: 300,
                buttons: Ext.Msg.OK,
                icon: Ext.MessageBox.INFO
            });            
            grid.getStore().load();
        }).error(function (response) {
            me.unmask();
            var resp = Ext.isEmpty(response.responseText) ? response : Ext.JSON.decode(response.responseText);            
            Ext.Msg.show({
                title: 'Ошибка',
                msg: resp.message,
                width: 300,
                buttons: Ext.Msg.OK,
                icon: Ext.MessageBox.ERROR
            });
        });
    },
    
    /**
     * Обработчик нажатия кнопки ручного сопоставления.     
     * @param {b4button} btn - Кнопка.
     */
    onClickManualCompare: function (btn) {
        var me = this,
            grid = me.getMainView(),
            selectedRecord = grid.getSelectionModel().getSelection()[0];
        // Показать диалоговое окно выбора ЛС
        me.showManualCompareDialog(selectedRecord);
    },

    /**
     * Обработать событие установки или снятия галочки на записи.
     * Здесь обыгрывется блокировка кнопки "Подтвердить",
     * в зависимости от того, выбраны ли записи. 
     */
    onCompareGridSelect: function (rowModel, record, index, eOpts) {
        var me = this,
            grid = me.paGrid.down("gridpanel"),
            button = me.paGrid.down("button[action=Accept]");
        button.setDisabled(false);
        if (grid.getSelectionModel().getSelection().length !== 1) { // Должна быть выбрана ровно 1 запис
            button.setDisabled(true);
        }
    },

    /**
     * Обработчик нажатия кнопки принятия ручного сопоставления.
     * Кнопка находится в диалоговом окне PersonalAccountCompareGrid.
     * @param {b4button} btn - Кнопка.
     */
    onClickAcceptComparing: function (btn) {
        var me = this,
            grid = me.paGrid.down('gridpanel'),
            record = grid.getSelectionModel().getSelection()[0], // Запись, выбранная в диалоговом окне
            warningGrid = me.getMainView();

        me.mask("Сопоставление ЛС...");
        B4.Ajax.request({
            url: B4.Url.action('ManualComparePersonalAccount', 'ClosedPeriodsImport'),
            params: {
                warningId: me.paGrid.warningId, // Заранее записанный идентификатор записи журнала предупреждений
                compareToAccountId: record.get("Id") // Идентификатор лицевого счёта, на который сопоставляется
            },
            timeout: 2 * 60 * 1000 // 2 мин. (с запасом на загрузку сервера, т.к. обычно импорты делаются под закрытие отчётного периода)
        }).next(function () {
            me.unmask();                
            Ext.Msg.show({
                title: 'Сопоставление ЛС',
                msg: 'Сопоставление ЛС прошло успешно',
                width: 300,
                buttons: Ext.Msg.OK,
                icon: Ext.MessageBox.INFO
            });
            me.paGrid.close();
            warningGrid.getStore().load();
        }).error(function (response) {
            me.unmask();
            var resp = Ext.isEmpty(response.responseText) ? response : Ext.JSON.decode(response.responseText);
            Ext.Msg.show({
                title: 'Ошибка',
                msg: resp.message,
                width: 300,
                buttons: Ext.Msg.OK,
                icon: Ext.MessageBox.ERROR
            });            
        });
    },    

    /**
     * Обработать действия со строкой грида
     */
    onRowAction: function (grid, action, rec) {
        var me = this;
        if (action.toLowerCase() === 'manualcompare') {
            // Показать диалоговое окно выбора ЛС для ручного сопоставления
            me.showManualCompareDialog(rec);            
        }
    },

    /**
     * Показать диалоговое окно выбора ЛС для ручного сопоставления.
     * @param {ЗаписьГрида} rec - Запись грида журнала предупреждений.
     */
    showManualCompareDialog: function (rec) {
        var me = this;
        // Создать диалоговое окно и положить его в поле класса. Чтобы потом достать в onClickAcceptComparing.        
        me.paGrid = me.getView('B4.view.import.PersonalAccountCompareInPaymentsGrid').create(
            {
                constrain: true,                
                renderTo: B4.getBody().getActiveTab().getEl(),
                closeAction: 'destroy'
            });
        me.paGrid.warningId = rec.get('Id'); // Записать идентификатор записи из журнала. Чтобы потом достать в onClickAcceptComparing.

        // Для удобства, в заголовок диалогового окна вывести: ФИО - Адрес из грида (если они заполнены)
        var name = rec.get('Name');
        var address = rec.get('Address');
        if (name != null && name != '' && address != null && address != '')
        {
            me.paGrid.setTitle(name + ' - ' + address);
        }        

        me.paGrid.show();
        me.paGrid.down('gridpanel').getStore().load();
    },

    /**
     * Обработать событие установки или снятия галочки на записи.
     * Здесь обыгрывется блокировка кнопок автоматического и ручного сопоставлений,
     * в зависимости от того, выбраны ли записи.
     */
    onRowSelect: function ( rowModel, record, index, eOpts ) {
        var me = this,
            grid = me.getMainView(),
            selectedRecords = grid.getSelectionModel().getSelection(),
            confirmAutoCompareButton = grid.down('#confirmAutoCompareButton'),
            manualCompareButton = grid.down('#manualCompareButton');
        
        // Подход оптимистичный. Т.е. сначала кнопки разблокировать, а блокировать только по необходимости.

        confirmAutoCompareButton.setDisabled(false);
        manualCompareButton.setDisabled(false);

        // Ни одна запись не выбрана
        if (selectedRecords.length === 0) {
            // Сопоставление невозможно
            confirmAutoCompareButton.setDisabled(true);
            manualCompareButton.setDisabled(true);
            return; // Дальше можно не проверять
        }
        // Выбрано больше одной записи
        if (selectedRecords.length > 1) {
            manualCompareButton.setDisabled(true); // Ручное сопоставление работает только по одной записи. Заблокировать кнопку.
        }
        // Выбрана хоть одна запись, которую нельзя автоматически сопоставить
        Ext.Array.each(selectedRecords, function (rec) {
            if (rec.get('IsCanAutoCompared') === 20 /*No*/) {
                confirmAutoCompareButton.setDisabled(true); // Заблокировать кнопку
                return false; // Break
            }
        });
    }    
});