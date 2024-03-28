namespace Bars.Gkh.Utils
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Linq;

    /// <summary>
    /// Вспомогательные методы для изображений
    /// </summary>
    public static class ImageUtils
    {
        private const int OrientationPropertyId = 0x0112;

        /// <summary>
        /// Повернуть в зависимости от метаданных
        /// </summary>
        /// <param name="image">Исходное изображение</param>
        public static void RotateWithMeta(this Image image)
        {
            var rotateProp = image.GetSafePropertyItem(ImageUtils.OrientationPropertyId);

            if (rotateProp != null)
            {
                var orientationValue = image.GetPropertyItem(rotateProp.Id).Value[0];

                var rotateFlipType = ImageUtils.GetOrientationToFlipType(orientationValue);

                image.RotateFlip(rotateFlipType);
            }
        }

        /// <summary>
        /// Установить ориентацию по-умолчанию
        /// </summary>
        /// <param name="image">Исходное изображение</param>
        public static void ResetOrientation(this Image image)
        {
            var orientProperty = image.GetSafePropertyItem(ImageUtils.OrientationPropertyId);

            if (orientProperty != null)
            {
                orientProperty.Value[0] = 1;

                image.SetPropertyItem(orientProperty);
            }
        }

        private static RotateFlipType GetOrientationToFlipType(int orientationValue)
        {
            RotateFlipType rotateFlipType;

            switch (orientationValue)
            {
                case 1:
                    rotateFlipType = RotateFlipType.RotateNoneFlipNone;
                    break;
                case 2:
                    rotateFlipType = RotateFlipType.RotateNoneFlipX;
                    break;
                case 3:
                    rotateFlipType = RotateFlipType.Rotate180FlipNone;
                    break;
                case 4:
                    rotateFlipType = RotateFlipType.Rotate180FlipX;
                    break;
                case 5:
                    rotateFlipType = RotateFlipType.Rotate90FlipX;
                    break;
                case 6:
                    rotateFlipType = RotateFlipType.Rotate90FlipNone;
                    break;
                case 7:
                    rotateFlipType = RotateFlipType.Rotate270FlipX;
                    break;
                case 8:
                    rotateFlipType = RotateFlipType.Rotate270FlipNone;
                    break;
                default:
                    rotateFlipType = RotateFlipType.RotateNoneFlipNone;
                    break;
            }

            return rotateFlipType;
        }

        /// <summary>
        /// Получает указанный элемент свойств из объекта System.Drawing.Image.
        /// Безопастный метод, в случае отсутствия метаданных исключение не возникает.
        /// </summary>
        /// <param name="image">Наше изображение</param>
        /// <param name="propertyId">Идентификатор (ID) элемента свойств, который нужно получить.</param>
        /// <returns></returns>
        private static PropertyItem GetSafePropertyItem(this Image image, int propertyId)
        {
            var props = image.PropertyItems
                .Where(x => x.Id == propertyId)
                .ToList();

            if (props.Count != 0)
            {
                var rotateProp = image.GetPropertyItem(propertyId);
                return rotateProp;
            }
            else
            {
                return null;
            }
        }
    }
}