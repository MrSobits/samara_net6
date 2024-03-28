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

        { name: 'EmployeeName' }, //��� ���������� ������������� ������
        { name: 'EmployeePost' }, //��������� ���������� ������������� ������
        { name: 'Department' }, //������������ ������
        { name: 'PremisesInfo' }, //�������� � ����� ���������/��������������� ����
        //����� ������ ���������/���������������� ����
        { name: 'Region' }, //������
        { name: 'District' }, //�����
        { name: 'City' }, //�����
        { name: 'Locality' }, //���������� �����
        { name: 'Street' }, //�����
        { name: 'House' }, //���
        { name: 'Housing' }, //������
        { name: 'Building' }, //��������
        { name: 'Apartment' }, //��������
        { name: 'Index' }, //������

        { name: 'CadastralNumber' }, //����������� �����
        { name: 'PropertyRightsDate' }, //���� ������������� ����� �������������
        //��������� ���������� - ��������� ������������� ����� ������������� ��� ����� ������� �����
        { name: 'DocRightNumber' },
        { name: 'DocRightDate' },

        { name: 'RightholderInfo' }, //C������� � ���������������
        { name: 'SupervisionDetails' }, //��������� ���������� (�����) ��������������� ������� ���������������� �������
        //��������� ���� ������������ ���������
        { name: 'InsNumber' },
        { name: 'InsDate' },
        //��������� ���������� � ��������� ������ ��������� ��������� (�����������) ��� ����������� ����������
        { name: 'ConNumber' },
        { name: 'ConDate' }
    ]
});