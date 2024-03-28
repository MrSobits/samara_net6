Ext.define('B4.signalR.Hub', {

    /**
     * Наименование хаба
     * @property {string}
     * */
    name: '',

    /**
     * Клиентские обработчики методов хаба
     * @property {object}
     * */
    handlers: {},

    /**
     * Подключение
     * @property {object}
     * */
    connection: null,

    /**
     * Возвращает относительный адрес хаба
     * @returns {string}
     * */
    getUrl: function () {
        return Ext.String.format(
            'signalr/{0}',
            (this.name || '').toLowerCase()
        );
    },

    /**
     * Возвращает строителя подключения
     * @returns {object}
     * */
    getConnectionBuilder: function () {
        return new signalR
            .HubConnectionBuilder()
            .withUrl(this.getUrl())
            .withAutomaticReconnect()
            .configureLogging(signalR.LogLevel.Error);
    },

    /**
     * Инициализация хаба
     * */
    init: function () {
        const me = this;

        if (Ext.isEmpty(me.name)) {
            me.name = Ext.getClassName(me).split('.').pop();
        }

        me.handlers = {};
        me.initHandlers(me.handlers);

        const connection = me
            .getConnectionBuilder()
            .build();

        me.connection = connection;
        me.configureConnection(me.connection);

        // Регистрируем клиентские обработчики методов хаба
        Ext.iterate(me.handlers, (methodName, handler) => {
            if (Ext.isFunction(handler)) {
                connection.on(methodName, handler);
            }
        });
    },

    /**
     * Метод для регистрации клиентских обработчиков
     * @param {object} handlers Объект вида { <Наименование метода>: <Функция-обработчик> }
     * */
    initHandlers: function (handlers) {
    },

    /**
     * Дополнительный метод настройки подключения
     * */
    configureConnection: function (connection) {
    },

    /**
     * Вызвать метод хаба на сервере
     * @param {string} methodName Наименование метода
     * @param {any[]} args Параметры метода
     * */
    invoke: function (methodName, args) {
        const me = this;

        me.connection
            .invoke
            .apply(me.connection, [methodName, ...Ext.Array.from(args)])
            .catch(error => {
                console.error(
                    Ext.getClassName(me),
                    'Ошибка при вызове метода',
                    {
                        methodName: methodName,
                        args: args
                    },
                    error
                );
            });
    },
    
    start: function (){
        this.connection
            .start()
            .catch(error => {
                console.error(
                    Ext.getClassName(this),
                    'Ошибка при открытии подключения',
                    error
                );
            });
    },
    
    stop: function () {
        this.connection.stop();
    }
});
