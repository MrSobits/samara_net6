Ext.define('B4.model.smevpremises.SMEVPremises', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVPremises'
    },
    fields: [
        { name: 'ReqId'},
        { name: 'RequestDate'},
        { name: 'Inspector'},
        { name: 'RequestState' },
        { name: 'Answer' },
        { name: 'CalcDate' },
        { name: 'MessageId' },
        { name: 'OKTMO' },
        { name: 'ActNumber' },
        { name: 'ActDate' },
        { name: 'ActName' },
        { name: 'ActDepartment' },

        { name: 'EmployeeName' }, //ФИО сотрудника обработавшего запрос
        { name: 'EmployeePost' }, //Должность сотрудника обработавшего запрос
        { name: 'Department' }, //Наименование органа
        { name: 'PremisesInfo' }, //Сведения о жилом помещении/многоквартирном доме
        //Адрес жилого помещения/многоквартирного дома
        { name: 'Region' }, //Регион
        { name: 'District' }, //Район
        { name: 'City' }, //Город
        { name: 'Locality' }, //Населенный пункт
        { name: 'Street' }, //Улица
        { name: 'House' }, //Дом
        { name: 'Housing' }, //Корпус
        { name: 'Building' }, //Строение
        { name: 'Apartment' }, //Квартира
        { name: 'Index' }, //Индекс

        { name: 'CadastralNumber' }, //Кадастровый номер
        { name: 'PropertyRightsDate' }, //Дата возникновения права собственности
        //Реквизиты документов - оснований возникновения права собственности или иного вещного права
        { name: 'DocRightNumber' },
        { name: 'DocRightDate' },

        { name: 'RightholderInfo' }, //Cведения о правообладателе
        { name: 'SupervisionDetails' }, //Реквизиты заключений (актов) соответствующих органов государственного надзора
        //Реквизиты акта обследования помещения
        { name: 'InsNumber' },
        { name: 'InsDate' },
        //Реквизиты заключения о признании жилого помещения пригодным (непригодным) для постоянного проживания
        { name: 'ConNumber' },
        { name: 'ConDate' }
    ]
});