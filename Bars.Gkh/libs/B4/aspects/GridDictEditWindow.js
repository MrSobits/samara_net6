/*
    Данный аспект расширает функционал аспекта с типом "grideditwindowaspect":
    Добавлена поддержка взаимодействия одного грида с несколькими окнами редактирования.
    Для этого необходимо:
            1) указать наименование поля модели, из которого будет получен ключ
            2) задать словарь modelAndEditWindowMap (переопределить метод setModelAndEditWindowDict),
        из которого по ключу будут определяться значения для подмены modelName, editFormSelector, editWindowView

    Пример:
    { 
        xtype: 'griddicteditwindowaspect',
        name: 'gridDictEditWindowAspect',
        gridSelector: 'gridSelector',
        entityPropertyName: 'ModelType',
        setModelAndEditWindowDict: function () {
            var me = this;

            // Для добавления новой записи
            me.modelAndEditWindowMap.set(0, ['Наименование0', 'Наименование0', 'Наименование0']);

            B4.enums.ModelType.getItems().forEach(function (item) {
                var key, properties;
                switch (item[0]) {
                    case B4.enums.ModelType.First:
                        key = B4.enums.ModelType.First;
                        properties = ['Наименование1','Наименование1','Наименование1'];
                        break;
                    case B4.enums.ModelType.Second:
                        key = B4.enums.ModelType.Second;
                        properties = ['Наименование2','Наименование2','Наименование2'];
                        break;
                    default:
                        return;
                }
                me.modelAndEditWindowMap.set(key, properties);
            });
        }
    }
*/
Ext.define('B4.aspects.GridDictEditWindow', {
    extend: 'B4.aspects.GridEditWindow',

    alias: 'widget.griddicteditwindowaspect',

    // Ключ, по которому из словаря будут получены 
    // наименования modelName, editFormSelector, editWindowView
    // для подмены по-умолчанию (при добавлении новой записи)
    defaultMapMatchingKey: 0,

    // Наименование поля модели, хранящее ключ, 
    // по которому из словаря будут получены наименования 
    // для подмены modelName, editFormSelector, editWindowView
    entityPropertyName: null,

    // Словарь в виде 
    // {
    //     'Key1': [modelName1, editFormSelector1, editWindowView1],
    //     'Key2': [modelName2, editFormSelector2, editWindowView2]
    // }
    modelAndEditWindowMap: null,

    // Редактирование записи грида по двойному клику
    doubleClickActionEnabled: false,

    init: function (controller) {
        var me = this,
            actions = {};

        me.modelAndEditWindowMap = new Map();

        // Заполняем словарь modelAndEditWindowMap
        me.setModelAndEditWindowDict();

        // Подменяем modelName, editFormSelector, editWindowView
        me.setCurrentModelAndEditWindow(me.defaultMapMatchingKey);

        me.callParent(arguments);

        if (me.editFormSelector) {
            actions[me.editFormSelector + ' b4closebutton'] = {
                'click': {
                    fn: me.closeWindowHandler,
                    scope: me
                }
            };
        }

        controller.control(actions);
    },

    rowAction: function (grid, action, record) {
        var me = this;

        if (!grid || grid.isDestroyed) return;
        if (this.fireEvent('beforerowaction', me, grid, action, record) !== false) {
            var key = record.get(me.entityPropertyName);

            // Перед выполнением действия нужно проставить
            // соответствующие для выбранной строки наименования
            // modelName, editFormSelector, editWindowView
            if (this.setCurrentModelAndEditWindow(key) === false) return;

            switch (action.toLowerCase()) {
                case 'doubleclick':
                    if (me.doubleClickActionEnabled){
                        this.editRecord(record);
                    }
                    break;
                case 'edit':
                    this.editRecord(record);
                    break;
                case 'delete':
                    this.deleteRecord(record);
                    break;
            }
        }
    },

    gridAction: function (grid, action) {
        if (!grid || grid.isDestroyed) return;

        // Перед выполнением действия нужно проставить
        // наименования modelName, editFormSelector, editWindowView
        // (полученные по defaultMapMatchingKey)
        if (this.setCurrentModelAndEditWindow(this.defaultMapMatchingKey) === false) return;

        if (this.fireEvent('beforegridaction', this, grid, action) !== false) {
            switch (action.toLowerCase()) {
                case 'add':
                    this.editRecord();
                    break;
                case 'update':
                    this.updateGrid();
                    break;
            }
        }
    },

    // Проставить значения свойств modelName, editFormSelector, editWindowView
    setCurrentModelAndEditWindow: function (mapMatchingKey){
        var me = this,
            properties = me.modelAndEditWindowMap.get(mapMatchingKey);

        if (properties === undefined) return false;

        me.modelName = properties[0];
        me.editFormSelector = properties[1];
        me.editWindowView = properties[2];
    },

    // Переопределяем, чтобы задать
    // словарь modelAndEditWindowMap
    setModelAndEditWindowDict: function (){
    }
});