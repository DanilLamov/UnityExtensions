using UnityEngine;

namespace Lamov.UnityExtensions.Runtime.ColorsModule
{
    public static class ColorEnumExtensions
    {
        public static Color ToColor(this ColorEnum colorEnum)
        {
            return colorEnum switch
            {
                ColorEnum.Black => Color.black,
                ColorEnum.White => Color.white,
                ColorEnum.Green => Color.green,
                ColorEnum.Red => Color.red,
                _ => Color.white
            };
        }
    }
}