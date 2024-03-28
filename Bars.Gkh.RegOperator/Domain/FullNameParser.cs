namespace Bars.Gkh.RegOperator.Domain
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class FullNameParser
    {
        public FullName Parse(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                return null;
            }

            var normalizedName = Normalize(fullName);

            if (!ValidateName(normalizedName))
            {
                return null;
            }

            var splitted = normalizedName.Split(' ');
            string firstName, secondName, lastName;

            if (splitted.Length == 2)
            {
                if (fullName.Trim().Contains('.') && !fullName.Trim().EndsWith("."))
                {
                    firstName = UpperFirst(splitted[0]);
                    secondName = string.Empty;
                    lastName = UpperFirst(splitted[1]);
                }
                else
                {
                    firstName = UpperFirst(splitted[1]);
                    secondName = string.Empty;
                    lastName = UpperFirst(splitted[0]);
                }
            }
            else
            {
                if (fullName.Trim().Contains('.') && !fullName.Trim().EndsWith("."))
                {
                    firstName = UpperFirst(splitted[0]);
                    secondName = UpperFirst(splitted[1]);
                    lastName = UpperFirst(splitted[2]);
                }
                else
                {
                    firstName = UpperFirst(splitted[1]);
                    secondName = UpperFirst(splitted[2]);
                    lastName = UpperFirst(splitted[0]);
                }
            }


            return new FullName
            {
                FirstName = firstName,
                SecondName = secondName,
                LastName = lastName
            };
        }

        private bool ValidateName(string fullName)
        {
            var negatives = new[] { "НЕ ЗАДАНО", "НЕЗАДАНО", "НЕТ ДАННЫХ", "НЕТДАННЫХ" };
            var regex = new Regex(@"^[a-zA-Z0-9а-яА-Я\- ]+$");
            return regex.IsMatch(fullName) && !negatives.Any(fullName.Contains) && fullName.Split(' ').Length <= 3 && fullName.Split(' ').Length > 1;
        }

        private string Normalize(string fullName)
        {
            var normalizedName = Regex.Replace(fullName.Replace('.', ' ').ToUpper().Trim(), @"\s+", " ");
            return normalizedName;
        }

        private string UpperFirst(string text)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
        }
    }

    public sealed class FullName
    {
        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string LastName { get; set; }
    }

    public class NameValidationException : Exception
    {
        public NameValidationException()
            : base("Ошибка при валидации строки, содержащей ФИО")
        {
        }
    }
}
