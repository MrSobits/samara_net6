namespace Bars.Gkh.Import.ElevatorsImport
{
    using System;
    using System.Collections.Generic;
    using B4.Utils;
    using Entities;

    public class ElevatorRecord
    {
        public long RoId { get; set; }
        public string PorchNum { get; set; }
        public string Capacity { get; set; }
        public string StopCount { get; set; }
        public string InstallationYear { get; set; }
        public string Lifetime { get; set; }
        public string Year { get; set; }
        public string Period { get; set; }
        public string SerialNumber { get; set; }
        public int RowNumber { get; set; }
        public string Ocenka { get; set; }
        public string Sum { get; set; }

        public TehPassportValue[] CreateElevator(int rowNum, TehPassport tehPassport)
        {
            var result = new List<TehPassportValue>();

            if (!PorchNum.IsEmpty())
            {
                result.Add(CreateTehPassportValue(rowNum, 1, tehPassport, PorchNum));
            }

            int capacity;
            if (!Capacity.IsEmpty() && int.TryParse(Capacity, out capacity))
            {
                result.Add(CreateTehPassportValue(rowNum, 5, tehPassport, capacity.ToString()));
            }

            int stopCount;
            if (!StopCount.IsEmpty() && int.TryParse(StopCount, out stopCount))
            {
                result.Add(CreateTehPassportValue(rowNum, 7, tehPassport, stopCount.ToString()));
            }

            int installationYear;
            if (!InstallationYear.IsEmpty() && int.TryParse(InstallationYear, out installationYear))
            {
                result.Add(CreateTehPassportValue(rowNum, 9, tehPassport, installationYear.ToString()));
            }

            DateTime lifetime;
            if (!Lifetime.IsEmpty() && DateTime.TryParse(Lifetime, out lifetime))
            {
                result.Add(CreateTehPassportValue(rowNum, 17, tehPassport, lifetime.ToShortDateString()));
            }

            int year;
            if (!Year.IsEmpty() && int.TryParse(Year, out year) && year > 0)
            {
                result.Add(CreateTehPassportValue(rowNum, 13, tehPassport, new DateTime(year, 1, 1).ToShortDateString()));
            }

            if (!Period.IsEmpty())
            {
                result.Add(CreateTehPassportValue(rowNum, 18, tehPassport, Period));
            }

            if (!SerialNumber.IsEmpty())
            {
                result.Add(CreateTehPassportValue(rowNum, 2, tehPassport, SerialNumber));
            }

            int ocenka;
            if (!Ocenka.IsEmpty() && int.TryParse(Ocenka, out ocenka))
            {
                result.Add(CreateTehPassportValue(rowNum, 11, tehPassport, ocenka.ToStr()));
            }

            decimal sum;
            if (!Sum.IsEmpty() && decimal.TryParse(Sum, out sum))
            {
                result.Add(CreateTehPassportValue(rowNum, 15, tehPassport, sum.RoundDecimal(2).ToStr()));
            }

            return result.ToArray();
        }

        private TehPassportValue CreateTehPassportValue(int rowNum, int cellCode, TehPassport tehPassport, string value)
        {
            return new TehPassportValue
            {
                CellCode = string.Format("{0}:{1}", rowNum, cellCode),
                FormCode = ElevatorsImport.FormCode,
                TehPassport = tehPassport,
                Value = value
            };
        }

        public void UpdateElevator(ElevatorProxy[] values, out TehPassportValue[] updated, out TehPassportValue[] removed)
        {
            var _updated = new List<TehPassportValue>();
            var _removed = new List<TehPassportValue>();

            foreach (var value in values)
            {
                switch (value.CellCode)
                {
                    case 1:
                        if (!PorchNum.IsEmpty())
                        {
                            value.Value.Value = PorchNum;
                            _updated.Add(value.Value);
                        }
                        else
                        {
                            _removed.Add(value.Value);
                        }
                        break;
                    case 2:
                        if (!SerialNumber.IsEmpty())
                        {
                            value.Value.Value = SerialNumber;
                            _updated.Add(value.Value);
                        }
                        else
                        {
                            _removed.Add(value.Value);
                        }
                        break;
                    case 5:
                        int capacity;
                        if (!Capacity.IsEmpty() && int.TryParse(Capacity, out capacity))
                        {
                            value.Value.Value = capacity.ToString();
                            _updated.Add(value.Value);
                        }
                        else
                        {
                            _removed.Add(value.Value);
                        }
                        break;
                    case 7:
                        int stopCount;
                        if (!StopCount.IsEmpty() && int.TryParse(StopCount, out stopCount))
                        {
                            value.Value.Value = stopCount.ToString();
                            _updated.Add(value.Value);
                        }
                        else
                        {
                            _removed.Add(value.Value);
                        }
                        break;
                    case 9:
                        int installationYear;
                        if (!InstallationYear.IsEmpty() && int.TryParse(InstallationYear, out installationYear))
                        {
                            value.Value.Value = installationYear.ToString();
                            _updated.Add(value.Value);
                        }
                        else
                        {
                            _removed.Add(value.Value);
                        }
                        break;
                    case 13:
                        int year;
                        if (!Year.IsEmpty() && int.TryParse(Year, out year) && year > 0)
                        {
                            value.Value.Value = new DateTime(year, 1, 1).ToShortDateString();
                            _updated.Add(value.Value);
                        }
                        else
                        {
                            _removed.Add(value.Value);
                        }
                        break;
                    case 17:
                        DateTime lifetime;
                        if (!Lifetime.IsEmpty() && DateTime.TryParse(Lifetime, out lifetime))
                        {
                            value.Value.Value = lifetime.ToShortDateString();
                            _updated.Add(value.Value);
                        }
                        else
                        {
                            _removed.Add(value.Value);
                        }
                        break;
                    case 18:
                        if (!Period.IsEmpty())
                        {
                            value.Value.Value = Period;
                            _updated.Add(value.Value);
                        }
                        else
                        {
                            _removed.Add(value.Value);
                        }
                        break;
                }
            }

            updated = _updated.ToArray();
            removed = _removed.ToArray();
        }
    }

    public class ElevatorProxy
    {
        public int RowNum { get; set; }
        public int CellCode { get; set; }
        public TehPassportValue Value { get; set; }
    }
}