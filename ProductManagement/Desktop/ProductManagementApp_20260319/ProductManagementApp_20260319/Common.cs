using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProductManagementApp_20260319
{
public static class Common
    {
        public static void Show(this string message)
        {
            MessageBox.Show(message);
        }
        public static void Err(this string message)
        {
            throw new Exception(message);
        }
        public static bool IsNullOrEmpty(this string message)
        {
            return string.IsNullOrEmpty(message);
        }
    }
}
