using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REproject3_1
{
    public static class UtilsClass
    {
        /// <summary>
        /// Метод, возвращающий путь к папке данного проекта с символом-разделителем.
        /// </summary>
        /// <returns></returns>
        public static string GetDirectory()
        {
            string[] dirs = Directory.GetCurrentDirectory().Split(Path.DirectorySeparatorChar);
            return string.Join(Path.DirectorySeparatorChar, dirs, 0, dirs.Length - 3) + Path.DirectorySeparatorChar;
        }
    }
}
