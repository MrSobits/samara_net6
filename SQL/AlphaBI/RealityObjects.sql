select id as "Id ����", 
       address as "�����",
       case when ro.type_house = 0 then '�� ������'
            when ro.type_house = 10 then '������������� ���������'
            when ro.type_house = 20 then '��������������'
            when ro.type_house = 30 then '���������������'
            when ro.type_house = 40 then '���������'
            end as "��� ����",
            ro.build_year as "��� ���������",
            ro.physical_wear as "���������� �����",
            ro.area_mkd as "����� ������� ����",
            ro.Area_Living as "� �.�. ����� �����",
            ro.floors as "���������",
            ro.maximum_floors as "������������ ���������",
            ro.Number_Apartments as  "���������� �������",
            case when ro.method_form_fund = 10 then '�� ����� ������������� ���������'
                 when ro.method_form_fund = 20 then '�� ����������� �����'
                 when ro.method_form_fund = 0 then '�� ������' end as "C����� ������������ ����� ��"
from gkh_reality_object ro